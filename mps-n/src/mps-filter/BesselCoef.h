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
#ifndef MPS_IIRFILTER_BESSELCOEF_H
#define MPS_IIRFILTER_BESSELCOEF_H

#include <vector>
#include <cstddef>

namespace mps{
namespace filter{ 

// Designs a Bessel filter
// This code assumes a constant filter order of 25 and
// given the poles for that order (taken from Matlab)
// will find the filter response
//=============================================================================
// Filter: A Digital Filter Class
//
//  This is an include file for a class for linear filtering of a signal.
//
//  A filter object represents a digital ARMA filter of the form:
//  
//  H(z) =    sum    ma_coef[i] * z ** ma_lag[i]
//             
//             1 - 2*cos(wo)z^-1 + z^2
//  H(z) = b * -------------------------
//             1 - 2*r*cos(wo)z^1 + r^2z^-2
//=============================================================================
#include <Pt/Types.h>

class BesselCoef
{
public:
    BesselCoef(Pt::uint32_t order, double sampleFrequency,double lowerFrequnecy, double upperFrequency, double delta, double stopRipple);
    bool calcCoef(std::vector<double>& a, std::vector<double>& b);
  
private:
    static void mult(std::vector<double>& ip, std::vector<double>& op, std::vector<double>& next);

private:
    std::vector<double> den_curr_array;
    std::vector<double> den_prev_array;
    std::vector<double> den_next_array;
    
    std::vector<double> num_curr_array;
    std::vector<double> num_prev_array;
    std::vector<double> num_next_array;
    
    double analog_fp;
    double analog_fs;
    double analog_omega;
    double pb_1, pb_2, sb_1, sb_2, c_bp, c_bs, a;
    double epsilon_pass, epsilon_stop;
    
    // set flag to determine type of filter
    bool lp_flag;
    bool hp_flag;
    bool bp_flag;
    bool bs_flag;
    
    // enter value of poles and absolute values for variuos filter order
    static double poles[25][13];
    static double abs_poles[25][13];
   
    double _sampleFrequency; 
    double _lowerFrequnecy;
    double _upperFrequency;
    double _delta;
    double _stopRipple;
    double _passRipple;
    Pt::uint32_t _order;
};

}}

#endif
