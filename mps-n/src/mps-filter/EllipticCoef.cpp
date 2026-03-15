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
#include "EllipticCoef.h"
#include <cmath>
#include <algorithm>

namespace mps{
namespace filter{

EllipticCoef::EllipticCoef(Pt::uint32_t order, double sampleFrequency,double lowerFrequnecy, double upperFrequency, double delta, double stopRipple) 
: analog_fp(0.0)
, analog_fs(0.0)
, analog_omega(0.0)
, lp_flag(false)
, hp_flag(false)
, bp_flag(false)
, bs_flag(false) 
, _passRipple(0)
, _order(order)
, _sampleFrequency(sampleFrequency)
, _lowerFrequnecy(lowerFrequnecy)
, _upperFrequency(upperFrequency)
, _delta(delta)
, _stopRipple(stopRipple)
, _resolution(2000)
{
} 

bool EllipticCoef::ideal_filter_elliptic() 
{
    double hz_per_point = (_sampleFrequency / 2) / _resolution;
    int lower_point = (int)( _centerFrequency - (2*hz_per_point) / hz_per_point );
    int upper_point = (int)( _centerFrequency + (2*hz_per_point) / hz_per_point );
    int upper_bound = (int)(_resolution);
    
    // done for testing purposes dumby data
    for (int i=0; i<upper_bound; i++) 
    {
        if (i<lower_point) 
        {
            //data.ideal_filter[i] = 0;
        }
        else if (i>upper_point)
        {
            //data.ideal_filter[i] = 0;
        }
        else 
        { 
        //	data.ideal_filter[i] = -30; 
        }
    }

    return true;
}

bool EllipticCoef::calcCoef(std::vector<double>& aa, std::vector<double>& bb)
{
    // Bug fixes to keep applet from hanging due to neg and pos 
    // infiniting plotting problems.  This is a hard coded fix
    // to keep the values from approaching the infinities
    // POOR CODING :(
    //
    //if (data.critical_frequency < 0.0001) {
    //data.critical_frequency = 0.0001;
    //}

    //if (data.center_frequency > ((data.sample_frequency/2)-0.0001)) {
    //data.center_frequency = (data.sample_frequency/2)-0.0001;
    //}

    // Set the filter order so that it plots properly
    //
    //data.f_order = 3;

    // calculate the filter order from the attenuation factor
    // critical freq and stopband freq
    //

    // To find the type of filter(LP, HP, BP)
    //

    int i = 0;

    if(_sampleFrequency <= 0.0) 
        return false;

    if(_delta <= 0.0) 
        return false;

    if(_stopRipple <= 0.0) 
        return false;

    pb_1 = _lowerFrequnecy;
    pb_2 = _upperFrequency;
    sb_1 = _lowerFrequnecy - _delta;
    sb_2 = _upperFrequency + _delta;

    if( pb_1 == 0.0  || (pb_1 < 0.0)) 
    {
        lp_flag = true;
        hp_flag = false;
        bp_flag = false;
    }
    else if( (pb_1 != 0.0) && (pb_2 >= (_sampleFrequency/2))) 
    {
        hp_flag = true;
        lp_flag = false;
        bp_flag = false;
    }
    else 
    {
        bp_flag = true;
        lp_flag = false;
        hp_flag = false;
    }

    _passRipple = 1.0;

    q= 0;
    
    if (_order == 0)
    {
        if(lp_flag) 
        {
            analog_fp = std::tan(Pt::pi<double>() * pb_2 / _sampleFrequency);

            analog_fs = std::tan(Pt::pi<double>() * sb_2 / _sampleFrequency);

            epsilon = 0.00001;

            // the code to find the filter order given the
            // specifications is from the software given by P.Kabal
            freq_ratio = analog_fp / analog_fs;
            calc_ratio = std::sqrt(1.0 - (freq_ratio * freq_ratio));

            q0 = 0.5 * (1.0 - std::sqrt(calc_ratio)) / ( 1.0 + std::sqrt(calc_ratio));
            q = q0 + 2.0 * std::pow(q0,5.0) + 15.0 * std::pow(q0,9.0) + 150.0 * std::pow(q0,13);
            double d = (std::pow(10.0,(_stopRipple/10.0)) - 1.0) /(std::pow(10.0,(_passRipple/10.0)) - 1.0);
            _order = (int) ((std::log(16.0 * d) / std::log(1.0 / q)) + ( 1.0 - epsilon));
        }

        if(hp_flag) 
        {
            analog_fp = 1.0 / std::tan(Pt::pi<double>() * pb_1 / _sampleFrequency);

            analog_fs = 1.0 / std::tan(Pt::pi<double>() * sb_1 / _sampleFrequency);

            // the code to find the filter order given the 
            //specifications  is from the software given by P.Kabal
            freq_ratio = analog_fp / analog_fs;
            calc_ratio = std::sqrt(1.0 - (freq_ratio * freq_ratio));
            q0 = 0.5 * (1.0 - std::sqrt(calc_ratio)) / ( 1.0 +  std::sqrt(calc_ratio));
            q = q0 + 2.0 * std::pow(q0,5.0) + 15.0 * std::pow(q0,9.0) + 150.0 * std::pow(q0,13);
            double d = (std::pow(10.0,(_stopRipple/10.0)) - 1.0) /(std::pow(10.0,(_passRipple/10.0)) - 1.0);
            _order = (int) ((std::log(16.0 * d) / std::log(1.0 / q)) + ( 1.0 - epsilon));
        }
        
        if(bp_flag) 
        {
            double omega_pa = 2 * Pt::pi<double>() * pb_1/_sampleFrequency;
            double omega_pb = 2 * Pt::pi<double>() * pb_2/_sampleFrequency;
            c_bp = std::sin(omega_pa + omega_pb) / (std::sin(omega_pa)+ std::sin(omega_pb));
            analog_fp = std::abs((c_bp - std::cos(omega_pb)) /std::sin(omega_pb));
            double omega_sa = 2 * Pt::pi<double>() * sb_1/_sampleFrequency;
            double omega_sb = 2 * Pt::pi<double>() * sb_2/_sampleFrequency;
            double analog_fs1 = (c_bp - std::cos(omega_sa)) / std::sin(omega_sa);
            double analog_fs2 = (c_bp - std::cos(omega_sb)) / std::sin(omega_sb);
            analog_fs = std::min(std::abs(analog_fs1),std::abs(analog_fs2));
            // the code to find the filter order given the
            //specifications is from the software given by P.Kabal
            freq_ratio = analog_fp / analog_fs;
            calc_ratio = std::sqrt(1.0 - (freq_ratio * freq_ratio));

            q0 = 0.5 * (1.0 - std::sqrt(calc_ratio)) / ( 1.0 +  std::sqrt(calc_ratio));

            q = q0 + 2.0 * std::pow(q0,5.0) + 15.0 * std::pow(q0,9.0) + 150.0 * std::pow(q0,13);
            
            double d = (std::pow(10.0,(_stopRipple/10.0)) - 1.0) /(std::pow(10.0,(_passRipple/10.0)) - 1.0);
            _order = (int) ((std::log(16.0 * d) / std::log(1.0 / q)) + ( 1.0 - epsilon));
        }
    }

    if (_order <= 0 )
        return false;

    if (_stopRipple > 1)
        _stopRipple = -_stopRipple;


    double pass_ripple = 0.0;
    Elliptic ellip((int) _order, pass_ripple, _sampleFrequency, pb_1, pb_2, _stopRipple);
    
    if(bp_flag)
        _order *= 2;

    _order++;

    aa.resize(_order);
    bb.resize(_order);

    for(i=0; i < static_cast<int>(_order); i++) 
    {
        aa[i] = ellip.getcoefa(i);
        //data.real_ar_lag[i] = - i;
        bb[i] = ellip.getcoefb(i);
        //data.real_ma_lag[i] = - i;
    }
    return true;
}
    
}}
