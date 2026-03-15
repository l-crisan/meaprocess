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
#ifndef MPS_IIRFILTER_BESSEL_H
#define MPS_IIRFILTER_BESSEL_H

#include <vector>

namespace mps{
namespace filter{

class Bessel
{
public:
    static double computeBessel(double num);

private:
    static double compute_bessel_0(double num);
    static double compute_bessel_1(double num);   
    static double compute_bessel_i(int n1, double num );

private:
    static const double BESSEL_ACC;
    static const double BESSEL_BIGNO;
    static const double BESSEL_BIGNI;
    static int  n1;   //ToDo: make re-entrant!

private:
    Bessel()
    { }
};

}}

#endif
