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

#include <sstream>
#include <fstream>
#include <mpal/vm/VirtualMachine.h>
#include <mpal/vm/Debugger.h>

using namespace mpal::vm;
using namespace std;

int main( int argc, const char* argv[] )
{
    cout<<"MPAL-Debugger [Version 1.0.0]"<<std::endl;
    cout<<"(C) Copyright 2010-2015 Atesion GmbH"<<std::endl;

    if( argc != 4)
    {
        cout<<"Usage: mpal-debugger <file> <host> <port>"<<endl<<endl;
        cout<<"                     <file> : The MPAL binary file."<<endl;
        cout<<"                     <host> : The host name or IP-address to listen."<<endl;
        cout<<"                     <port> : The port to listen."<<endl<<endl;
        cout<<"Example: mpal-debugger \"c:\\test.mpp\" \"127.0.0.1\" 5050"<<endl;
        return -1;
    }

    try
    {
            stringstream ss;
            int port = 0;

            ss<<argv[3];
            ss>>port;
            VirtualMachine vm;

            Debugger debugger(vm);
            debugger.setup(argv[2], port);

            fstream fdata;
            std::string mppFile = argv[1];
        
            fdata.open(mppFile.c_str(), ios_base::in|ios_base::binary);
        
            if( !fdata )
            {
                std::cout<<" The file '"<<mppFile<<"' couldn't be loaded."<<std::endl;
                return -2;
            }

      debugger.load(fdata);
      fdata.close();
      debugger.waitEnd();
      debugger.close();
    }
    catch( const exception& ex )
    {
      cout<<ex.what();
            return -3; 
    }
    
    return 0;
}

