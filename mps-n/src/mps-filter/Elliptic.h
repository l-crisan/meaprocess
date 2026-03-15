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
#ifndef MPS_IIRFILTER_ELLIPTIC_H
#define MPS_IIRFILTER_ELLIPTIC_H

#include <vector>
#include <Pt/Math.h>

namespace mps{
namespace filter{ 

struct SecondOrderIIRRecord 
{
    SecondOrderIIRRecord()
    : dg(0.0)
    , da1(0.0)
    , da2(0.0)
    , db1(0.0)
    , db2(0.0)
    {}

    double dg;   /* Filter gain                     */
    double da1;  /* Filter coefficient, numerator   */
    double da2;  /* Filter coefficient, numerator   */
    double db1;  /* Filter coefficient, denominator */
    double db2;
};

class Common 
{
public:
    Common()
    : ip(0)
    , jp(0)
    , fs(0)
    {
    }

    Common(int n) 
    : ip(0)
    , jp(0)
    , fs(0)
    {
        a.resize(n,0);
        b.resize(n,0);
        c.resize(n,0);
        f.resize(2*n+1,0);
        res.resize(n);
    }    

    int ip;
    int jp;
    double fs;
    
    std::vector<double> a;
    std::vector<double> b;
    std::vector<double> c;
    std::vector<double> f;
    
    std::vector<SecondOrderIIRRecord> res;
};

class Elliptic
{
public:
    Elliptic(int n, double dbr, double fs, double f1, double f2, double fthree);
    static double square(double x);   
    static double akk(Common& p, double ak, double akp);
    static double asn(Common& pc, double z);
    static void scn(Common& pc, double u, std::vector<double>& sn, std::vector<double>& cn, std::vector<double>& dn, double ak);   
    static void sing(Common& p, int n, double u0, double ak, double wc, std::vector<double>& x, std::vector<int>& np, std::vector<int>& nz);   
    static double cay(double q);	
    static void dbdwn(Common& p, std::vector<double>& f3, int icase, double c,double cgam, double dbr, int n);
    static void quad(double zr, double zi, std::vector<double>& qr, std::vector<double>& qm);    
    static void zpln(Common& p, std::vector<int>& np, std::vector<int>& nz, double c, double cgam, int icase, double scale, int n);       
    static void ellip( Common& p,  int n,  double wr,  double dbr, int icase, double c,  double cgam);    
    static void design_elliptic_iir( int n,  double dbr,  double fs,double f1,  double f2,  std::vector<double>& f3, Common& common);
    
    double getcoefa(int position)
    { return main_common.a[position+1]; }
    
    double getcoefb(int position)
    { return main_common.b[position+1]; }
private:
    Common  main_common;
};

}}

#endif
