/*
 * Copyright (C) 2015 by Laurentiu-Gheorghe Crisan
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * As a special exception, you may use this file as part of a free
 * software library without restriction. Specifically, if other files
 * instantiate templates or use macros or inline functions from this
 * file, or you compile this file and link it with other files to
 * produce an executable, this file does not by itself cause the
 * resulting executable to be covered by the GNU General Public
 * License. This exception does not however invalidate any other
 * reasons why the executable file might be covered by the GNU Library
 * General Public License.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 */
#include "FuzzyControllerPS.h"
#include <mps/core/SignalList.h>
#include "FuzzyControllerSignal.h"
#include <algorithm>

namespace mps{
namespace controller{

FuzzyControllerPS::FuzzyControllerPS()
: _inputLingVars(0)
, _fuzzyRules(0)
{
}

FuzzyControllerPS::~FuzzyControllerPS()
{
    if(_inputLingVars != 0)
    {
        for( Pt::uint32_t i = 0; i < _inputLingVars->size(); ++i)
            delete _inputLingVars->at(i);

        delete _inputLingVars;
    }

    if(_fuzzyRules != 0)
    {
        for( Pt::uint32_t i = 0; i < _fuzzyRules->size(); ++i)
            delete _fuzzyRules->at(i);

        delete _fuzzyRules;
    }
}

void FuzzyControllerPS::addObject(Object* obj, const std::string& type, const std::string& subType)
{
    if(type == "Mp.Controller.LinguisticVars")
    {
        _inputLingVars = (mps::core::ObjectVector<LinguisticVar*>*) obj;
    }
    else if( type == "Mp.Controller.FuzzyRules")
    {
        _fuzzyRules = (mps::core::ObjectVector<FuzzyRule*>*) obj;
    }
    else
    {
        FiFoSynchSourcePS::addObject(obj, type, subType);
    }
}

void FuzzyControllerPS::updateLingVar(FuzzyOperation* operation)
{
    if(operation->type() == FuzzyOperation::LingVar)
    {
        std::string varName = operation->varName();
        const LinguisticVar* ligVar = getLingVarByName(varName);
        operation->setLinguisticVar(ligVar);
    }
    else
    {
        updateLingVar(operation->left());
        updateLingVar(operation->right());
    }
}

LinguisticVar* FuzzyControllerPS::getLingVarByName(const std::string& name)
{
    const mps::core::Port* port = _outputPorts->at(0);

    const FuzzyControllerSignal* signal = (FuzzyControllerSignal*) port->signalList()->at(0);

    for( Pt::uint32_t i = 0; i < signal->lingVars()->size(); ++i)
    {
        LinguisticVar* var = signal->lingVars()->at(i);
        if(var->varName() == name)
            return var;
    }

    for( Pt::uint32_t i = 0; i <_inputLingVars->size(); ++i)
    {
        LinguisticVar* var = _inputLingVars->at(i);
        if(var->varName() == name)
            return var;
    }

    return 0;
}

const mps::core::Signal* FuzzyControllerPS::getInputSignal(Pt::uint32_t id)
{
    const mps::core::Port* port = _inputPorts->at(0);

    for( Pt::uint32_t i = 0; i <port->signalList()->size(); ++i)
    {
        const mps::core::Signal* signal = port->signalList()->at(i);

        if( signal->signalID() == id)
            return signal;
    }

    return 0;
}

void FuzzyControllerPS::onInitInstance()
{
    FiFoSynchSourcePS::onInitInstance();

    for(Pt::uint32_t i = 0; i < _fuzzyRules->size(); ++i)
    {
        FuzzyRule* rule = _fuzzyRules->at(i);
        updateLingVar(rule->condition());
        updateLingVar(rule->result());
    }

    Pt::uint32_t index = 0;
    std::vector<Pt::uint32_t> sizes;
    std::vector<double> rates;
    double maxRate = 100;

    for( Pt::uint32_t i = 0; i < _inputLingVars->size(); ++i)
    {
        LinguisticVar* lingVar = _inputLingVars->at(i);

        SigId2IdxIt it = _sigId2Idx.find(lingVar->signal());

        if( it == _sigId2Idx.end())
        {
            SigId2IdxItem item;
            item.index = index;            
            item.lingVars.push_back(lingVar);
            
            std::pair<Pt::uint32_t, SigId2IdxItem> pair(lingVar->signal(), item);
            
            _sigId2Idx.insert(pair);

            const mps::core::Signal* signal = getInputSignal(lingVar->signal());
            rates.push_back(signal->sampleRate());
            
            maxRate = std::max(maxRate, signal->sampleRate());
            
            index++;
        }
        else
        {
            it->second.lingVars.push_back(lingVar);
        }
    }

    sizes.resize(rates.size(), sizeof( double));

    const mps::core::Port* outPort = _outputPorts->at(0);
    const mps::core::Signal* outSignal = outPort->signalList()->at(0);
    _recordBuilder.init(static_cast<Pt::uint32_t>(maxRate), sizes, rates, outSignal->sampleRate());
}

void FuzzyControllerPS::onStart()
{
    _recordBuilder.reset();

    FiFoSynchSourcePS::onStart();
}

void FuzzyControllerPS::onReadData()
{
    const Pt::uint8_t* data1;
    Pt::uint32_t count1;
    const Pt::uint8_t* data2;
    Pt::uint32_t count2;

    _recordBuilder.get(&data1, count1, &data2, count2);

    if( count1 != 0)
        processData(data1, count1);

    if( count2 != 0)
        processData(data2, count2);
}

double FuzzyControllerPS::evalRules() 
{
    double sumDegreeOfPerformance = 0;
    double degreeOfPerformance = 0;

    for( Pt::uint32_t i = 0; i < _fuzzyRules->size(); ++i)
    {
        FuzzyRule* rule = _fuzzyRules->at(i);
        double output = 0;
        const double value = rule->evalRule(output);
        sumDegreeOfPerformance += value;
        degreeOfPerformance += ( output* value);
    }
    
    if( sumDegreeOfPerformance != 0)
        return degreeOfPerformance/ sumDegreeOfPerformance;
    
    return 0;
}

void FuzzyControllerPS::processData(const Pt::uint8_t* data, Pt::uint32_t records)
{    
    const double* pvalue = (const double*) data;

    for(Pt::uint32_t rec = 0; rec < records; ++rec)
    {
        SigId2IdxIt it = _sigId2Idx.begin();

        for( ; it != _sigId2Idx.end(); ++it)
        {
            const double& value  = pvalue[_sigId2Idx.size() * rec + it->second.index];

            for( Pt::uint32_t j = 0; j < it->second.lingVars.size(); ++j)
            {
                LinguisticVar* var = it->second.lingVars[j];

                if( j == 0)
                    var->setPosition(-1);
                else if( j == it->second.lingVars.size()- 1)
                    var->setPosition(1);
                else
                    var->setPosition(0);

                var->calcFuzzyValue(value);
            }
        }   

        const double result = evalRules();
        putRecords(0,0,1, (const Pt::uint8_t*) &result);
    }
}

void FuzzyControllerPS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
{
    const mps::core::Sources& sources = port->sources();
    const std::vector<mps::core::Signal*>& source = sources[sourceIdx];

    for( Pt::uint32_t  i = 0; i < source.size(); ++i)
    {
        const mps::core::Signal* signal = source[i];
        SigId2IdxIt it = _sigId2Idx.find(signal->signalID());
        
        if( it == _sigId2Idx.end())
            continue;

        const Pt::uint32_t recordSize = port->sourceDataSize(sourceIdx);
        const Pt::uint32_t offset = port->signalOffsetInSource(sourceIdx, i);

        for( Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
        {
            const Pt::uint8_t* pdata = &data[rec * recordSize + offset];
            const double value = signal->scaleValue(pdata);
            _recordBuilder.insert((const Pt::uint8_t*) &value, it->second.index);
        }
    }
}

}}
