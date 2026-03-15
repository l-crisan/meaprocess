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
#include <mps/filter/mps-filter.h>
#include <mps/filter/Filter.h>

extern "C"
{

MPS_FILTER_API Pt::uint64_t mps_createFilter(Pt::int32_t type, Pt::uint32_t order, Pt::uint32_t lowerFrequency, Pt::uint32_t upperFrequency, 
                                              Pt::uint32_t transitionBandWidth, Pt::uint32_t stopBandAttenuation, Pt::uint32_t sampleRate)
{
    mps::filter::Filter* filter = new mps::filter::Filter();
    
    if(!filter->calcCoef((mps::filter::Filter::FilterType)type, order, lowerFrequency, upperFrequency, transitionBandWidth, stopBandAttenuation, sampleRate))
    {
        delete filter;
        return 0;
    }

    return (Pt::uint64_t)(filter);
}

MPS_FILTER_API double  mps_filter(Pt::uint64_t  filterHandle, double value)
{
    mps::filter::Filter* flt = (mps::filter::Filter*) filterHandle;
    return flt->filter(value);
}

MPS_FILTER_API void  mps_startFilter(Pt::uint64_t  filterHandle)
{
    mps::filter::Filter* flt = (mps::filter::Filter*) filterHandle;
    flt->start();
}

MPS_FILTER_API void  mps_freeFilter(Pt::uint64_t filterHandle)
{
    mps::filter::Filter* flt = (mps::filter::Filter*) filterHandle;
    delete flt;
}

}
