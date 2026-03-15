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
#include "Bessel.h"
#include <cmath>

namespace mps{
namespace filter{
    
const double Bessel::BESSEL_ACC = 40.0;
const double Bessel::BESSEL_BIGNO = 1.0e10;
const double Bessel::BESSEL_BIGNI = 1.0e-10;
int Bessel::n1 = 0;

double Bessel::compute_bessel_0(double num) 
{
    double bessel_value = 0.0;  // same thing as "ans"
    double x = num;
    double ax;
    double y;
    
    
    if (( ax = std::abs(x)) < 3.75) 
    {// Direct Rational Function Fit
        y = x / 3.75;
        y *= y;

        bessel_value = 1.0 + y *(3.5156229 + y * (3.0899424 + y * (1.2067492 + y * (0.2659732 + y * (0.360768e-1 + y * 0.45813e-2)))));
    }
    else 
    {// Fitting Function
        y = 3.75 / ax;

        bessel_value = (std::exp(ax)/std::sqrt(ax)) *(0.39894228 + y * (0.1328592e-1 + y *(0.225319e-2 + y *(-0.157565e-2 + y *(0.916281e-2 + y *
                       (-0.2057706e-1 + y * (0.2635537e-1 + y * (-0.1647633e-1 + y * 0.392377e-2))))))));
    }
    
    return bessel_value;
}
    
double Bessel::compute_bessel_1(double num) 
{
    double bessel_value = 0.0;  // same thing as "ans"
    double x = num;
    double ax;
    double y;
    
    if ((ax=std::abs(x)) < 3.75) 
    {// Direct Rational Function Fit
        y = x / 3.75;
        y *= y;
        
        bessel_value = ax * (0.5 + y *(0.87890594 + y *(0.51498869 + y * (0.15084934 + y * 
                       (0.2658733e-1 + y * (0.301532e-2 + y * 0.32411e-3))))));

    }
    else 
    {// Fitting Function
        y = 3.75 / ax;

        bessel_value = 0.2282967e-1 + y *(-0.2895312e-1 + y * (0.1787654e-1 - y * 0.420059e-2));

        bessel_value = 0.39894228 + y * (-0.3988024e-1 + y * (-0.362018e-2 + y * (0.163801e-2 + y * (-0.1031555e-1 + y * bessel_value))));

        bessel_value *= (std::exp(ax)/std::sqrt(ax));
    }
        return bessel_value;
}


double Bessel::compute_bessel_i(int n1, double num ) 
{
    int n = n1;
    double bessel_value = 0.0;  // same thing as "ans"
    double x = num;
    double bi, bim, bip, tox, ans;

    if (n1 < 2)
        return bessel_value = 1e100; // error!!!!!!! you can't have a bessel of 2

    // Check For Zero
    if (x == 0.0)
        return bessel_value = 0.0;

    // Initialize
    tox = 2.0 / std::abs(x);
    bip = 0.0;
    ans = 0.0;
    bi = 1.0;

    // Recurrence
    for (int j = 2*(n+(int)std::sqrt(BESSEL_ACC*n)); j > 0; j--) 
    {
        bim = bip + j * tox * bi;
        bip = bi;
        bi  = bim;
        
        if (std::abs(bi) > BESSEL_BIGNO) 
        {
            bessel_value *= BESSEL_BIGNI;
            bi  *= BESSEL_BIGNI;
            bip *= BESSEL_BIGNI;
        }
        
        if (j == n)
            bessel_value = bip;
    }

    bessel_value *= compute_bessel_0(x) / bi;
    return bessel_value;
}

double Bessel::computeBessel(double num) 
{
    if (n1 == 0)
        return compute_bessel_0(num);

    if (n1 == 1)
        return compute_bessel_1(num);

    return compute_bessel_i(n1, num);
}

}}
