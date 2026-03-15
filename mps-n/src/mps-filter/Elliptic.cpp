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
#include "Elliptic.h"
#include <cmath>

namespace mps {
namespace filter {


Elliptic::Elliptic(int n, double dbr, double fs, double f1, double f2, double fthree)
    : main_common(80)
{
    std::vector<double> f3(1);
    f3[0] = fthree;

    design_elliptic_iir(n, dbr, fs, f1, f2, f3, main_common);
}

double Elliptic::square(double x)
{
    return x*x;
}

double Elliptic::akk(Common& p, double ak, double akp)
{
    int j;
    p.a[1] = 1;
    p.b[1] = akp;
    p.c[1] = ak;
    j = 1;
    do {
        p.a[j + 1] = 0.5 * (p.a[j] + p.b[j]);
        p.b[j + 1] = std::sqrt(p.a[j] * p.b[j]);
        p.c[j + 1] = 0.5 * (p.a[j] - p.b[j]);
        j++;
    } while ((j <= 19) && (p.c[j] > 1.111e-16));
    if (j > 19)
    {

    }
    p.jp = j;

    return 0.5 * Pt::pi<double>() / p.a[j];
}

double Elliptic::asn(Common& pc, double z)
{
    int j, kq, kh;
    double p, two, q, tt;

    p = std::atan(z);
    two = 1;
    kq = 1;
    for (j = 1; j <= pc.jp; j++)
    {
        pc.f[j] = p / (pc.a[j] * two);
        two *= 2;
        kq = 2 * kq - 1;
        q = pc.b[j] * std::sin(p) / (pc.a[j] * std::cos(p));
        tt = std::atan(q);
        if (tt < 0)
            kq++;
        kh = kq / 2;
        p += tt + 3.14159265358979324 * (double)kh;
    }
    return pc.f[pc.jp];
}

void Elliptic::scn(Common& pc, double u, std::vector<double>& sn, std::vector<double>& cn, std::vector<double>& dn, double ak)
{
    int k, jpm, jpmk, n;
    double p, q;
    jpm = pc.jp - 1;
    n = jpm - 1;
    p = std::pow(2.0, (double)n) * pc.a[jpm] * u;
    for (k = 1; k <= n; k++)
    {
        jpmk = jpm + 1 - k;
        q = pc.c[jpmk] * std::sin(p) / pc.a[jpmk];
        p = 0.5 * (p + std::asin(q));
    }
    sn[0] = std::sin(p);
    cn[0] = std::cos(p);
    dn[0] = std::sqrt(1 - square(ak * sn[0]));
}

void Elliptic::sing(Common& p, int n, double u0, double ak, double wc, std::vector<double>& x, std::vector<int>& np, std::vector<int>& nz)
{
    int l, lr, li, lm;
    double akp = 0, z = 0, den = 0;
    std::vector<double> sn(1);
    std::vector<double> cn(1);
    std::vector<double> dn(1);
    std::vector<double> s(1);
    std::vector<double> c(1);
    std::vector<double> d(1);
    std::vector<double> s1(1);
    std::vector<double> c1(1);
    std::vector<double> d1(1);

    akp = std::sqrt(1 - square(ak));
    akk(p, akp, ak);  //Return value not used.
    scn(p, u0, s1, c1, d1, akp);
    nz[0] = n / 2;
    np[0] = (n + 1) / 2;
    x[0] = akk(p, ak, akp);
    for (lm = 1; lm <= nz[0]; lm++) {
        l = n + 1 - lm - lm;
        z = (double)l * x[0] / (double)n;
        lr = 2 * np[0] + 2 * lm - 1;
        li = lr + 1;
        scn(p, z, sn, cn, dn, ak);
        p.f[lr] = 0;
        p.f[li] = wc / (ak*sn[0]);
    }
    for (lm = 1; lm <= np[0]; lm++) {
        l = n + 1 - lm - lm;
        z = (double)l * x[0] / (double)n;
        lr = 2 * lm - 1;
        li = lr + 1;
        scn(p, z, s, c, d, ak);
        den = square(c1[0]) + square(ak*s[0] * s1[0]);
        p.f[lr] = -wc * c[0] * d[0] * s1[0] * c1[0] / den;
        p.f[li] = wc * s[0] * d1[0] / den;
    }
}

double Elliptic::cay(double q)
{
    bool  qf_too_big = true;
    double ad, an, qf, qp;

    an = 1;
    ad = 1;
    qf = 1;
    qp = q;

    do {
        qf *= qp;
        an += 2 * qf;
        if (qf < 1.111e-16) qf_too_big = false;
        else
        {
            qf *= qp;
            ad += qf;
            qp *= q;
            if (qf > 1.111e-16)
                qf_too_big = true;
            else
                qf_too_big = false;
        }
    } while (qf_too_big);
    return 4 * std::sqrt(q) * square(ad / an);
}

void Elliptic::dbdwn(Common& p, std::vector<double>& f3, int icase, double c, double cgam, double dbr, int n)
{
    double epsq, epsil, as, ak1, ak1p, u, v, qp, akp, ak, wr, rad, cphi, x;

    epsq = std::exp(dbr / 4.34294481904) - 1;
    epsil = std::sqrt(epsq);
    as = std::exp(-f3[0] / 4.34294481904);
    ak1 = epsil / std::sqrt(as - 1);
    ak1p = std::sqrt(1 - square(ak1));
    u = akk(p, ak1, ak1p);
    v = akk(p, ak1p, ak1);
    qp = std::exp(-Pt::pi<double>() *n*u / v);
    akp = cay(qp);
    ak = std::sqrt(1 - square(akp));
    wr = 1 / ak;
    if (icase == 3 || icase == 4) wr = 1 / wr;
    if ((icase & 1) != 0)
    {
        f3[0] = p.fs*std::atan(c*wr) / Pt::pi<double>();
    }
    else
    {
        x = square(c*wr);
        rad = x*(1 - square(cgam)) + square(x);
        cphi = (cgam + std::sqrt(rad)) / (1 + x);
        f3[0] = std::acos(cphi) * p.fs / (2 * Pt::pi<double>());
    }
}

void Elliptic::quad(double zr, double zi, std::vector<double>& qr, std::vector<double>& qm)
{
    qr[0] = -zr - zr;
    qm[0] = zr*zr + zi*zi;
    if (zi < 1.111e-16)
    {
        qm[0] = 0;
        qr[0] = -zr;
    }
}

void Elliptic::zpln(Common& p, std::vector<int>& np, std::vector<int>& nz, double c, double cgam, int icase, double scale, int n)
{
    int j, nt, nr, ni, nd, nn, nc, jt, jj, ir, ii, kk, m, mp1, icnt, k;
    int mh, iie, j1, jh, jl, nnp, icntp, czz, cpp;
    double den, rr, ri, ar, ai, br, bi, qi, qa, sqqr, sqqi;
    double ssign, an, pn, gam, cng, q;
    std::vector<std::vector<double> > z;
    z.resize(2);
    z[0].resize(4 * n + 2, 0);
    z[1].resize(4 * n + 2, 0);
    std::vector<double> qr(1);
    std::vector<double> qm(1);
    int new_np = np[0];
    int new_nz = nz[0];

    pn = 0;
    ni = 0;
    m = 0;
    qr[0] = 0;
    qm[0] = 0;
    if (icase >= 3)
    {
        nt = new_np + new_nz;
        for (j = 1; j <= nt; j++)
        {
            nr = 2 * j - 1;
            ni = nr + 1;
            den = square(p.f[nr]) + square(p.f[ni]);
            p.f[nr] /= den;
            p.f[ni] /= den;
        }
        while (new_np > new_nz)
        {
            nr = ni + 1;
            ni = nr + 1;
            new_nz++;
            p.f[nr] = 0;
            p.f[ni] = 0;
        }
    }
    /*Read in s-plane roots */
    nd = new_np;
    nn = -1;
    nnp = -1;
    jt = 0;
    nc = nd;
    ii = 0;
    do
    {
        ir = ii + 1;
        ii = ir + 1;
        rr = p.f[ir];
        ri = p.f[ii];

        nc--;
        den = square(ri*c) + square(1 - rr*c);
        if (icase == 1 || icase == 3)
        {
            jt++;
            z[0][jt] = (1 - square(rr*c) - square(ri*c)) / den;
            z[1][jt] = -2 * ri*c / den;
            if (ri != 0)
            {
                jt++;
                z[0][jt] = z[0][jt - 1];
                z[1][jt] = -z[1][jt - 1];
            }
        }
        else
        {
            rr *= c;
            ri *= c;
            den = square(ri) + square(1 - rr);
            ar = (1 - rr)*cgam / den;
            ai = cgam*ri / den;
            br = (1 - square(rr) - square(ri)) / den;
            bi = 2 * ri / den;
            qr[0] = square(ar) - square(ai) - br;
            qi = 2 * ar * ai - bi;
            qm[0] = std::sqrt(square(qr[0]) + square(qi));
            qm[0] = std::sqrt(qm[0]);
            qa = 0.5 * std::atan2(qi, qr[0]);
            sqqr = qm[0] * std::cos(qa);
            sqqi = qm[0] * std::sin(qa);
            jt++;
            z[0][jt] = ar + sqqr;
            z[1][jt] = ai + sqqi;
            jt++;
            z[0][jt] = ar - sqqr;
            z[1][jt] = ai - sqqi;
            if (ri > 0)
            {
                jt += 2;
                for (kk = 1; kk <= 2; kk++)
                {
                    jj = jt - 2 + kk;
                    z[0][jj] = z[0][jj - 2];
                    z[1][jj] = -z[1][jj - 2];
                }
            }
        }
        nnp = nn;
        if (nc <= 0)
        {
            if (nn < 0)
            {
                nn = new_nz;
                m = jt;
                if (nn > 0)  nc = nn;
            }
        }
    } while (nc > 0 || (nnp < 0 && nz[0] > 0));

    /* Insert numerator zeroes as needed */
    while (2 * m - jt > 0)
    {
        jt++;
        z[0][jt] = -1;
        z[1][jt] = 0;
        if (icase == 2 || icase == 4)
        {
            jt++;
            z[0][jt] = 1;
            z[1][jt] = 0;
        }
    }

    /* Create the polynomials */
    mp1 = m + 1;
    icnt = 0;
    do
    {
        for (j = 1; j <= mp1; j++)
        {
            p.b[j] = 0;
            p.c[j] = 0;
        }
        p.b[1] = 1;
        for (j = 1; j <= m; j++)
            for (kk = 1; kk <= j; kk++)
            {
                k = j + 1 - kk;
                jj = j + m * icnt;
                p.b[k + 1] += -z[0][jj] * p.b[k] + z[1][jj] * p.c[k];
                p.c[k + 1] -= z[1][jj] * p.b[k] + z[0][jj] * p.c[k];
            }
        icntp = icnt;
        if (icnt <= 0)
        {
            icnt = 1;
            for (j = 1; j <= mp1; j++) p.a[j] = p.b[j];
        }
    } while (icntp <= 0);

    /* Normalize to s =   0 */
    ssign = 1;
    if (icase != 2)
    {
        if (icase == 3)  ssign = -1;
        pn = 1;
        an = 1;
        for (j = 2; j <= mp1; j++)
        {
            pn = ssign * pn + p.b[j];
            an = ssign * an + p.a[j];
        }
    }
    else
    {  /* icase == 2 */
        gam = std::acos(cgam);
        mh = m / 2;
        pn = p.b[mh + 1];
        an = p.a[mh + 1];
        iie = m / 4;
        ai = 0;
        if (mh - 2 * iie > 0)
        {
            ai = 1;
            pn = 0;
            an = 0;

        }
        for (j = 1; j <= mh; j++)
        {
            j1 = j;
            cng = std::cos(j1 * gam - 0.5 * ai * Pt::pi<double>());
            jh = mh + 1 + j;
            jl = mh + 1 - j;
            pn += cng * (p.b[jh] + (1 - 2 * ai)*p.b[jl]);
            an += cng * (p.a[jh] + (1 - 2 * ai)*p.a[jl]);
        }
    }
    q = an / (pn * scale);
    for (j = 1; j <= mp1; j++)
        p.b[j] *= q;

    /* The filter parameters are printed out */
    czz = cpp = 0;
    for (j = 1; j <= m; j++)
    {
        if (z[1][j] >= 0)
        {
            quad(z[0][j], z[1][j], qr, qm);



            p.res[cpp].db1 = qr[0];
            p.res[cpp].db2 = qm[0];
            cpp++;
        }
    }
    for (j = 1; j <= m; j++)
    {
        jj = j + m;
        if (z[1][jj] >= 0)
        {
            quad(z[0][jj], z[1][jj], qr, qm);
            p.res[czz].da1 = qr[0];
            p.res[czz].da2 = qm[0];
            p.res[czz].dg = 1;
            czz++;
        }
    }
    p.res[0].dg = p.b[1];
}


void Elliptic::ellip(Common& p, int n, double wr, double dbr, int icase, double c, double cgam)
{
    int nh, nc;
    double wc, epsq, epsil, psq, ak, akp, v, as, y, q, scale, ak1, ak1p,
        rho, theta, dbd, z, u0;

    std::vector<double> x(1);
    std::vector<int>   np(1);
    std::vector<int>   nz(1);

    np[0] = 0;
    nz[0] = 0;
    wc = 1;
    epsq = std::exp(dbr / 4.34294481904) - 1;
    epsil = std::sqrt(epsq);
    psq = 1 - 1 / (1 + epsq);
    rho = std::sqrt(psq);
    ak = wc / wr;
    akp = std::sqrt(1 - square(ak));
    x[0] = akk(p, ak, akp);
    y = akk(p, akp, ak);
    q = std::exp(-Pt::pi<double>() * (double)n * y / x[0]);
    scale = 1;
    nh = n / 2;
    nc = nh + nh;
    if (nc == n)  scale = std::sqrt(1 + epsq);
    ak1 = cay(q);
    ak1p = std::sqrt(1 - square(ak1));
    v = akk(p, ak1p, ak1);
    as = 1 + square(epsil / ak1);
    dbd = -4.34294481904 * std::log(as);
    theta = std::asin(wc / wr) * 180 / Pt::pi<double>();
    z = 1 / epsil;
    u0 = y * asn(p, z) / v;
    sing(p, n, u0, ak, wc, x, np, nz);
    zpln(p, np, nz, c, cgam, icase, scale, n);
}

void Elliptic::design_elliptic_iir(int n, double dbr, double fs, double f1, double f2, std::vector<double>& f3, Common& common)
{
    int  j;
    double dbd, fnyq, x, bw, ang, cang, sang, c, tfo, cgam, wr, q, den;
    std::vector<int> icase(1);

    /* Specifications entered in z-plane, converted to s-plane.
        Case 1- Low Pass
        Case 2- Band Pass
        Case 3- High Pass
        Case 4- Band Reject
        In s-plane all filters have pass band edge at w=  wc=  1. Stop =
        band edge at wr.
    */

    common.fs = fs;
    dbd = -f3[0];
    fnyq = 0.5 * common.fs;
    icase[0] = 2;
    if (f2 < f1)
    {
        x = f2;
        f2 = f1;
        f1 = x;
        icase[0] = 4;
    }
    if (f2 == f1) {
        return;
    }
    bw = f2 - f1;
    if (f1 <= 0)
    {
        icase[0] = 1;
        f1 = 0;
        if (dbd <= dbr)
            if (f3[0] <= f2 || f3[0] >= fnyq)
            {
                return;
            }
    }
    else
    {
        if (f2 >= fnyq)
        {
            f2 = fnyq;
            bw = f1;
            icase[0] = 3;
            if (dbd <= dbr)
                if (f3[0] <= 0 || f3[0] > f1)
                {
                    return;
                }

        }
        else
        {
            if (dbd <= dbr)
            {
                if (f3[0] < f1 || f3[0] > f2) icase[0] = 2;
                if (f3[0] >= fnyq || f3[0] <= 0)
                {
                    return;
                }
                if (f3[0] < f2  &&  f3[0] > f1) icase[0] = 4;
            }
        }
    }
    ang = bw * Pt::pi<double>() / common.fs;
    cang = std::cos(ang);
    c = std::sin(ang) / cang;
    tfo = f1 + f2;
    cgam = std::cos(tfo *Pt::pi<double>() / common.fs) / cang;
    if (dbd > dbr)  dbdwn(common, f3, icase[0], c, cgam, dbr, n);
    ang = f3[0] * Pt::pi<double>() / common.fs;
    cang = std::cos(ang);
    sang = std::sin(ang);
    if (icase[0] == 1 || icase[0] == 3)
    {
        wr = sang / (cang * c);
    }
    else
    {
        q = square(cang) - square(sang);
        sang = 2 * cang * sang;
        cang = q;
        wr = (cgam - cang) / (sang * c);
    }
    if (icase[0] == 3 || icase[0] == 4)  wr = 1 / wr;
    wr = std::abs(wr);
    common.c[1] = 1;
    common.c[2] = wr;
    if (icase[0] == 3 || icase[0] == 4)  common.c[2] = 1 /
        common.c[2];
    if (icase[0] == 1 || icase[0] == 3)
    {
        for (j = 1; j <= 2; j++)
            common.a[j] = std::atan(c * common.c[j]) *
            common.fs / Pt::pi<double>();
    }
    else
    {
        for (j = 1; j <= 2; j++)
        {
            den = std::atan(c * common.c[j]);
            q = std::sqrt(1 + square(c * common.c[j]) -
                square(cgam));
            q = std::atan2(q, cgam);
            common.a[j] = (q + den) * fnyq / Pt::pi<double>();
            common.b[j] = (q - den) * fnyq / Pt::pi<double>();
        }
    }
    ellip(common, n, wr, dbr, icase[0], c, cgam);
}

}}
