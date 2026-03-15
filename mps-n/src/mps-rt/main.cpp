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
#include <Pt/Main.h>
#include <Pt/System/FileInfo.h>
#include <Pt/Net/Endpoint.h>
#include <Pt/Net/TcpSocket.h>
#include <fstream>
#include "MpsApplication.h"
#include <Pt/TextStream.h>
#include <Pt/Utf8Codec.h>
#include "MpsSettings.h"
#include <Pt/System/Logger.h>
#include "System.h"
#include <stdio.h>
#include <stdlib.h>
#include <memory.h>
#include <iostream>

//The NULL Buffer class
class Nullbuf : public std::streambuf 
{
public:
    virtual int overflow(int c) 
    { 
        return traits_type::not_eof(c); 
    } 
};

void startServer(const std::string& path, const mps::rt::MpsSettings& settings)
{
    std::cout<<std::endl;
    std::cout<< "MeaProcess - Runtime [Version 2.0 (BETA, BUILD: 20151028) ]"<<std::endl;
    std::cout<< "Copyright (C) 2010-2015 Atesion GmbH"<<std::endl;
    
    //Redirect the stderr to null device
    Nullbuf nullBuf;
    std::streambuf* orgBuf = std::cerr.rdbuf(&nullBuf);

    //Run the server.
    mps::rt::MpsApplication app;

    try
    {
        app.init(settings, path);
        app.run();
        app.close();
    }
    catch(const std::exception& ex)
    {
        std::cout<<ex.what()<<std::endl;
    }
    
    //Restore the original stderr.
    std::cerr.rdbuf(orgBuf);
}

void stopServer(int port)
{
    char terminate = 7;
    Pt::uint32_t endTrans = 0;

    try
    {
        Pt::Net::TcpSocket socket(Pt::Net::Endpoint::ip4Any(port));

        socket.write(&terminate, 1);
        socket.write((char*)&endTrans, sizeof(Pt::uint32_t));
        socket.close();
    }
    catch(const std::exception& ex)
    {
        std::cerr<<ex.what()<<std::endl;
    }
}

int main(int argc, char** args)
{	
    try
    {			
        //Determinate the startup path
        Pt::System::Path finfo(args[0]);
        std::string path = finfo.dirName().narrow();
    
        //Load the settings
        mps::rt::MpsSettings mpsSettings;

        //The settings file
        const std::string configFile = path + "mea.conf"; 
        
        //The settings deserialiser
        Pt::Settings settings;

        //The settings stream
        std::ifstream is(configFile.c_str());
        Pt::TextIStream tx(is, new Pt::Utf8Codec() );
        settings.load(tx);
        
        //Deserialize the object from the settings stream 
        mpsSettings.load(settings);
        is.close();

        //Check arguments
        switch(argc)
        {
            case 1:
            {
                startServer(path, mpsSettings);
                return 0;
            }
            break;

            case 2:
            {
                const std::string argument = args[1];
        
                if( argument == "stop")
                {
                    stopServer(mpsSettings.port());
                }
                else if( argument == "start")
                {
                    startServer(path, mpsSettings);
                }
                else if( argument == "restart")
                {
                    stopServer( mpsSettings.port());
                    Pt::System::Thread::sleep(200);
                    startServer(path, mpsSettings);
                }
                else
                {
                    std::cout<<"Options : stop = for stop a running runtime.";
                    std::cout<<"        : start = for start the runtime.";
                    std::cout<<"        : restart = for restart the runtime.";

                    return -1;			  
                }

                return 0;
            }
            break;
        }

        return 0;
    }
    catch(const std::exception& ex)
    {
        std::cout<<"MeaProcess runtime: Exception catched."<<std::endl;
        std::cout<<ex.what()<<std::endl;
        std::cout<<"MeaProcess runtime: Terminate with error."<<std::endl;
    }

    return -1;
}

