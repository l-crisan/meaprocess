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
#include "LJObjectFactory.h"
#include "mps-labjack.h"
#include "InputPS.h"
#include "OutputPS.h"
#include "LJSignal.h"
#include "LJOutSignal.h"
#include <mps/core/ObjectVector.h>
#include <mps/core/RuntimeEngine.h>

using namespace mps::core;

namespace mps{
namespace labjack{

LJObjectFactory::LJObjectFactory(void)
{
}

LJObjectFactory::~LJObjectFactory(void)
{
}

std::string LJObjectFactory::resourceID() const
{
    return "mps-labjack";
}

Object* LJObjectFactory::createObject( const Pt::String& type, const Pt::String& subType, Pt::uint32_t id )
{
    if(type == L"Mp.LabJackU12.PS.In")
    {
        return new InputPS();
    }
    else if( subType == L"Mp.Sig.LabJack.Digital" ||
             subType == L"Mp.Sig.LabJack.Analog" ||
             subType == L"Mp.Sig.LabJack.Counter")
    {
        return new LJSignal(id);
    }
  else if( type == L"Mp.LabJackU12.PS.Out")
  {
      return new OutputPS();
  }
  else if( type == L"Mp.LabJackU12.Output")
  {
      return new ObjectVector<LJOutSignal*>();
  }
  else if( type == L"Mp.LabJackU12.OutChn")
  {
      return new LJOutSignal();
  }

    return 0;
}

extern "C"
{
    MPS_LABJACK_API ObjectFactory* mpsGetFactory()
    {
        static LJObjectFactory factory;
        return &factory;
    }
}

}}