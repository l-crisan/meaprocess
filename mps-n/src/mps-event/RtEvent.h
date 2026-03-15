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
#ifndef MPS_EVENTPS_RTEVENT_H
#define MPS_EVENTPS_RTEVENT_H

#include <Pt/Types.h>
#include <mps/core/Object.h>
#include <string>

namespace mps{
namespace eventps{

class RtEvent : public mps::core::Object
{
public:
    enum Operation
    {
        NotEq,
        Eq,
        Ls,
        Le,
        Gr,
        Ge
    };

    RtEvent(void);

    virtual ~RtEvent(void);

    inline const std::string& message() const 
    {
        return _message;
    }
    
    inline void setMessage(const std::string& msg)
    {
        _message = msg;
    }

    inline Pt::uint8_t outputTarget() const
    {
        return _target;
    }

    inline void setOutputTarget(Pt::uint8_t t)
    {
        _target = t;
    }
    
    inline Pt::uint32_t signal() const
    {
        return _signal;
    }

    inline void setSignal(Pt::uint32_t sig)
    {
        _signal = sig;
    }

    inline const std::string& buffer() const
    {
        return _message;
    }

    void setBuffer(const std::string& b);

    inline std::vector<Pt::uint8_t>& audioData()
    {
        return _audioData;
    }

    inline const std::string& command() const
    {
        return _command;
    }

    inline void setCommand(const std::string& c)
    {
        _command = c;
    }
    
    inline const std::string& commandParam() const
    {
        return _commandParam;
    }

    inline void setCommandParam(const std::string& c)
    {
        _commandParam = c;
    }

    inline double limit() const
    {
        return _limit;
    }

    inline void setLimit(double l)
    {
        l = _limit;
    }

    inline Pt::uint8_t operation() const
    {
        return _operation;
    }

    inline void setOperation(Pt::uint8_t o)
    {
        _operation = o;
    }

    inline Pt::uint8_t priority() const
    {
        return _priority;
    }

    inline void setPriority(Pt::uint8_t o)
    {
        _priority = o;
    }

private:
    std::string _message;	
    Pt::uint32_t _signal;
    std::vector<Pt::uint8_t> _audioData;
    std::string _command;
    std::string _commandParam;	
    Pt::uint8_t _target;
    double      _limit;
    Pt::uint8_t _operation;
    Pt::uint8_t _priority;
};


}}

#endif
