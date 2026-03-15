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
#ifndef MPS_LABJACK_OUTBOARD_H
#define MPS_LABJACK_OUTBOARD_H

#include <mps/core/Object.h>
#include <mps/core/Signal.h>
#include <vector>
#include "LJOutSignal.h"
#include <Pt/System/Thread.h>
#include "Board.h"

namespace mps{

class Signal;

namespace labjack{

class OutBoard : public Board
{
public:
    OutBoard(void);

    virtual ~OutBoard(void);

    void addSignal(LJOutSignal* signal);

    void init();

    inline std::vector<LJOutSignal*>& channels() 
    {
        return _channels;
    }

    void start();

    void writeData(const mps::core::Signal* signal, const Pt::uint8_t* data);

    void stop();

private:

    void output();

    static double getFactor(double min, double max);

    static double getOffset(double min, double max);

private:
    std::vector<LJOutSignal*> _channels;
    std::vector<LJOutSignal*> _digitalChannels;
    std::vector<LJOutSignal*> _analogChannels;
    std::vector<std::pair<double,double>>  _analogScaling;

    std::vector<float> _analogData;
    std::vector<Pt::uint8_t>  _digitalData;

    Pt::System::AttachedThread* _outThread;
    Pt::uint32_t _sampleTime;
    bool _running;
};

}}
#endif
