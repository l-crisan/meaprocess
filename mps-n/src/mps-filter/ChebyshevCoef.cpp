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
#include "ChebyshevCoef.h"
#include <cmath>
#include <algorithm>
#include <Pt/Math.h>

namespace mps{
namespace filter{

ChebyshevCoef::ChebyshevCoef(Pt::uint32_t order, double sampleFrequency,double lowerFrequency, double upperFrequency, double delta, double stopRipple)
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
, _lowerFrequency(lowerFrequency)
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

bool ChebyshevCoef::calcCoef(std::vector<double>& aa, std::vector<double>& bb)
{
    if(_sampleFrequency <= 0.0) 
        return false;

    if(_lowerFrequency < 0.0) 
        return false;

    if(_upperFrequency< _lowerFrequency) 
        return false;

    if( _delta <= 0.0)
        return false;

    if(_stopRipple < 0.0)
        return false;

    if( _order < 0)
        return false;

    aa.resize(_order);
    bb.resize(_order);

    // To find the type of filter(LP, HP, BP)
    //
    pb_1 = _lowerFrequency;
    pb_2 = _upperFrequency;
    sb_1 = _lowerFrequency - _delta;
    sb_2 = _upperFrequency + _delta;
    
    _passRipple = 1.0;
    
    if( pb_1 == 0.0  || (sb_1 < 0.0)) 
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
    
    if(lp_flag) 
    {
        analog_fp = std::tan(Pt::pi<double>() * pb_2 / _sampleFrequency);

        analog_fs = std::tan(Pt::pi<double>() * sb_2 / _sampleFrequency);

        epsilon_pass = std::sqrt(std::pow(10,(_passRipple/10))-1);
        epsilon_stop = std::sqrt(std::pow(10,(_stopRipple/10))-1); 
        double ripple_ratio = epsilon_stop / epsilon_pass;
        double freq_ratio = analog_fs / analog_fp;

        if(_order == 0) 
        {
            _order = (int) std::ceil((std::log(ripple_ratio + 
                             std::sqrt(ripple_ratio * ripple_ratio - 1.0))/
                             (std::log(freq_ratio + std::sqrt(freq_ratio * freq_ratio - 1.0)))));
        }

        double inv_hyp_cos = (std::log(1.0 / epsilon_pass + 
                       std::sqrt((1.0 / epsilon_pass) *
                             (1.0 / epsilon_pass) - 1.0)))/ ((double) _order);

        analog_omega = analog_fp * (std::exp(inv_hyp_cos) + std::exp(-inv_hyp_cos)) / 2.0;
    }

    if(hp_flag) 
    {
        analog_fp = 1.0 / std::tan(Pt::pi<double>() * pb_1 / _sampleFrequency);

        analog_fs = 1.0 / std::tan(Pt::pi<double>() * sb_1 / _sampleFrequency);

        epsilon_pass = std::sqrt(std::pow(10,(_passRipple/10))-1);
        epsilon_stop = std::sqrt(std::pow(10,(_stopRipple/10))-1); 
        double ripple_ratio = epsilon_stop / epsilon_pass;
        double freq_ratio = analog_fs / analog_fp;

        if(_order == 0) 
        {
            _order = (int) std::ceil((std::log(ripple_ratio + 
                             std::sqrt(ripple_ratio * ripple_ratio - 1.0))/
                            (std::log(freq_ratio + std::sqrt(freq_ratio * freq_ratio - 1.0)))));
        }

        double inv_hyp_cos = (std::log(1.0 / epsilon_pass + std::sqrt((1.0 / epsilon_pass) *(1.0 / epsilon_pass) - 1.0)))/ ((double) _order);

        analog_omega = analog_fp * (std::exp(inv_hyp_cos) + std::exp(-inv_hyp_cos)) / 2.0;	    
    }

    if(bp_flag) 
    {
        double omega_pa = 2 * Pt::pi<double>() * pb_1 / _sampleFrequency;
        double omega_pb = 2 * Pt::pi<double>() * pb_2 / _sampleFrequency;

        c_bp = std::sin(omega_pa + omega_pb) / (std::sin(omega_pa)+ std::sin(omega_pb));
        analog_fp = std::abs((c_bp - std::cos(omega_pb)) /std::sin(omega_pb));
        double omega_sa = 2 * Pt::pi<double>() * sb_1 / _sampleFrequency;
        double omega_sb = 2 * Pt::pi<double>() * sb_2 / _sampleFrequency;

        double analog_fs1 = (c_bp - std::cos(omega_sa)) / std::sin(omega_sa);
        double analog_fs2 = (c_bp - std::cos(omega_sb)) / std::sin(omega_sb);

        analog_fs = std::min(std::abs(analog_fs1),std::abs(analog_fs2));

        epsilon_pass = std::sqrt(std::pow(10,(_passRipple/10))-1);
        epsilon_stop = std::sqrt(std::pow(10,(_stopRipple/10))-1);
        double ripple_ratio = epsilon_stop / epsilon_pass;
        double freq_ratio = analog_fs / analog_fp;

        if(_order == 0) 
        {
            _order = (int) std::ceil((std::log(ripple_ratio + 
                             std::sqrt(ripple_ratio * ripple_ratio - 1.0))/(std::log(freq_ratio + std::sqrt(freq_ratio * freq_ratio - 1.0)))));
        }

        double inv_hyp_cos = (std::log(1.0 / epsilon_pass + std::sqrt((1.0 / epsilon_pass) *
                             (1.0 / epsilon_pass) - 1.0)))/ ((double) _order);
        analog_omega = analog_fp * (std::exp(inv_hyp_cos) + 
                    std::exp(-inv_hyp_cos)) / 2.0;
        
    }
    // to find the location of the poles of the analog filter
    //
    double realpole_butterworth = 0.0;
    double imagpole_butterworth = 0.0;
    std::vector<double> poles(2*_order);
    std::vector<double> abs_poles(_order);

    double inv_epsilon_pass = 1.0 / epsilon_pass;
    a = (std::log((inv_epsilon_pass) + std::sqrt((inv_epsilon_pass *inv_epsilon_pass) + 1.0))) / ((double) _order);

    int i=0;
    int k=0;
    double del = std::sqrt(std::pow(10,(0.1 * _passRipple)) - 1.0);

    // define mu = asinh(1/del) / data.f_order
    //
    del = 1.0 / del;
    double mu = std::log(del + std::sqrt(del * del + 1.0))/ (double)_order;

    for(i=0; i < ((static_cast<int>(_order)+1)/2); i++,k=k+2) 
    {
        realpole_butterworth = std::cos((Pt::pi<double>() /2) + ((2 * i + 1) *
                               Pt::pi<double>() / ( 2 * _order)));

        imagpole_butterworth = std::sin((Pt::pi<double>() /2) + ((2 * i + 1) *
                               Pt::pi<double>() / (2 * _order)));

        poles[k] = (std::exp(mu) - std::exp(-mu)) * realpole_butterworth / 2.0;
        poles[k+1] = (std::exp(mu) + std::exp(-mu)) * imagpole_butterworth / 2.0;

        abs_poles[i] = std::sqrt(poles[k] * poles[k] +
          poles[k+1] * poles[k+1]);
    }
    
    // compute coefficients
    //
    std::vector<double> omega(_order);
    double a0 =0.0;
    std::vector<double> si(_order);
    // if the filter order is odd then there is a 
    // first order filter when considering cascade form
    //
    if(lp_flag) 
    {
        if((_order == 1)) 
        {
            omega[0] = abs_poles[0] * analog_omega;
            a0 = omega[0] + 1.0;
            aa[0] = 1.0;
            //data.real_ar_lag[0] = 0;
            aa[1] = (omega[0] - 1.0) / a0;
            //data.real_ar_lag[1] = -1;
            bb[0] = omega[0] / a0;
            //data.real_ma_lag[0] = 0;
            bb[1] = omega[0] / a0;
            //data.real_ma_lag[0] = -1;
        }    
        else if(_order > 1) 
        {
            int taps = 0;
            taps = (int) _order;
            // calculate the coefficients for the first 2nd order
            //
            si[0] = - 2 * poles[0] / abs_poles[0];
            omega[0] = abs_poles[0] * analog_omega;
            a0 = 1 + si[0] * omega[0] + omega[0] * omega[0];

            den_prev_array[0] = 1.0;
            den_prev_array[1] = (2 * omega[0] * omega[0] - 2.0) /a0;
            den_prev_array[2] = (1 - si[0] * omega[0] +
                         omega[0] * omega[0]) / a0;

            num_prev_array[0] = omega[0] * omega[0] / a0;
            num_prev_array[1] = 2 * omega[0] * omega[0] / a0;
            num_prev_array[2] = omega[0] * omega[0] / a0;

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

                    omega[i] = abs_poles[i] * analog_omega;
                    si[i] = - 2 * poles[2*i] / abs_poles[i];
                    a0 = 1 + si[i] * omega[i] + omega[i] * omega[i];

                    den_curr_array[0] = 1.0;
                    den_curr_array[1] = (2 * omega[i] * omega[i] - 2.0) /a0;
                    den_curr_array[2] = (1 - si[i] * omega[i] +
                                 omega[i] * omega[i]) / a0;

                    mult(den_prev_array, den_curr_array, den_next_array);
                    den_prev_array = den_next_array;

                    num_curr_array[0] = omega[i] * omega[i] / a0;
                    num_curr_array[1] = 2 * omega[i] * omega[i] / a0;
                    num_curr_array[2] = omega[i] * omega[i] / a0;

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
                    omega[i] = abs_poles[i] * analog_omega;
                    a0 = omega[i] + 1.0;
                    den_curr_array[0] = 1.0;
                    den_curr_array[1] = (omega[i] - 1.0) / a0;
                    mult(den_prev_array, den_curr_array, den_next_array);
                    den_prev_array = den_next_array;
                    
                    num_curr_array[0] = omega[i] / a0;
                    num_curr_array[1] = omega[i] / a0;
                    mult(num_prev_array, num_curr_array, num_next_array);
                    
                    den_prev_array = den_next_array;
                    num_prev_array = num_next_array;
                }
            }
        }
        _order = _order + 1; 
    }
    
