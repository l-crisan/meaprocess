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
#include "ButterworthCoef.h"
#include <cmath>
#include <algorithm>
#include <Pt/Math.h>

namespace mps{
namespace filter{

ButterworthCoef::ButterworthCoef(Pt::uint32_t order, double sampleFrequency,double lowerFrequnecy, double upperFrequency, double delta, double stopRipple)
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
{
    den_curr_array.resize(5);
    den_prev_array.resize(5);
    den_next_array.resize(9);

    num_curr_array.resize(5);
    num_prev_array.resize(5);
    num_next_array.resize(9);
}

bool ButterworthCoef::calcCoef(std::vector<double>& a, std::vector<double>& b)
{
    if(_sampleFrequency <= 0.0) 
        return false;

    if(_lowerFrequnecy < 0.0) 
        return false;

    if(_upperFrequency< _lowerFrequnecy) 
        return false;

    if( _delta <= 0.0) 
        return false;

    if(_stopRipple < 0.0) 
        return false;

    if( _order < 0) 
        return false;

    a.resize(_order);
    b.resize(_order);

    pb_1 = _lowerFrequnecy;
    pb_2 = _upperFrequency;
    sb_1 = _lowerFrequnecy - _delta;
    sb_2 = _upperFrequency + _delta;

    // the pass band ripple is constant and fixed to 1.0
    _passRipple = 1.0;

    if( pb_1 == 0.0  || (sb_1 < 0.0)) 
    {//low pass filter 
        lp_flag = true;
        hp_flag = false;
        bp_flag = false;
    }
    else if( (pb_1 != 0.0) && (pb_2 >= (_sampleFrequency/2))) 
    {//high pass filter
        hp_flag = true;
        lp_flag = false;
        bp_flag = false;
    }
    else 
    {// band pass filter
        bp_flag = true;
        lp_flag = false;
        hp_flag = false;
    }
    
    if(lp_flag) 
    {
        analog_fp = std::tan(Pt::pi<double>() * pb_2 / _sampleFrequency);

        analog_fs = std::tan(Pt::pi<double>() * sb_2 / _sampleFrequency);

        double epsilon_pass = std::sqrt(std::pow(10,(_passRipple/10))-1);
        double epsilon_stop = std::sqrt(std::pow(10,(_stopRipple/10))-1); 

        // find filter order
        if(_order == 0) 
            _order = (int) std::ceil(std::log(epsilon_stop/epsilon_pass) / std::log(analog_fs/analog_fp));

        // find analog frequency in radians
        double temp = std::pow(epsilon_pass,(1/(double)_order));
        analog_omega = analog_fp / temp;
    }


    // high pass filter case
    if(hp_flag) 
    {
        analog_fp =  1 / std::tan(Pt::pi<double>() * pb_1 / _sampleFrequency);

        analog_fs =  1 / std::tan(Pt::pi<double>() * sb_1 / _sampleFrequency);

        double epsilon_pass = std::sqrt(std::pow(10,(_passRipple/10))-1);
        double epsilon_stop = std::sqrt(std::pow(10,(_stopRipple/10))-1); 

        if(_order == 0) 
            _order = (int) std::ceil(std::log(epsilon_stop/epsilon_pass) / std::log(analog_fs/analog_fp));

        // find analog frequency in radians
        analog_omega = analog_fp / std::pow(epsilon_pass,(1/(double)_order));
    }
    
    // band pass filter
    if(bp_flag) 
    {
        double omega_pa = 2 * Pt::pi<double>() * pb_1 / _sampleFrequency;
        double omega_pb = 2 * Pt::pi<double>() * pb_2 / _sampleFrequency;

        c_bp = std::sin(omega_pa + omega_pb) / (std::sin(omega_pa) + std::sin(omega_pb));
        analog_fp = std::abs((c_bp - std::cos(omega_pb)) /std::sin(omega_pb));
        double omega_sa = 2 * Pt::pi<double>() * sb_1 / _sampleFrequency;
        double omega_sb = 2 * Pt::pi<double>() * sb_2 / _sampleFrequency;

        double analog_fs1 = (c_bp - std::cos(omega_sa)) / std::sin(omega_sa);
        double analog_fs2 = (c_bp - std::cos(omega_sb)) / std::sin(omega_sb);

        analog_fs = std::min(std::abs(analog_fs1),std::abs(analog_fs2));

        double epsilon_pass = std::sqrt(std::pow(10,(_passRipple/10))-1);
        double epsilon_stop = std::sqrt(std::pow(10,(_stopRipple/10))-1); 

        if(_order == 0)
            _order = (int) std::ceil(std::log(epsilon_stop/epsilon_pass) / std::log(analog_fs/analog_fp));

        analog_omega = analog_fp / std::pow(epsilon_pass,(1/(double)_order));    
    }

    // to find the location of the poles of the analog filter

    std::vector<double> poles(2*_order);
    int i=0;
    int k=0;
    for( i = 0; i < static_cast<int>(_order)/2; i++, k = k+2 ) 
    {
        poles[k] = std::cos((Pt::pi<double>() /2) + ((2 * i + 1) * Pt::pi<double>() / (2 *_order)));
        poles[k+1] = std::sin((Pt::pi<double>() /2) + ((2 * i + 1) * Pt::pi<double>() / (2 *_order)));
    }

    // compute coefficients
    double a0 =0.0;
    double si=0.0;

    // if the filter order is odd then there is a 
    // first order filter when considering cascade form
    if(lp_flag) 
    {
        if(_order == 1) 
        {
            a0 = analog_omega + 1.0;
            a[0] = 1.0;
            //data.real_ar_lag[0] = 0;
            a[1] = (analog_omega - 1.0) / a0;
            //data.real_ar_lag[1] = -1;
            b[0] = analog_omega / a0;
            //data.real_ma_lag[0] = 0;
            b[1] = analog_omega / a0;
            //data.real_ma_lag[0] = -1;
        }    
        else if(_order > 1) 
        {
            int taps = 0;
            taps = (int)_order;
            // calculate the coefficients for the first 2nd order
            //
            si = - 2 * poles[0];
            a0 = 1 + si * analog_omega + analog_omega * analog_omega;

            den_prev_array[0] = 1.0;
            den_prev_array[1] = (2 * analog_omega * analog_omega - 2.0) /a0;
            den_prev_array[2] = (1 - si * analog_omega +
                         analog_omega * analog_omega) / a0;

            num_prev_array[0] = analog_omega * analog_omega / a0;
            num_prev_array[1] = 2 * analog_omega * analog_omega / a0;
            num_prev_array[2] = analog_omega * analog_omega / a0;

            taps = taps - 2;
            for(i=1;i<((static_cast<int>(_order)+1)/2);i++) 
            {
                if(taps != 0 && taps != 1) 
                {
                    den_curr_array.clear();
                    den_curr_array.resize(3,0);

                    num_curr_array.clear();
                    num_curr_array.resize(3,0);

                    den_next_array.clear();
                    den_next_array.resize(den_prev_array.size() + den_curr_array.size() - 1,0);

                    num_next_array.clear();
                    num_next_array.resize(num_prev_array.size() + num_curr_array.size() - 1,0);
                    si = - 2 * poles[2*i];
                    a0 = 1 + si * analog_omega + analog_omega * analog_omega;

                    den_curr_array[0] = 1.0;
                    den_curr_array[1] = (2 * analog_omega * analog_omega - 2.0) /a0;
                    den_curr_array[2] = (1 - si * analog_omega +
                                 analog_omega * analog_omega) / a0;
                    
                    mult(den_prev_array, den_curr_array, den_next_array);
                    den_prev_array = den_next_array;

                    num_curr_array[0] = analog_omega * analog_omega / a0;
                    num_curr_array[1] = 2 * analog_omega * analog_omega / a0;
                    num_curr_array[2] = analog_omega * analog_omega / a0;
                    
                    mult(num_prev_array, num_curr_array, num_next_array);
                    num_prev_array = num_next_array;

                    taps = taps - 2;
                }
                else 
                {
                    den_curr_array.clear();
                    den_curr_array.resize(2,0);

                    num_curr_array.clear();
                    num_curr_array.resize(2,0);

                    den_next_array.clear();
                    den_next_array.resize(den_prev_array.size() + den_curr_array.size() - 1,0);

                    num_next_array.clear();
                    num_next_array.resize(num_prev_array.size()+num_curr_array.size() - 1,0);

                    a0 = analog_omega + 1.0;
                    den_curr_array[0] = 1.0;
                    den_curr_array[1] = (analog_omega - 1.0) / a0;
                    mult(den_prev_array, den_curr_array, den_next_array);
                    den_prev_array = den_next_array;

                    num_curr_array[0] = analog_omega / a0;
                    num_curr_array[1] = analog_omega / a0;
                    mult(num_prev_array, num_curr_array, num_next_array);

                    den_prev_array = den_next_array;
                    num_prev_array = num_next_array;
                }
            }
        }
       _order =_order + 1;
    }
    
    if(hp_flag) 
    {
        if((_order == 1)) 
        {
            a0 = analog_omega + 1.0;
            a[0] = 1.0;
            //data.real_ar_lag[0] = 0;
            a[1] = - (analog_omega - 1.0) / a0;
            //data.real_ar_lag[1] = -1;
            b[0] = analog_omega / a0;
            //data.real_ma_lag[0] = 0;
            b[1] =  - analog_omega / a0;
            //data.real_ma_lag[0] = -1;
        }
        else if(_order > 1) 
        {
            int taps = 0;
            taps = (int)_order;
            // calculate the coefficients for the first 2nd order
            //
            si = - 2 * poles[0];
            a0 = 1 + si * analog_omega + analog_omega * analog_omega;
            
            den_prev_array[0] = 1.0;
            den_prev_array[1] = -(2 * analog_omega * analog_omega - 2.0) /a0;
            den_prev_array[2] = (1 - si * analog_omega +
                         analog_omega * analog_omega) / a0;
            
            num_prev_array[0] = analog_omega * analog_omega / a0;
            num_prev_array[1] =  - 2 * analog_omega * analog_omega / a0;
            num_prev_array[2] = analog_omega * analog_omega / a0;
            
            taps = taps - 2;
            for(i=1;i<((static_cast<int>(_order)+1)/2);i++) 
            {
                if(taps != 0 && taps != 1) 
                {
                    den_curr_array.clear();
                    den_curr_array.resize(3,0);

                    num_curr_array.clear();
                    num_curr_array.resize(3,0);

                    den_next_array.clear();
                    den_next_array.resize(den_prev_array.size()+den_curr_array.size() - 1,0);

                    num_next_array.clear();
                    num_next_array.resize(num_prev_array.size()+num_curr_array.size() - 1,0);

                    si = - 2 * poles[2*i];
                    a0 = 1 + si * analog_omega + analog_omega * analog_omega;

                    den_curr_array[0] = 1.0;
                    den_curr_array[1] = -(2 * analog_omega * analog_omega - 2.0) /a0;
                    den_curr_array[2] = (1 - si * analog_omega +
                                 analog_omega * analog_omega) / a0;

                    mult(den_prev_array, den_curr_array, den_next_array);
                    den_prev_array = den_next_array;

                    num_curr_array[0] = analog_omega * analog_omega / a0;
                    num_curr_array[1] = - 2 * analog_omega * analog_omega / a0;
                    num_curr_array[2] = analog_omega * analog_omega / a0;

                    mult(num_prev_array, num_curr_array, num_next_array);
                    num_prev_array = num_next_array;

                    taps = taps - 2;
                }
                else
                {
                    den_curr_array.clear();
                    den_curr_array.resize(2,0);

                    num_curr_array.clear();
                    num_curr_array.resize(2,0);

                    den_next_array.clear();
                    den_next_array.resize(den_prev_array.size()+den_curr_array.size() - 1,0);

                    num_next_array.clear();
                    num_next_array.resize(num_prev_array.size()+num_curr_array.size() - 1,0);

                    a0 = analog_omega + 1.0;
                    den_curr_array[0] = 1.0;
                    den_curr_array[1] = -(analog_omega - 1.0) / a0;
                    mult(den_prev_array, den_curr_array, den_next_array);
                    den_prev_array = den_next_array;

                    num_curr_array[0] = analog_omega / a0;
                    num_curr_array[1] = - analog_omega / a0;
                    mult(num_prev_array, num_curr_array, num_next_array);

                    den_prev_array = den_next_array;
                    num_prev_array = num_next_array;
                }
            }
        }
       _order =_order + 1; 
    }

    if(bp_flag) 
    {
        if(_order == 1) 
        {
            a0 = analog_omega + 1.0;
            a[0] = 1.0;
            //data.real_ar_lag[0] = 0;
            a[1] = - (2 * c_bp) / a0;
            //data.real_ar_lag[1] = -1;
            a[2] =  (1.0 - analog_omega) / a0;
            //data.real_ar_lag[2] = -2;
            b[0] = analog_omega / a0;
            //data.real_ma_lag[0] = 0;
            b[1] =   0.0 ;
            //data.real_ma_lag[1] = -1;
            b[2] =   analog_omega / a0;
            //data.real_ma_lag[2] = -2;
        }    
        else if(_order > 1) 
        {
            int taps = 0;
            taps = (int)_order;
            // calculate the coefficients for the first 2nd order
            //
            si = - 2 * poles[0];
            a0 = 1 + si * analog_omega + analog_omega * analog_omega;

            den_prev_array[0] = 1.0;
            den_prev_array[1] = 2 * c_bp * (analog_omega *  (-si) - 2.0) / a0;
            den_prev_array[2] = 2 * ( 2 * c_bp * c_bp + 1.0 - 
                          analog_omega * analog_omega) / a0;
            den_prev_array[3] = - 2 * c_bp * (analog_omega *(-si) + 2.0) / a0;
            den_prev_array[4] = (1 - si * analog_omega +
                         analog_omega * analog_omega) / a0;

            num_prev_array[0] = analog_omega * analog_omega / a0;
            num_prev_array[1] = 0.0;
            num_prev_array[2] =  - 2 * analog_omega * analog_omega / a0;
            num_prev_array[3] = 0.0;
            num_prev_array[4] = analog_omega * analog_omega / a0;

            taps = taps - 2;
            for(i=1;i<((static_cast<int>(_order)+1)/2);i++) 
            {
                if(taps != 0 && taps != 1) 
                {
                    den_curr_array.clear();
                    den_curr_array.resize(5,0);

                    num_curr_array.clear();
                    num_curr_array.resize(5,0);

                    den_next_array.clear();
                    den_next_array.resize(den_prev_array.size()+den_curr_array.size() - 1,0);

                    num_next_array.clear();
                    num_next_array.resize(num_prev_array.size()+num_curr_array.size() - 1,0);
                    si = - 2 * poles[2*i];
                    a0 = 1 + si * analog_omega + analog_omega * analog_omega;

                    den_curr_array[0] = 1.0;
                    den_curr_array[1] = 2 * c_bp * (analog_omega * (-si) - 2.0) / a0;
                    den_curr_array[2] = 2 * ( 2 * c_bp * c_bp + 1.0 - 
                                  analog_omega * analog_omega) / a0;
                    den_curr_array[3] = - 2 * c_bp * (analog_omega * (-si) + 2.0) / a0;
                    den_curr_array[4] = (1 - si * analog_omega +
                                 analog_omega * analog_omega) / a0;

                    mult(den_prev_array, den_curr_array, den_next_array);
                    den_prev_array = den_next_array;

                    num_curr_array[0] = analog_omega * analog_omega / a0;
                    num_curr_array[1] = 0.0;
                    num_curr_array[2] =  - 2 * analog_omega * analog_omega / a0;
                    num_curr_array[3] = 0.0;
                    num_curr_array[4] = analog_omega * analog_omega / a0;

                    mult(num_prev_array, num_curr_array, num_next_array);
                    num_prev_array = num_next_array;

                    taps = taps - 2;
                }
                else 
                {
                    den_curr_array.clear();
                    den_curr_array.resize(3,0);

                    num_curr_array.clear();
                    num_curr_array.resize(3,0);

                    den_next_array.clear();
                    den_next_array.resize(den_prev_array.size()+den_curr_array.size() - 1,0);

                    num_next_array.clear();
                    num_next_array.resize(num_prev_array.size()+num_curr_array.size() - 1,0);

                    a0 = analog_omega + 1.0;
                    den_curr_array[0] = 1.0;
                    den_curr_array[1] = -( 2 * c_bp) / a0;
                    den_curr_array[2] = ( 1.0 - analog_omega) / a0;
                    mult(den_prev_array, den_curr_array, den_next_array);
                    den_prev_array = den_next_array;

                    num_curr_array[0] = analog_omega / a0;
                    num_curr_array[1] = 0.0;
                    num_curr_array[2] = - analog_omega / a0;
                    mult(num_prev_array, num_curr_array, num_next_array);

                    den_prev_array = den_next_array;
                    num_prev_array = num_next_array;
                }
            }
        }
       _order = 2 *_order + 1;
    }

    a.resize(_order);
    b.resize(_order);

    for( i=0; i<static_cast<int>(_order); i++) 
    {
        a[i] = den_prev_array[i];
        //data.real_ar_lag[i] = - i;
        b[i] = num_prev_array[i];
        //data.real_ma_lag[i] = - i;
    }

    // Compute the gain of the filter
    double gain = 1.0;
    return true;
}

void ButterworthCoef::mult(std::vector<double>& ip, std::vector<double>& op, std::vector<double>& next)
{
    int index;
    for( int j=0; j<static_cast<int>(ip.size()); j++) 
    {
        index = j;
        for( int k=0; k < static_cast<int>(op.size()); k++) 
        {
            next[index] = next[index] + ip[j] * op[k];
            index++;
        }
    }
} 

}}
