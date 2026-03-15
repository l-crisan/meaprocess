//    MeaProcess - Meaurement and Automation framework.
//    Copyright (C) 2015  Laurentiu-Gheorghe Crisan
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.

#ifndef MPS_STOPPS_H
#define MPS_STOPPS_H

#include <mps/ProcessStation.h>

namespace mps {

	class Signal;

class StopPS : public ProcessStation
{

public:
	StopPS();
	~StopPS();

	virtual void onInitInstance();
	virtual void onStart();
	virtual void onUpdateDataValue(size_t noOfRecords, size_t sourceIdx, const Port* port, const Pt::uint8_t* data);

	inline Pt::uint32_t delay() const
	{
		return _delay;
	}

	inline void setDelay(Pt::uint32_t delay)
	{
		_delay = delay;
	}

private:
	size_t _srcIdx;
	const Signal* _stopSignal;
	size_t _offsetInSrc;
	bool _stoping;
	Pt::uint32_t _delay;
};

}

#endif
