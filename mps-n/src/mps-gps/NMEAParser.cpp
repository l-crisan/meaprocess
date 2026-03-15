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
#include "NMEAParser.h"
#include <assert.h>

namespace mps{
namespace gps{

double toDouble(std::string str)
{
    double d;
    std::stringstream ss;
    ss<<str;
    ss>>d;
    return d;
}

int toInt(std::string str)
{
    int d;
    std::stringstream ss;
    ss<<str;
    ss>>d;
    return d;
}

NMEAParser::NMEAParser()
{
    reset();
}

NMEAParser::~NMEAParser()
{
}

void NMEAParser::reset()
{	
    _lineStream.clear();
    _lineStream.str("");
    _packetType = "";
    _strGPRMC = "";
    _strGPGGA = "";
    _strGPGSA = "";
    _altitude = 0;
    _longitude = 0;
    _latitude = 0;
    _day = 0;
    _month = 0;
    _second = 0;
    _year = 0;
    _hour = 0;
    _minute = 0;
    _hdop = 0;
    _pdop = 0;
    _vdop = 0;
}

std::vector<std::string> NMEAParser::splittLine(std::stringstream& ss)
{
    std::vector<std::string> splittetLine;
    char buffer[100];

    while(ss)
    {
        ss.getline(buffer,100,',');
        splittetLine.push_back(buffer);
    }

    return splittetLine;
}

void NMEAParser::parse(char ch)
{
    if(ch == '$')
    {//Begin of line
        _lineStream.clear();
        _lineStream.str("");
        return;
    }
    
    if(ch == 10 || ch == 13)
    {//End of line
        std::vector<std::string> splittetLine = splittLine(_lineStream);

        if(splittetLine.size() == 0)
            return;

        if(splittetLine[0] == "GPRMC")
            parseGPRMC(splittetLine);
        else if(splittetLine[0] == "GPGGA")
            parseGPGGA(splittetLine);
        else if(splittetLine[0] == "GPGSA")
            parseGPGSA(splittetLine);
    }

    _lineStream<<ch;
}

double NMEAParser::getLongitude(const std::string& str1, const std::string& str2)
{
    std::stringstream ss;
    double d;
    double longitude;
    std::string lat = str1;
    
    lat.erase(0,3);

    if( str1[0] == '\0')
        return 0.0;

    ss<<str1[0]<<str1[1]<<str1[2];
    ss>>longitude;

    d = toDouble(lat);
    longitude += (d/60.0);

    if( str2[0] == 'W')
        longitude = -longitude;
    
    return longitude;	
}

double NMEAParser::getLatitude(const std::string& str1, const std::string& str2)
{
    std::stringstream ss;
    double latitude;
    std::string lat = str1;
    lat.erase(0,2);

    if( str1[0] == '\0')
        return 0.0;

    ss<<str1[0]<<str1[1];
    ss>>latitude;

    double d = toDouble(lat);

    latitude += (d/60.0);

    if( str2[0] == 'S')
        latitude = -latitude;

    return latitude;
}

void NMEAParser::parseGPGSA(const std::vector<std::string>& splittetLine)
{
    if(splittetLine.size() < 17)
        return;

    //PDOP	
    if( splittetLine[15][0] != '\0')
        _pdop = toDouble(splittetLine[15]);
    else
        _pdop = 0;

    //HDOP	
    if( splittetLine[16][0] != '\0')
        _hdop = toDouble(splittetLine[16]);
    else
        _hdop = 0;

    //VDOP
    if( splittetLine[17][0] != '\0')
        _vdop = toDouble(splittetLine[16]);
    else
        _vdop = 0;

    dataAvailable.send();
}

/*
GPGGA Sentence format

$GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M, ,*47
   |   |	  |			 |			 | |  |	  |		  |      | | 
   |   |	  |			 |			 | |  |	  |		  |		 | checksum data
   |   |	  |			 |			 | |  |	  |		  |		 |
   |   |	  |			 |			 | |  |	  |		  |		 empty field
   |   |	  |			 |			 | |  |	  |		  |
   |   |	  |			 |			 | |  |	  |		  46.9,M Height of geoid (m) above WGS84 ellipsoid
   |   |	  |			 |			 | |  |	  |
   |   |	  |			 |			 | |  |	  545.4,M Altitude (m) above mean sea level
   |   |	  |			 |			 | |  |
   |   |	  |			 |			 | |  0.9 Horizontal dilution of position (HDOP)
   |   |	  |			 |			 | |
   |   |	  |			 |			 | 08 Number of satellites being tracked
   |   |	  |			 |			 |
   |   |	  |			 |			 1 Fix quality:	0 = invalid
   |   |	  |			 |							1 = GPS fix (SPS)
   |   |	  |			 |							2 = DGPS fix
   |   |	  |			 |							3 = PPS fix
   |   |	  |			 |							4 = Real Time Kinematic
   |   |	  |			 |							5 = Float RTK
   |   |	  |			 |							6 = estimated (dead reckoning) (2.3 feature)
   |   |	  |			 |							7 = Manual input mode
   |   |	  |			 |							8 = Simulation mode
   |   |	  |			 |
   |   |	  |			 01131.000,E Longitude 11 deg 31.000' E
   |   |	  |
   |   |	  4807.038,N Latitude 48 deg 07.038' N	
   |   |
   |   123519 Fix taken at 12:35:19 UTC
   |
   GGA Global Positioning System Fix Data

*/
void NMEAParser::parseGPGGA(const std::vector<std::string>& splittetLine)
{
    if(splittetLine.size()< 13)
        return;

    //Time
    if(splittetLine[1][0] ==  '\0')
    {
        _hour = 0;
        _minute = 0;
        _second = 0;
    }
    else
    {
        int no;
        std::stringstream ss;
        ss<<splittetLine[1][0]<<splittetLine[1][1];
        ss>>no;
        _hour = no;

        std::stringstream ss2;
        ss2<<splittetLine[1][2]<<splittetLine[1][3];
        ss2>>no;
        _minute = no;

        std::stringstream ss3;
        ss3<<splittetLine[1][4]<<splittetLine[1][5];
        ss3>>no;
        _second = no;
    }

    //Latitude
    _latitude = getLatitude(splittetLine[2], splittetLine[3]);
    
    //Longitude
    _longitude = getLongitude(splittetLine[4], splittetLine[5]);

    // GPS quality
    if(splittetLine[6][0]  != '\0')
    {
        int quality = toInt(splittetLine[6]);
        _qualityOfMea = (QualityOfMea)quality;		
    }
    else
    {
        _qualityOfMea = (QualityOfMea)0;
    }

    // Satellites in use
    if( splittetLine[7][0] != '\0')
        _satellitesInUse = toInt(splittetLine[7]);
    else
        _satellitesInUse = 0;

    // HDOP
    if(splittetLine[8][0] != '\0')
        _hdop = toDouble(splittetLine[8]);
    else
        _hdop = 0;


    // Altitude
    if( splittetLine[9][0] != '\0')
        _altitude = toDouble(splittetLine[9]);
    else
        _altitude = 0.0;

    dataAvailable.send();
}

void NMEAParser::parseGPRMC(const std::vector<std::string>& splittetLine)
{
    if(splittetLine.size() < 10)
        return;
    
    int no;
    //Time
    if(splittetLine[1][0] != '\0')
    {
        std::stringstream ss;
        ss<<splittetLine[1][0]<<splittetLine[1][1];
        ss>>no;
        _hour = no;

        std::stringstream ss2;
        ss2<<splittetLine[1][2]<<splittetLine[1][3];
        ss2>>no;
        _minute = no;

        std::stringstream ss3;
        ss3<<splittetLine[1][4]<<splittetLine[1][5];
        ss3>>no;
        _second = no;
    }
    else
    {
        _hour = 0;
        _minute = 0;
        _second = 0;
    }

    //Status
    _status = splittetLine[2][0] == 'A';

    //Latitude
    _latitude = getLatitude(splittetLine[3], splittetLine[4]);

    //Longitude
    _longitude = getLongitude(splittetLine[5], splittetLine[6]);
    
    //Ground speed
    if(splittetLine[7][0] != '\0')
        _groundSpeed = toDouble(splittetLine[7]);
    else
        _groundSpeed = 0;
    
    //Track angle
    if(splittetLine[8][0] != '\0')
        _trackAngle = toDouble(splittetLine[8]);
    else
        _trackAngle = 0;
    
    //Date 
    if(splittetLine[9][0] != '\0')
    {
        std::stringstream ss5;
        ss5<<splittetLine[9][0]<<splittetLine[9][1];
        ss5>>no;
        _day = no;

        std::stringstream ss6;
        ss6<<splittetLine[9][2]<<splittetLine[9][3];
        ss6>>no;
        _month = no;

        std::stringstream ss7;
        ss7<<splittetLine[9][4]<<splittetLine[9][5];
        ss7>>no;
        _year = no;
    }
    else
    {
        _day = 0;
        _month = 0;
        _year = 0;
    }
    
    dataAvailable.send();
}

double NMEAParser::latitude() const
{
    return _latitude;
}

double NMEAParser::longitude() const
{
    return _longitude;
}

double NMEAParser::altitude() const
{
    return _altitude;
}

Pt::uint32_t NMEAParser::satellitesInUse() const
{
    return _satellitesInUse;
}

Pt::uint8_t NMEAParser::day() const
{
    return _day;
}

Pt::uint8_t NMEAParser::month() const
{
    return _month;
}
    
Pt::uint16_t NMEAParser::year() const
{
    return _year;
}

Pt::uint8_t NMEAParser::hour() const
{
    return _hour;
}
    
Pt::uint8_t NMEAParser::minute() const
{
    return _minute;
}

Pt::uint8_t NMEAParser::second() const
{
    return _second;
}

bool NMEAParser::status()const
{
    return _status;
}

double NMEAParser::groundSpeed() const
{
    return _groundSpeed;
}

double NMEAParser::trackAngle() const
{
    return _trackAngle;
}

NMEAParser::QualityOfMea NMEAParser::qualityOfMea() const
{
    return _qualityOfMea;
}

double NMEAParser::hdop() const
{
    return _hdop;
}
    
double NMEAParser::vdop() const
{
    return _vdop;
}

double NMEAParser::pdop() const
{
    return _pdop;
}

}}
