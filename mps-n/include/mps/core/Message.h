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
#ifndef MPS_CORE_MESSAGE_H
#define MPS_CORE_MESSAGE_H

#include <string>
#include <Pt/Types.h>
#include <Pt/DateTime.h>
#include <mps/core/Api.h>

namespace mps{
namespace core{

namespace MessageResult
{
    enum MsgResult
    {
        Yes,
        No,
    };
}

/**@brief Represents a message 
*
*  This class is deprecated.
*/
class MPS_CORE_API Message
{
public:
    
    /**@brief The message target*/
    enum MessageTarget
    {
        Output,  ///< Default output.
        Event,   ///< Show as event.
        Status,  ///< Show as status.
        Modal,   ///< Show modal.
        LogFile, ///< Write to log file. 
        Trace,   ///< Write to trace.
        File,     ///< Write to the specified file.
        System	 ///< Notification 
    };

    /**@brief The message type*/
    enum MessageType
    {
        Info = 0, /**< Information.*/
        Warning,  /**< Warning.*/
        Error,	  /**< Error. */
        Question, /**< Question. */
        Stop,	  /**< Runtime engine is stopped.*/
        EventMsg, /**< Event message.*/
        QuestionFile, /**< Question overwrite file.*/
    };

    /**@brief Default constructor.*/
    Message(void);

    /**@brief Construct a message object by the given data.
    *
    * @param text The Message text.
    * @param target The Output target of the message.
    * @param type The type of the message.
    * @param timeStamp The time stamp (msecs. since epoch)*/
    Message(const std::string& text, MessageTarget target, MessageType type, const Pt::DateTime& timeStamp); 
    
    /**@brief Destructor.*/
    virtual ~Message(void);
    
    /**@brief == operator.*/
    bool operator==(const Message& msg) const;

    /**@brief != operator.*/
    bool operator!=(const Message& msg);

    /**@brief Gets the message text.
    *
    * @return The message text*/
    inline const std::string& text() const
    {
        return _text;
    }

    /**@brief Sets the message text.
    *
    * @param t The message text*/
    inline void setText(const std::string& t)
    {
        _text = t;
    }

    /**@brief Gets the message comment.
    *
    * @return The message comment*/
    inline const std::string& comment() const
    {
        return _comment;
    }

    /**@brief Sets the message comment.
    *
    * @param c The message text*/
    inline void setComment(const std::string& c)
    {
        _comment = c;
    }

    /**@brief Gets the message output file name.
    *
    * @return The message comment*/
    inline const std::string& fileName() const
    {
        return _fileName;
    }

    /**@brief Sets the message output file name.
    *
    * @param f The message output file name*/
    inline void setFileName(const std::string& f)
    {
        _fileName = f;
    }

    /**@brief Gets the message target.
    *
    * @return The message target*/
    inline MessageTarget target() const
    {
        return _target;
    }

    /**@brief Sets the message target.
    *
    * @param c The message target*/
    inline void setTarget(MessageTarget t)
    {
        _target = t;
    }

    /**@brief Gets the message type.
    *
    * @return The message target*/
    inline MessageType type() const
    {
        return _type;
    }

    /**@brief Sets the message type.
    *
    * @param c The message type*/
    inline void setType(MessageType t)
    {
        _type = t;
    }


    /**@brief Gets the message type stamp in (msecs. since epoch).
    *
    * @return The message timr stamp*/
    inline Pt::int64_t timeStamp() const
    {
        return _timeStamp;
    }

    inline Pt::DateTime formatedTimeStamp() const
    {
        static const Pt::DateTime dt(1970, 1, 1);
        Pt::Timespan ts(_timeStamp*1000);
        return dt + ts;
    }

    /**@brief Sets the message time stamp in (msecs. since epoch).
    *
    * @param t The message time stamp*/
    inline void setTimeStamp(const Pt::DateTime& t)
    {
        static const Pt::DateTime dt(1970, 1, 1); //from epoch
        _timeStamp  = (t - dt).toMSecs();
    }

    /**@brief Gets the message error code.
    *
    * @return The message error code*/
    inline Pt::uint32_t errorCode() const
    {
        return _errorCode;
    }

    /**@brief Sets the message error code.
    *
    * @param e The message error code*/
    inline void setErrorCode(Pt::uint32_t e)
    {
        _errorCode = e;
    }

private:
    
    std::string		_text;
    std::string		_comment;
    std::string		_fileName;
    MessageTarget	_target;
    MessageType		_type;
    Pt::int64_t	    _timeStamp;
    Pt::uint32_t	_errorCode;
};

}}
#endif
