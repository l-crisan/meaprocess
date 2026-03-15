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
#include <iostream>
//#include <sstream>

#include <Pt/Http/Reply.h>

#include "HTTPResponder.h"

namespace mps {

    namespace rt {

        std::string fileExtension(const char* file)
        {
            const std::string&     fileName          = file;
            std::string::size_type extensionPointPos = fileName.rfind('.');

            if (extensionPointPos != std::string::npos)
                return fileName.substr(extensionPointPos + 1);
            else
                return "";
        }

        ////////////////////////////////////////////////////////////////////////////////

        HTTPResponder::HTTPResponder(Pt::Http::Service& service)
        : Pt::Http::Responder(service) ,_workingDir("")
        {}

        HTTPResponder::~HTTPResponder()
        {}

        void HTTPResponder::onBeginRequest(Pt::Http::Request& request, Pt::Http::Reply& reply, Pt::System::EventLoop& loop)
        {
            _mimeType = _getMimeType(fileExtension(request.url().c_str()), _binary);

            if( _binary) _stream.open((_workingDir + request.url()).c_str(), std::ios::binary);
            else         _stream.open((_workingDir + request.url()).c_str());

            if(!_stream.is_open()) {
                const std::string& err = "Request '" + request.url() + "' not found";
                std::cout << err << std::endl;
            }
        }
        
        void HTTPResponder::onReadRequest(Pt::Http::Request& request, Pt::Http::Reply& reply, Pt::System::EventLoop& loop)
        {}

        void HTTPResponder::onBeginReply(const Pt::Http::Request& request, Pt::Http::Reply& reply, Pt::System::EventLoop& loop)
        {
            if(!_stream.is_open()) {
                reply.beginSend();
                return;
            }

            try {
                reply.header().set("Content-Type", _mimeType.c_str());
                
                char ch;
                while(_stream.read(&ch, 1)) reply.body() << ch;
                //while(_stream.read(&ch, 1)) reply.body().write(&ch, 1);
            }
            catch(const std::exception& ex) {
                std::cout << "HTTP Request error: " << ex.what() << std::endl;
            }

            reply.beginSend();

            _stream.close();
        }

        void HTTPResponder::onWriteReply(const Pt::Http::Request& request, Pt::Http::Reply& reply, Pt::System::EventLoop& loop)
        {}

        std::string HTTPResponder::_getMimeType(const std::string& ext, bool& binary)
        {
            if(ext == "jpg" || ext == "jpeg") {
                binary = true;
                return "image/jpeg";
            }

            else if(ext == "png") {
                binary = true;
                return "image/png";
            }

            else if(ext == "js") {
                binary = false;
                return "text/javascript";
            }

            else if(ext == "css") {
                binary = false;
                return "text/css";
            }

            else if(ext == "html" || ext == "htm") {
                binary = false;
                return "text/html";
            }

            binary = false;
            return "text/plain";
        }

    } // namespace cs

} // namespace mps
