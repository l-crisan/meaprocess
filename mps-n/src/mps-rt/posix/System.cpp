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
#include "System.h"
#include <signal.h>
#include <memory.h>
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <fcntl.h>
#include <sys/types.h>
#include <sys/mman.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <netdb.h> 
#include <arpa/inet.h>
#include <linux/if.h>
#include <unistd.h>
#include <sys/ioctl.h>
#include <iostream>
namespace mps{
namespace rt{
  
std::string System::_workDir = "/opt/atesion/mea/";

void socketWriteErrorSig(int sig)
{
}

void System::setError(bool on)
{
    std::string cmd = _workDir + "stateError.sh";

    if(on)
        cmd += " on";
    else
        cmd += " off";
    
    system(cmd.c_str());
}

void System::setReady(bool on)
{
    std::string cmd = _workDir + "stateReady.sh";

    if(on)
        cmd += " on";
    else
        cmd += " off";
    
    system(cmd.c_str());
}

void System::setRun(bool on)
{
    std::string cmd = _workDir + "stateRun.sh";

    
    if(on)
        cmd += " on";
    else
        cmd += " off";
    
    system(cmd.c_str());
}


Pt::uint64_t System::initSystem()
{
    //Ignore SIGPIPE
    struct sigaction newvec;
    memset(&newvec, 0, sizeof(newvec));
    newvec.sa_handler = SIG_IGN;
    sigaction(SIGINT, &newvec, 0);
    signal(SIGPIPE, socketWriteErrorSig) ;
    return 0;
}


void System::deinitSystem(Pt::uint64_t handle)
{
}


}}
