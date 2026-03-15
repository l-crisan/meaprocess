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
#include <mps/fft/mps-fft.h>
#include "tools/kiss_fftr.h"

extern "C"
{

MPS_FFT_API Pt::uint64_t mpsCreateFFT(int samples, bool ifft)
{
    if(ifft)
        return  (Pt::uint64_t)kiss_fftr_alloc((int) samples, 1, 0, 0);
    else
        return (Pt::uint64_t) kiss_fftr_alloc((int) samples, 0, 0, 0);
}

MPS_FFT_API void mpsFFT(Pt::uint64_t fftHandle, const double* timeData, mpsFFTComplex* freqData)
{
    kiss_fftr_cfg kiss_cfg = (kiss_fftr_cfg) fftHandle;
    kiss_fftr(kiss_cfg, timeData, (kiss_fft_cpx*) (freqData));
}

MPS_FFT_API void  mpsIFFT(Pt::uint64_t  fftHandle, const mpsFFTComplex* freqData, double* timeData)
{
    kiss_fftr_cfg kiss_cfg = (kiss_fftr_cfg) fftHandle;
    kiss_fftri(kiss_cfg, (kiss_fft_cpx*)freqData, timeData);
}

MPS_FFT_API void  mpsFreeFFT(Pt::uint64_t fftHandle)
{
    kiss_fftr_cfg kiss_cfg = (kiss_fftr_cfg) fftHandle;
    kiss_fftr_free(kiss_cfg);
}

}
