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
#ifndef MPS_GPS_NMEAPARSER_H
#define MPS_GPS_NMEAPARSER_H

#include <Pt/Signal.h>
#include <Pt/System/IODevice.h>
#include <iostream>
#include <vector>
#include <sstream>

namespace mps{
namespace gps{

class NMEAParser
{
public:
    enum QualityOfMea
    {
        Invalid = 0,
        GPSfixSPS,
        DGPSfix,
        PPSfix,
        RealTimeKinematic,
        FloatRTK,
        Estimated,
        ManualInputMode,
        SimulationMode
    };

    NMEAParser();

    virtual ~NMEAParser();

    void parse(char ch);

    void reset();

    Pt::Signal<> dataAvailable;

    double latitude() const;

    double longitude() const;

    double altitude() const;

    Pt::uint32_t satellitesInUse() const;
    
    Pt::uint8_t day() const;

    Pt::uint8_t month() const;
    
    Pt::uint16_t year() const;

    Pt::uint8_t hour() const;
    
    Pt::uint8_t minute() const;

    Pt::uint8_t second() const;

    bool status()const;

    double groundSpeed() const;

    double trackAngle() const;

    QualityOfMea qualityOfMea() const;

    double hdop() const;

    double vdop() const;

    double pdop() const;

private:
    void parseGPRMC(const std::vector<std::string>& splittetLine);

    void parseGPGGA(const std::vector<std::string>& splittetLine);

    void parseGPGSA(const std::vector<std::string>& splittetLine);

    double getLatitude(const std::string& str1, const std::string& str2);

    double getLongitude(const std::string& str1, const std::string& str2);

    static std::vector<std::string> splittLine(std::stringstream& ss);

private:
    double  _latitude;
    double  _longitude;
    double  _altitude;
    double  _groundSpeed;
    double  _trackAngle;
    double  _hdop;
    double  _pdop;
    double  _vdop;
    Pt::uint32_t  _satellitesInUse;
    Pt::uint8_t   _day;
    Pt::uint8_t   _month;
    Pt::uint16_t  _year;
    Pt::uint8_t   _hour;
    Pt::uint8_t   _minute;
    Pt::uint8_t   _second;
    bool          _status;
    QualityOfMea  _qualityOfMea;
    std::stringstream _lineStream;
    std::string _packetType;
    std::string _strGPRMC;
    std::string _strGPGGA;
    std::string _strGPGSA;
};

}}

#endif