    if(hp_flag) 
    {
        if(_order == 1) 
        {
            omega[0] = abs_poles[0] * analog_omega;
            a0 = omega[0] + 1.0;
            aa[0] = 1.0;
            //data.real_ar_lag[0] = 0;
            aa[1] = - (omega[0] - 1.0) / a0;
            //data.real_ar_lag[1] = -1;

            bb[0] = omega[0] / a0;
            //data.real_ma_lag[0] = 0;
            bb[1] =  - omega[0] / a0;
            //data.real_ma_lag[1] = -1;
        }
        else if(_order > 1) 
        {
            int taps = 0;
            taps = (int) _order;
            // calculate the coefficients for the first 2nd order
            //
            si[0] = - 2 * poles[0] / abs_poles[0];
            omega[0] = abs_poles[0] * analog_omega;
            a0 = 1 + si[0] * omega[0] + omega[0] * omega[0];

            den_prev_array[0] = 1.0;
            den_prev_array[1] = - (2 * omega[0] * omega[0] - 2.0) /a0;
            den_prev_array[2] = (1 - si[0] * omega[0] + omega[0] * omega[0]) / a0;

            num_prev_array[0] = omega[0] * omega[0] / a0;
            num_prev_array[1] = - 2 * omega[0] * omega[0] / a0;
            num_prev_array[2] = omega[0] * omega[0] / a0;


            taps = taps - 2;
            for( i = 1; i < ((static_cast<int>(_order)+1)/2); i++) 
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

                    omega[i] = abs_poles[i] * analog_omega;
                    si[i] = - 2 * poles[2*i] / abs_poles[i];
                    a0 = 1 + si[i] * omega[i] + omega[i] * omega[i];

                    den_curr_array[0] = 1.0;
                    den_curr_array[1] = - (2 * omega[i] * omega[i] - 2.0) /a0;
                    den_curr_array[2] = (1 - si[i] * omega[i] +
                                 omega[i] * omega[i]) / a0;

                    mult(den_prev_array, den_curr_array, den_next_array);
                    den_prev_array = den_next_array;
                    
                    num_curr_array[0] = omega[i] * omega[i] / a0;
                    num_curr_array[1] = - 2 * omega[i] * omega[i] / a0;
                    num_curr_array[2] = omega[i] * omega[i] / a0;

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

                    omega[i] = abs_poles[i] * analog_omega;
                    a0 = omega[i] + 1.0;
                    den_curr_array[0] = 1.0;
                    den_curr_array[1] = - (omega[i] - 1.0) / a0;
                    mult(den_prev_array, den_curr_array, den_next_array);
                    den_prev_array = den_next_array;

                    num_curr_array[0] = omega[i] / a0;
                    num_curr_array[1] = - omega[i] / a0;
                    mult(num_prev_array, num_curr_array, num_next_array);

                    den_prev_array = den_next_array;
                    num_prev_array = num_next_array;
                }
            }
        }
        _order = _order + 1;
    }
    
    if(bp_flag) 
    {
        if(_order == 1) 
        {
            omega[0] = abs_poles[0] * analog_omega;
            a0 = omega[0] + 1.0;
            aa[0] = 1.0;
            //data.real_ar_lag[0] = 0;
            aa[1] = - (2 * c_bp) / a0;
            //data.real_ar_lag[1] = -1;
            aa[2] =  (1.0 - omega[i]) / a0;
            //data.real_ar_lag[2] = -2;

            bb[0] = omega[0] / a0;
            //data.real_ma_lag[0] = 0;
            bb[1] =  0.0;
            //data.real_ma_lag[1] = -1;
            bb[2] =  - omega[0] / a0;
            //data.real_ma_lag[2] = -2;
        }
        else if(_order > 1) 
        {
            int taps = 0;
            taps = (int) _order;
            // calculate the coefficients for the first 2nd order
            //
            si[0] = - 2 * poles[0] / abs_poles[0];
            omega[0] = abs_poles[0] * analog_omega;
            a0 = 1 + si[0] * omega[0] + omega[0] * omega[0];

            den_prev_array[0] = 1.0;
            den_prev_array[1] = 2 * c_bp * ((-si[0]) * omega[0] - 2.0) / a0;
            den_prev_array[2] = 2 * ( 2 * c_bp * c_bp + 1.0 - 
                          omega[0] * omega[0]) / a0;
            den_prev_array[3] = - 2 * c_bp * (omega[0] *(-si[0]) + 2.0) / a0;

            den_prev_array[4] = (1.0 - si[0] * omega[0] + omega[0] * omega[0])/
                a0;

            num_prev_array[0] = omega[0] * omega[0] / a0;
            num_prev_array[1] = 0.0;
            num_prev_array[2] = - 2 * omega[0] * omega[0] / a0;
            num_prev_array[3] = 0.0;
            num_prev_array[4] = omega[0] * omega[0] / a0;


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
                    omega[i] = abs_poles[i] * analog_omega;
                    si[i] = - 2 * poles[2*i] / abs_poles[i];
                    a0 = 1 + si[i] * omega[i] + omega[i] * omega[i];

                    den_curr_array[0] = 1.0;
                    den_curr_array[1] = 2 * c_bp * ((-si[i]) * omega[i] - 2.0) / a0;

                    den_curr_array[2] = 2 * ( 2 * c_bp * c_bp + 1.0 - omega[i] * omega[i]) / a0;
                    den_curr_array[3] = - 2 * c_bp * (omega[i] *(-si[i]) + 2.0) / a0;

                    den_curr_array[4] = (1.0 - si[i] * omega[i] + omega[i] * omega[i])/a0;

                    mult(den_prev_array, den_curr_array, den_next_array);
                    den_prev_array = den_next_array;
                    
                    num_curr_array[0] = omega[i] * omega[i] / a0;
                    num_curr_array[1] = 0.0;
                    num_curr_array[2] = - 2 * omega[i] * omega[i] / a0;
                    num_curr_array[3] = 0.0;
                    num_curr_array[4] = omega[i] * omega[i] / a0;
                    
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
                    den_next_array.resize(den_prev_array.size()+den_curr_array.size()- 1,0);

                    num_next_array.clear();
                    num_next_array.resize(num_prev_array.size()+num_curr_array.size() - 1,0);
                    omega[i] = abs_poles[i] * analog_omega;
                    a0 = omega[i] + 1.0;
                    den_curr_array[0] = 1.0;
                    den_curr_array[1] = -( 2 * c_bp) / a0;
                    den_curr_array[2] = ( 1.0 - omega[i]) / a0;
                    mult(den_prev_array, den_curr_array, den_next_array);
                    den_prev_array = den_next_array;

                    num_curr_array[0] = omega[i] / a0;
                    num_curr_array[1] = 0.0;
                    num_curr_array[2] = - omega[i] / a0;
                    mult(num_prev_array, num_curr_array, num_next_array);

                    den_prev_array = den_next_array;
                    num_prev_array = num_next_array;
                }
            }
            }
            _order = 2 * _order + 1;
    }
    
    aa.resize(_order);
    bb.resize(_order);

    for( i = 0; i < static_cast<int>(_order); i++) 
    {
        aa[i] = den_prev_array[i];
//	    data.real_ar_lag[i] = - i;
        bb[i] = num_prev_array[i];
//	    data.real_ma_lag[i] = - i;
    }

    // Compute the gain of the filter
    double gain = 1.0;
    
    return true;
}

void ChebyshevCoef::mult(std::vector<double>& ip, std::vector<double>& op, std::vector<double>& next)
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
