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
#ifndef MPS_CORE_SYNCHSOURCEPS_H
#define MPS_CORE_SYNCHSOURCEPS_H

#include <Pt/Types.h>
#include <mps/core/Api.h>
#include <mps/core/ProcessStation.h>
#include <mps/core/Signal.h>
#include <mps/timer/TimerListener.h>

namespace mps {
namespace core {

/**@brief The base class for data input synchronized process station.
*
* A synchronized process station is a process station that receive 
* system timer events and convert this events into source events.
* A source is a group of signals with the same sample rate and with the same 
* physical time source. A source event is fired when the signals in the source 
* should process their data to the output ports.
* 
* To implement an specific data source process station like a CAN-bus
* process station, derive this class and overwrite onSourceEvent. 
*/

class MPS_CORE_API SynchSourcePS  : public ProcessStation , public mps::timer::TimerListener
{
public:
    /** @brief Destructor.*/
    virtual ~SynchSourcePS(void);

    virtual bool isSynchronizedPS() const;
    
    /**@brief Return true if the process station is running.
    *
    * @return True if the process station is running.*/
    virtual bool isActive() const;

    virtual void onInitInstance();

    virtual PSType psType() const
    { return SourcePS; }

    void onStart();
    void onStop();

    void onOverload();

protected:
    /**@brief Default constructor */
    SynchSourcePS(void);

    /**@brief Called by the framework to read source data.
    *
    * @param [in,out] noOfRecords As input the number of requested records and as output the number of written records.
    * @param maxNoOfSamples The maximum number of records which can be written in the stream.
    * @param sourceIdx The source index.
    * @param portIdx The port index.
    * @param data The data stream which receive the requested data.*/
    virtual void onSourceEvent(Pt::uint32_t& noOfRecords, Pt::uint32_t maxNoOfSamples, Pt::uint32_t sourceIdx, Pt::uint32_t portIdx, Pt::uint8_t* data) = 0;

    virtual void onTimer(Pt::uint32_t timerCounter );
    
    inline Pt::uint32_t signalIndex(const Signal* signal) const
    {
        return signal->signalIndex();
    }

private:

    class SourceEventInfo
    {
        public:

            double curretTime;
            double eventOn;
            double noOfSamples;
            double samplesToGet;
    };

    bool _run;
    std::vector<Pt::uint8_t> _dataBuffer;
    std::vector<std::vector<SourceEventInfo> > _sourceEventInfo;
    static	const Pt::uint8_t MAX_BUFFER_FACTOR;	
    
};

}}

#endif
