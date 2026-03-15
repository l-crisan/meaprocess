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
#ifndef MPS_LABJACJ_INBOARD_H
#define MPS_LABJACJ_INBOARD_H

#include <Pt/System/Thread.h>
#include <Pt/System/Mutex.h>
#include <map>
#include <vector>
#include "LJSignal.h"
#include "Board.h"
#include <mps/core/FiFoSynchSourcePS.h>

namespace mps{
namespace labjack{

class InBoard : public Board
{
public:
    InBoard(mps::core::FiFoSynchSourcePS* ps);

    virtual ~InBoard(void);

    void init();

    void start();

    void stop();

    void deinit();

    void addSignal(const LJSignal* signal, Pt::uint32_t srcIdx);

    void scanData(Pt::uint32_t records, Pt::uint32_t sourceIdx);

private:
    void scan();

    void scanAnalog();

    void scanDigital();

    void scanCounter();

    static long getFreeChannel(std::vector<long>& channels, bool singleEnded);

private:
    mps::core::FiFoSynchSourcePS* _ps;
    Pt::System::AttachedThread* _scanThread;
    bool _running;        
    bool _scanModeStream;
    const LJSignal* _counterSignal; 
    std::map<Pt::uint32_t, std::vector<const LJSignal*>> _src2signals;
    std::map<Pt::uint32_t, std::vector<Pt::uint8_t>> _data;    
    std::vector<const LJSignal*> _analogSignals;
    std::vector<const LJSignal*> _digitalSignals;   
    std::map<Pt::uint32_t, std::pair<Pt::uint32_t,Pt::uint32_t>> _sigId2SrcOffset;   
    Pt::System::Mutex _mutex;
    float _streamData[4096][4];
    std::vector<long>   _channels;
    std::vector<long>   _gains;
    long _boardID;

};

}}
#endif