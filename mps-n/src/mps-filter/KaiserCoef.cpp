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
#include "KaiserCoef.h"
#include <cmath>
#include <Pt/Math/MathUtils.h>

namespace mps{
namespace filter{
 
KaiserCoef::KaiserCoef(Pt::uint32_t order, double sampleFrequency,double lowerFrequnecy, double upperFrequency, double delta, double stopRipple) 
, _passRipple(0)
, _order(order)
, _sampleFrequency(sampleFrequency)
, _lowerFrequnecy(lowerFrequnecy)
, _upperFrequency(upperFrequency)
, _delta(delta)
, _stopRipple(stopRipple)
{
}

bool KaiserCoef::calcCoef(std::vector<double>& aa, std::vector<double>& bb)
{
    int N;
    int order;
    int min_order;
    int k;
    double alpha;
    double c, w, w_norm;
    double sum, sum_l, sum_u;
    int counter; 

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

    aa.resize(_order);
    bb.resize(_order);
    
    // Check for user input of order
    if (_order == 0) 
    {
        // Compute the order
        order = (int)std::ceil((data.attenuation - 7.95) /(28.72 * data.delta / data.sample_frequency));
        
        // Make Sure N Is Odd
        order = (order/2) * 2 + 1;
    }
    else if ( ((data.f_order % 2) == 0.0) || ( data.f_order < 0 ) )
    {
        return false;
    }
    else 
    {
        order = _order;
    }
    

    // If N<1 then you can't compute filter... exit
    if (order < 1)
        return false;
    
    // Check if order is odd
    if ( ( order%2 ) == 0.0 )
        return false;
    
    // Set the order Data.java for use in computing
    // phase and magnitude
    _order = order;
    
    // Set split the order up for computation of -,+, and 0
    N = (order - 1) / 2;
    
    //  Check if the actual order specified is sufficiently large.
    min_order = (int)std::ceil((data.attenuation - 7.95) /(28.72 * data.delta / data.sample_frequency));
    
    if (order < min_order)
        return false;
    
    // Compute Beta
    if (data.attenuation > 50.0)
        alpha = 0.1102 * (data.attenuation - 8.7);
    else if (data.attenuation > 21.0) 
        alpha = 0.5842  * std::pow(data.attenuation - 21.0, 0.4)+ 0.07886 * (data.attenuation - 21.0);
    else
        alpha = 0.0;
    
    // Compute Normalization Factor
    w_norm = Bessel::computeBessel(alpha);
    
    // Compute c(k) and w(k)
    counter = 0;
    k       = -N;
    
    while (k < 0) 
    {
        // The Alpha Multiplier
        sum = std::sqrt(1 - std::pow(k/N, 2.0)) * alpha;
        
        // Compute w(k)
        w = Bessel::computeBessel(sum) / w_norm;
        
        // Compute c(k)

        sum_u  = std::sin(data.M_TWOPI * k * (data.f_upper / data.sample_frequency));
        sum_l = std::sin(data.M_TWOPI * k * data.f_lower / data.sample_frequency);
        
        c     = (sum_u - sum_l) / k / data.M_PI;
        
        // Compute The Filter Coefficients
        bb[counter] = c * w;
        //data.real_ma_lag[counter]  = k;
        
        // Update Counters
        //
        counter++;
        k++;
    }
    
    // Set Value At Zero
    //
    bb[counter] = 2 * (data.f_upper - data.f_lower) / data.sample_frequency;
    //data.real_ma_lag[counter]  = 0;
    counter++;
    
    // Copy Remaining Coeffici=ents
    //
    k = N;
    k--;
    while (k >= 0) {
        data.real_ma_coef[counter] = data.real_ma_coef[k];
        data.real_ma_lag[counter]  = -data.real_ma_lag[k];
        counter++;
        k--;
    }
    
    // exit gracefully
    //
    return true;
    
    }
    
    
    //****************************************************************
    //
    // public methods
    //
    //****************************************************************
    
    
} // end class Filter_Kaiser
