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
#ifndef MPS_LICENCE_DES_H
#define MPS_LICENCE_DES_H

#include <stdio.h>
#include <string.h>
#include <mps/licence/Licence.h>

namespace mps{
namespace licence{

class MPS_LICENCE_API Des
{
public:
    inline void setKey(const int* key64)
    {
      memcpy(&key[0], key64, 64*sizeof(int));
    }

    const Pt::uint8_t* encrypt(const Pt::uint8_t* data, int size);
    const Pt::uint8_t* decrypt(const Pt::uint8_t* data, int size);

 private:
     Pt::uint8_t text[1000];

    int keyi[16][48],
      total[64],
      left[32],
      right[32],
      ck[28],
      dk[28],
      expansion[48],
      z[48],
      xor1[48],
      sub[32],
      p[32],
      xor2[32],
      temp[64],
      pc1[56],
      ip[64],
      inv[8][8];

    int key[64];

    Pt::uint8_t final[1000];
    void IP();
    void PermChoice1();
    void PermChoice2();
    void Expansion();
    void inverse();
    void xor_two();
    void xor_oneE(int);
    void xor_oneD(int);
    void substitution();
    void permutation();
    void keygen();
};

}}

#endif
