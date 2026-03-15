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
#ifndef MPS_IIRFILTER_ELLIPTICCOEF_H
#define MPS_IIRFILTER_ELLIPTICCOEF_H

#include <vector>

#include "Elliptic.h"

namespace mps{
namespace filter{

class EllipticCoef
{
public:
    EllipticCoef(Pt::uint32_t order, double sampleFrequency, double lowerFrequnecy, double upperFrequency, double delta, double stopRipple);
    bool calcCoef(std::vector<double>& a, std::vector<double>& b);

private:
    double analog_fp;
    double analog_fs;
    double analog_omega;
    double pb_1, pb_2, sb_1, sb_2, c_bp, c_bs, a;
    double calc_ratio, q, q0;
    double al, sigma0, w, freq_ratio, a0, a1, b0, dr, omegai, vi, amu;
    double b2, b1, h0, prod, epsilon, a2, fs;
    
    bool lp_flag;
    bool hp_flag;
    bool bp_flag;
    bool bs_flag;
        
    double _sampleFrequency; 
    double _lowerFrequnecy;
    double _upperFrequency;
    double _delta;
    double _stopRipple;
    double _passRipple;
    Pt::uint32_t _order;
    double _resolution;
    double _centerFrequency;

    bool ideal_filter_elliptic();
};

}}

#endif
