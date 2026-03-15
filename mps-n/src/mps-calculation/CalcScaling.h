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
#ifndef MPS_CALCSCALING_H
#define MPS_CALCSCALING_H

#include <Pt/Types.h>
#include <mps/core/Object.h>
#include <vector>

namespace mps{
namespace calculation{

typedef std::pair<double,double> PointF;

class CalcScaling : public mps::core::Object
{
public:
    CalcScaling();
    virtual ~CalcScaling();

    inline Pt::uint32_t outSignal() const
    {
        return _outSignal;
    }

    inline void setOutSignal(Pt::uint32_t s)
    {
        _outSignal = s;
    }

    inline Pt::uint8_t type() const
    {
        return _type;
    }

    inline void setType(Pt::uint8_t t)
    {
        _type = t;
    }

    inline double factor() const
    {
        return _factor;
    }

    inline void setFactor(double f)
    {
        _factor = f;
    }

    inline double offset() const
    {
        return _offset;
    }

    inline void setOffset( double o)
    {
        _offset = o;
    }

    inline Pt::uint32_t signal() const
    {
        return _signal;
    }

    inline void setSignal(Pt::uint32_t s)
    {
        _signal = s;
    }

    inline double p1x() const
    {
        return _p1x;
    }

    inline void setP1x(double p)
    {
        _p1x = p;
    }

    inline double p1y() const
    {
        return _p1y;
    }

    inline void setP1y(double p)
    {
        _p1y = p;
    }

    inline double p2x() const
    {
        return _p2x;
    }

    inline void setP2x(double p)
    {
        _p2x = p;
    }

    inline double p2y() const
    {
        return _p2y;
    }

    inline void setP2y(double p)
    {
        _p2y = p;
    }

    inline const std::string& table() const
    {
        return _tableStr;
    }

    void setTable(const std::string& table);

    const std::vector<PointF>& tableData() const
    {
        return _table;
    }

private:
    Pt::uint32_t _outSignal;
    Pt::uint8_t _type;
    double _factor;
    double _offset;
    Pt::uint32_t _signal;
    double _p1x;
    double _p1y;
    double _p2x;
    double _p2y;
    std::string _tableStr;
    std::vector<PointF> _table;
};

}}

#endif
