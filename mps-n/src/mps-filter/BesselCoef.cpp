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
#include "BesselCoef.h"
#include <cmath>
#include <algorithm>

namespace mps{
namespace filter{

static const double Pi = 3.14159265358979323846;
static const double PiDouble = 6.28318530717958647692;
static const double PiHalf = 1.57079632679489661923;
static const double PiQuart = 0.78539816339744830961;
static const double Pi180 = 0.01745329251994329576;
static const double PiSqr = 9.86960440108935861883449099987615114f;

double BesselCoef::poles[25][13] = { { -1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
               0.0, 0.0, 0.0, 0.0 },
             { -0.8660, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
               0.0, 0.0, 0.0, 0.0 },
             { -0.7456, -0.9416, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
               0.0, 0.0, 0.0, 0.0, 0.0 },
             { -0.6572, -0.9048, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
               0.0, 0.0, 0.0, 0.0, 0.0 }, 
             { -0.8516, -0.5906, -0.9264, 0.0, 0.0, 0.0, 0.0,
               0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
             { -0.9094, -0.7997, -0.5386, 0.0, 0.0, 0.0, 0.0,
               0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
             { -0.8800, -0.7527, -0.4967, -0.9195, 0.0, 0.0,
               0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
             { -0.9097, -0.8473, -0.7111, -0.4622, 0.0, 0.0,
               0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
             { -0.8911, -0.8148, -0.6744, -0.4331, -0.9155, 0.0,
               0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
             { -0.9091, -0.8688, -0.7838, -0.6418, -0.4083, 0.0,
               0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
             { -0.8924, -0.8453, -0.7547, -0.6127, -0.3868,
               -0.9129, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
             { -0.9084, -0.8803, -0.8217, -0.7277, -0.5866,
               -0.3680, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
             { -0.8991, -0.8625, -0.7987, -0.7026, -0.5632,
               -0.3513, -0.9111, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
             { -0.9078, -0.8870, -0.8441, -0.7767, -0.6794,
               -0.5419, -0.3364, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
             { -0.9007, -0.8731, -0.8257, -0.7556, -0.6579,
               -0.5225, -0.3230, -0.9097, 0.0, 0.0, 0.0, 0.0,
               0.0 },
             { -0.9072, -0.8912, -0.8584, -0.8075, -0.7356,
               -0.6380, -0.5048, -0.3109, 0.0, 0.0, 0.0, 0.0,
               0.0 }, 
             { -0.9016, -0.8801, -0.8433, -0.7898, -0.7167,
               -0.6194, -0.4885, -0.2998, -0.9087, 0.0, 0.0, 0.0,
               0.0 },
             { -0.9067, -0.8940, -0.8681, -0.8282, -0.7726,
               -0.6988, -0.6020, -0.4734, -0.2898, 0.0, 0.0, 0.0,
               0.0 },
             { -0.9022, -0.8849, -0.8556, -0.8132, -0.7561,
               -0.6818, -0.5859, -0.4595, -0.2805, -0.9079, 0.0,
               0.0, 0.0 }, 
             { -0.9063, -0.8959, -0.8750, -0.8428, -0.7984,
               -0.7403, -0.6658, -0.5707, -0.4466, -0.2719, 0.0,
               0.0, 0.0 }, 
             { -0.9025, -0.8884, -0.8644, -0.8299, -0.7840,
               -0.7251, -0.6506, -0.5565, -0.4345, -0.2640,
               -0.9072, 0.0, 0.0 },
             { -0.9059, -0.8973, -0.8800, -0.8535, -0.8172,
               -0.7700, -0.7105, -0.6362, -0.5431, -0.4233,
               -0.2566, 0.0, 0.0 },
             { -0.9028, -0.8909, -0.8709, -0.8424, -0.8046,
               -0.7565, -0.6966, -0.6226, -0.5305, -0.4127,
               -0.2498, -0.9067, 0.0 },
             { -0.9055, -0.8983, -0.8837, -0.8615, -0.8312,
               -0.7922, -0.7433, -0.6833, -0.6096, -0.5186,
               -0.4028, -0.2433, 0.0 },
             { -0.9029, -0.8929, -0.8759, -0.8519, -0.8210,
               -0.7800, -0.7307, -0.6705, -0.5973, -0.5073,
               -0.3935, -0.2373, -0.9062 }
    }; 


double BesselCoef::abs_poles[25][13] = { { 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
                   0.0, 0.0, 0.0, 0.0 },
                 { 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
                   0.0, 0.0, 0.0, 0.0 },
                 { 1.0305, 0.9416, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
                   0.0, 0.0, 0.0, 0.0, 0.0 },
                 { 1.0588, 0.9444, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
                   0.0, 0.0, 0.0, 0.0, 0.0 }, 
                 { 0.9598, 1.0825, 0.9264, 0.0, 0.0, 0.0, 0.0,
                   0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
                 { 0.9282, 0.9775, 1.1022, 0.0, 0.0, 0.0, 0.0,
                   0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
                 { 0.9369, 0.9948, 1.1188, 0.9195, 0.0, 0.0,
                   0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
                 { 0.9206, 0.9483, 1.0110, 1.1329, 0.0, 0.0,
                   0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
                 { 0.9262, 0.9605, 1.0259, 1.1451, 0.9155, 0.0,
                   0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
                 { 0.9162, 0.9341, 0.9726, 1.0394, 1.1558, 0.0,
                   0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
                 { 0.9202, 0.9429, 0.9843, 1.0517, 1.1652,
                   0.9129, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
                 { 0.9135, 0.9259, 0.9522, 0.9955, 1.0629,
                   1.1736, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
                 { 0.9164, 0.9326, 0.9614, 1.0060, 1.0732,
                   1.1810, 0.9111, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
                 { 0.9115, 0.9207, 0.9398, 0.9705, 1.0159,
                   1.0827, 1.1878, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
                 { 0.9137, 0.9259, 0.9472, 0.9793, 1.0252,
                   1.0914, 1.1939, 0.9097, 0.0, 0.0, 0.0, 0.0,
                   0.0 },
                 { 0.9101, 0.9171, 0.9317, 0.9547, 0.9878,
                   1.0340, 1.0994, 1.1995, 0.0, 0.0, 0.0, 0.0,
                   0.0 },
                 { 0.9118, 0.9213, 0.9378, 0.9620, 0.9959,
                   1.0422, 1.1069, 1.2047, 0.9087, 0.0, 0.0, 0.0,
                   0.0 },
                 { 0.9090,  0.9146, 0.9261, 0.9440, 0.9693,
                   1.0037, 1.0500, 1.1139, 1.2094, 0.0, 0.0,
                   0.0, 0.0 }, 
                 { 0.9104, 0.9180, 0.9311, 0.9502, 0.9763,
                   1.0111, 1.0573, 1.1203, 1.2138, 0.9079, 0.0,
                   0.0, 0.0 }, 
                 { 0.9081, 0.9127, 0.9219, 0.9363, 0.9564,
                   0.9832, 1.0182, 1.0642, 1.1264, 1.2178,
                   0.0, 0.0, 0.0 },
                 { 0.9093, 0.9155, 0.9262, 0.9416, 0.9625,
                   0.9898, 1.0250, 1.0708, 1.1321, 1.2216,
                   0.9072, 0.0, 0.0 },
                 { 0.9074, 0.9112, 0.9188, 0.9306, 0.9470,
                   0.9685, 0.9962, 1.0315, 1.0770, 1.1374,
                   1.2252, 0.0, 0.0 },
                 { 0.9084, 0.9136, 0.9225, 0.9352, 0.9523,
                   0.9744, 1.0023, 1.0377, 1.0828, 1.1425,
                   1.2285, 0.9067, 0.0 },
                 { 0.9068, 0.9100, 0.9164, 0.9263, 0.9399,
                   0.9576, 0.9801, 1.0083, 1.0436, 1.0884,
                   1.1472, 1.2316, 0.0 },
                 { 0.9077, 0.9121, 0.9196, 0.9303, 0.9446,
                   0.9628, 0.9857, 1.0140, 1.0493, 1.0937,
                   1.1517, 1.2345, 0.9062 }
                 
    }; 
  
BesselCoef::BesselCoef(Pt::uint32_t order, double sampleFrequency,double lowerFrequnecy, double upperFrequency, double delta, double stopRipple) 
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

bool BesselCoef::calcCoef(std::vector<double>& a, std::vector<double>& b)
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

    // To find the type of filter(LP, HP, BP)
    pb_1 = _lowerFrequnecy;
    pb_2 = _upperFrequency;
    sb_1 = _lowerFrequnecy - _delta;
    sb_2 = _upperFrequency + _delta;

    if( pb_1 == 0.0  || (sb_1 < 0.0)) 
    {
        lp_flag = true;
        hp_flag = false;
        bp_flag = false;
    }

    else if( (pb_1 != 0.0) && (pb_2 >= (_sampleFrequency/2.0))) 
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

    if(lp_flag) 
    {
        analog_fp = std::tan(Pi * pb_2 / _sampleFrequency);
        
        analog_fs = std::tan(Pi *  sb_2 / _sampleFrequency);

        epsilon_pass = std::sqrt(std::pow(10,(_passRipple/10))-1);
        epsilon_stop = std::sqrt(std::pow(10,(_stopRipple/10))-1); 
        double ripple_ratio = epsilon_stop / epsilon_pass;
        double freq_ratio = analog_fs / analog_fp;
        
        if(_order == 0) 
            _order = (int) std::ceil(std::log(epsilon_stop/epsilon_pass) / std::log(analog_fs/analog_fp));
        
        if(_order > 25) 
            _order = 25;

        analog_omega = analog_fp / std::pow(epsilon_pass,(1/(double)_order));	    
    }

    if(hp_flag) 
    {
        analog_fp = 1.0 / std::tan(Pi * pb_1 / _sampleFrequency);
        
        analog_fs = 1.0 / std::tan(Pi * sb_1 / _sampleFrequency);
        
        epsilon_pass = std::sqrt(std::pow(10,(_passRipple/10))-1);

        epsilon_stop = std::sqrt(std::pow(10,(_stopRipple/10))-1); 

        double ripple_ratio = epsilon_stop / epsilon_pass;

        double freq_ratio = analog_fs / analog_fp;

        if(_order == 0) 
            _order = (int) std::ceil(std::log(epsilon_stop/epsilon_pass) / std::log(analog_fs/analog_fp));

        if(_order > 25) 
            _order = 25;

        analog_omega = analog_fp / std::pow(epsilon_pass,(1/(double)_order));

    }

    if(bp_flag) 
    {
        double omega_pa = 2 * Pi * pb_1 / _sampleFrequency;

        double omega_pb = 2 * Pi * pb_2 / _sampleFrequency;

        c_bp = std::sin(omega_pa + omega_pb) / (std::sin(omega_pa) + std::sin(omega_pb));

        analog_fp = std::abs((c_bp - std::cos(omega_pb)) /std::sin(omega_pb));

        double omega_sa = 2 * Pi * sb_1 / _sampleFrequency;

        double omega_sb = 2 * Pi * sb_2 / _sampleFrequency;

        double analog_fs1 = (c_bp - std::cos(omega_sa)) / std::sin(omega_sa);
        double analog_fs2 = (c_bp - std::cos(omega_sb)) / std::sin(omega_sb);

        analog_fs = std::min(std::abs(analog_fs1),std::abs(analog_fs2));

        epsilon_pass = std::sqrt(std::pow(10,(_passRipple/10))-1);

        epsilon_stop = std::sqrt(std::pow(10,(_stopRipple/10))-1);

        double ripple_ratio = epsilon_stop / epsilon_pass;

        double freq_ratio = analog_fs / analog_fp;

        if(_order == 0) 
            _order = (int) std::ceil(std::log(epsilon_stop/epsilon_pass) / std::log(analog_fs/analog_fp));

        if(_order > 25)
            _order = 25;

        analog_omega = analog_fp / std::pow(epsilon_pass,(1/(double)_order));    
    }

    // compute coefficients
    int i;
    std::vector<double> omega(2*_order);
    double a0 =0.0;
    std::vector<double> si(2*_order);

    // if the filter order is odd then there is a 
    // first order filter when considering cascade form
    if(lp_flag) 
    {
        if((_order == 1)) 
        {
            omega[0] = abs_poles[0][0] * analog_omega;
            a0 = omega[0] + 1.0;
            a[0] = 1.0;
            //data.real_ar_lag[0] = 0;
            a[1] = (omega[0] - 1.0) / a0;
            //data.real_ar_lag[1] = -1;
            b[0] = omega[0] / a0;
            //data.real_ma_lag[0] = 0;
            b[1] = omega[0] / a0;
            //data.real_ma_lag[0] = -1;
        }
        else if(_order > 1) 
        {
            int taps = 0;
            taps = (int) _order;
            // calculate the coefficients for the first 2nd order
            si[0] = - 2 * poles[_order-1][0] / abs_poles[_order-1][0];
            omega[0] = abs_poles[_order-1][0] * analog_omega;
            a0 = 1 + si[0] * omega[0] + omega[0] * omega[0];

            den_prev_array[0] = 1.0;
            den_prev_array[1] = (2 * omega[0] * omega[0] - 2.0) /a0;
            den_prev_array[2] = (1 - si[0] * omega[0] + omega[0] * omega[0]) / a0;

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
                    den_next_array.resize(den_prev_array.size() +den_curr_array.size() - 1,0);
                    num_next_array.resize(num_prev_array.size()+num_curr_array.size() - 1,0);

                    omega[i] = abs_poles[_order-1][i] * analog_omega;
                    si[i] = - 2 * poles[_order-1][i] / abs_poles[_order-1][i];
                    a0 = 1 + si[i] * omega[i] + omega[i] * omega[i];
                    
                    den_curr_array[0] = 1.0;
                    den_curr_array[1] = (2 * omega[i] * omega[i] - 2.0) /a0;
                    den_curr_array[2] = (1 - si[i] * omega[i] + omega[i] * omega[i]) / a0;

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
                    omega[i] = abs_poles[_order-1][i] * analog_omega;
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
        if((_order == 1))
        {
            omega[0] = abs_poles[0][0] * analog_omega;
            a0 = omega[0] + 1.0;
            a[0] = 1.0;
            //data.real_ar_lag[0] = 0;
            a[1] = - (omega[0] - 1.0) / a0;
            //data.real_ar_lag[1] = -1;
            
            b[0] = omega[0] / a0;
            //data.real_ma_lag[0] = 0;
            b[1] =  - omega[0] / a0;
            //data.real_ma_lag[1] = -1;
        }
        else if(_order > 1) 
        {
            int taps = 0;
            taps = (int) _order;
            // calculate the coefficients for the first 2nd order
            //
            si[0] = - 2 * poles[_order-1][0] / abs_poles[_order-1][0];
            omega[0] = abs_poles[_order-1][0] * analog_omega;
            a0 = 1 + si[0] * omega[0] + omega[0] * omega[0];
            
            den_prev_array[0] = 1.0;
            den_prev_array[1] = - (2 * omega[0] * omega[0] - 2.0) /a0;
            den_prev_array[2] = (1 - si[0] * omega[0] +
                         omega[0] * omega[0]) / a0;
            
            num_prev_array[0] = omega[0] * omega[0] / a0;
            num_prev_array[1] = - 2 * omega[0] * omega[0] / a0;
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
                    omega[i] = abs_poles[_order-1][i] * analog_omega;
                    si[i] = - 2 * poles[_order-1][i] / abs_poles[_order-1][i];
                    a0 = 1 + si[i] * omega[i] + omega[i] * omega[i];

                    den_curr_array[0] = 1.0;
                    den_curr_array[1] = - (2 * omega[i] * omega[i] - 2.0) /a0;
                    den_curr_array[2] = (1 - si[i] * omega[i] + omega[i] * omega[i]) / a0;

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
                    omega[i] = abs_poles[_order-1][i] * analog_omega;
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
        if((_order == 1)) 
        {
            omega[0] = abs_poles[0][0] * analog_omega;
            a0 = omega[0] + 1.0;
            a[0] = 1.0;
            //data.real_ar_lag[0] = 0;
            a[1] = - (2 * c_bp) / a0;
            //data.real_ar_lag[1] = -1;
            a[2] =  (1.0 - omega[0]) / a0;
            //data.real_ar_lag[2] = -2;
            
            b[0] = omega[0] / a0;
            //data.real_ma_lag[0] = 0;
            b[1] =  0.0;
            //data.real_ma_lag[1] = -1;
            b[2] =  - omega[0] / a0;
            //data.real_ma_lag[2] = -2;
        }
        else if(_order > 1) 
        {
            int taps = 0;
            taps = (int)_order;
            // calculate the coefficients for the first 2nd order
            //
            si[0] = - 2 * poles[_order-1][0] / abs_poles[_order-1][0];
            omega[0] = abs_poles[_order-1][0] * analog_omega;
            a0 = 1 + si[0] * omega[0] + omega[0] * omega[0];

            den_prev_array[0] = 1.0;
            den_prev_array[1] = 2 * c_bp * ((-si[0]) * omega[0] - 2.0) / a0;
            den_prev_array[2] = 2 * ( 2 * c_bp * c_bp + 1.0 - 
                          omega[0] * omega[0]) / a0;
            den_prev_array[3] = - 2 * c_bp * (omega[0] *(-si[0]) + 2.0) / a0;

            den_prev_array[4] = (1.0 - si[0] * omega[0] + omega[0] * omega[0]) / a0;

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
                    omega[i] = abs_poles[_order-1][i] * analog_omega;
                    si[i] = - 2 * poles[_order-1][i] / abs_poles[_order-1][i];
                    a0 = 1 + si[i] * omega[i] + omega[i] * omega[i];

                    den_curr_array[0] = 1.0;
                    den_curr_array[1] = 2 * c_bp * ((-si[i]) * omega[i] - 2.0) / a0;

                    den_curr_array[2] = 2 * ( 2 * c_bp * c_bp + 1.0 - 
                                  omega[i] * omega[i]) / a0;
                    den_curr_array[3] = - 2 * c_bp * (omega[i] *(-si[i]) + 2.0) / a0;

                    den_curr_array[4] = (1.0 - si[i] * omega[i] + omega[i] * omega[i])/ a0;

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
                    den_next_array.resize(den_prev_array.size() + den_curr_array.size() - 1,0);
                    num_next_array.clear();
                    num_next_array.resize(num_prev_array.size() + num_curr_array.size() - 1,0);
                    omega[i] = abs_poles[_order-1][i] * analog_omega;
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

    a.resize(_order);
    b.resize(_order);
    for( i = 0; i < static_cast<int>(_order); ++i) 
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


void BesselCoef::mult(std::vector<double>& ip, std::vector<double>& op, std::vector<double>& next) 
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
