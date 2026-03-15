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
#ifndef MPS_CORE_PORT_H
#define MPS_CORE_PORT_H

#include <Pt/Types.h>
#include <mps/core/Object.h>
#include <mps/core/Api.h>
#include <mps/core/Sources.h>
#include <map>

namespace mps{
namespace core{

class ProcessStation;
class SignalList;
class Signal;

/**@brief The base class for each process station port.
 *
 * This class implements the data processing functionality between
 * ports and process station.*/
class MPS_CORE_API Port : public Object
{
public:
    /** @brief Constructor.
    * @param type The type identifier of the port. */
    Port();
    
    /** @brief Destructor*/
    virtual ~Port(void);

    /** @brief Override this to setup the port.
    *
    *  The properties of the port are allready loaded.*/
    virtual void onInitInstance();

    /** @brief Override this to clear the port.*/
    virtual void onExitInstance();

    /** @brief Override this to process the data working.
    * 
    *  @param noOfRecords The number of records in the data stream.
    *  @param sourceIdx The source index.
    *  @param data The data stream.*/
    virtual void onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const Pt::uint8_t* data);

    /** @brief Sets the parent process station.
    *
    *   @param processStation The parent process station.*/
    void setParentPS( ProcessStation* processStation );

    /** @brief Define this port as input port.
    *
    *   @param input True by input port false by output port.*/
    void setInputPort(bool input);
    
    /** @brief Ask if this port is input port.
    *   @return True if is input port.*/
    bool isInputPort() const;

    bool isConnected() const;

    /** @brief Gets the port number.
    *   @return The port number.*/
    Pt::uint32_t portNumber() const;	

    /** @brief Sets the port number.
    *   @param number The port number.*/
    void setPortNumber( Pt::uint32_t number );

    /** @brief Gets the port identifier.
    *   @return The port identifier.*/
    const std::string& getName() const;

    /** @brief Sets the port identifier.
    *   @param name The port identifier.*/
    void setName( const std::string& name );

    /** @brief Connect this port to the given port.
    *   @param portId The port to connect.*/
    void connectToPort( Pt::uint32_t portId );

    /** @brief Gets the port signal list.
    *   @return  The port signal list.*/
    inline const SignalList* signalList() const 
    { 
        return  _signalList; 
    }

    /** @brief Sets the port signal list id.
    *   @param id The signal list id.*/
    void setSignalListID(Pt::uint32_t id);

    /** @brief Gets the port signal list id.
    *   @return The signal list id.*/
    Pt::uint32_t getSignalListID() const;
    
    /**@brief Return a reference of the source array*/
    const Sources& sources() const;				
        

    /**@brief Return the size of the data in the source in byte.
    *
    *  @param srcIdx The source index.
    *  @return The size in bytes
    */
    Pt::uint32_t sourceDataSize(Pt::uint32_t srcIdx) const;

    /**@brief Return the source index.
    *
    * @param sigIndexInList The signal index in the signal list.
    * @return The source index. */
    inline Pt::uint32_t sourceIndex(Pt::uint32_t sigIndexInList) const
    {
        return _sourceIndex[sigIndexInList];
    }

    /**@brief Return the signal offset in the source.
    *
    * @param[in] sigIndexInList The signal index in the signal list.
    * @return The byte offset of the signal in the source.
    */
    inline Pt::uint32_t signalOffsetInSource(Pt::uint32_t sigIndexInList) const
    {
        return _signalOffsetInSrc[sigIndexInList];
    }

    /**@brief Returns the signal index in the source.
    * @param sigIndexInList The signal index in the signal list..
    * @return The signal index in the source. */
    inline Pt::uint32_t signalIndexInSource(Pt::uint32_t sigIndexInList) const
    {
        return _signalIdxInSrc[sigIndexInList];
    }

    /**@brief Return the signal byte offset in the source.
    *
    * @param[in] srcIdx The source index.
    * @param[in] sigIdxInSrc The signal index in the source.
    * @return The signal byte offset in the source.
    */
    inline Pt::uint32_t signalOffsetInSource(Pt::uint32_t srcIdx, Pt::uint32_t sigIdxInSrc) const
    {
        return _signalOffsetInSrcByIdx[srcIdx][sigIdxInSrc];
    }

    Signal* getSignalByID(Pt::uint32_t id) const;

private:
    void createSources();
    static Pt::uint64_t signalSourceID(Pt::uint32_t source, double samplerate);
    static Pt::uint64_t signalSourceID(const Signal* signal);

    Pt::uint32_t dummy() const;
private:

    std::string	_name;
    std::vector<long> _connectedToPortsID;
    std::vector<Port*> _connectedToPorts;
    const SignalList* _signalList;
    bool _inputPort;
    ProcessStation* _processStation;
    Pt::uint32_t _portNumber;
    Pt::uint32_t _signalListID;		
    Sources _sources;
    std::vector<Pt::uint32_t> _sourceDataSize;
    std::vector<Pt::uint32_t> _signalOffsetInSrc;
    std::vector<std::vector<Pt::uint32_t> >  _signalOffsetInSrcByIdx;
    std::vector<Pt::uint32_t> _signalIdxInSrc;
    std::vector<Pt::uint32_t> _sourceIndex;
};

}}

#endif
