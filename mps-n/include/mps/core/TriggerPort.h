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
#ifndef MPS_CORE_TRIGGERPORT_H
#define MPS_CORE_TRIGGERPORT_H

#include <mps/core/Port.h>
#include <Pt/Signal.h>

namespace mps{
namespace core {

/**@brief The class implements a trigger imut port.*/
class MPS_CORE_API TriggerPort : public Port
{
public:

	enum TriggerType
	{
		NoTrigger = 0, ///< The trigger type is not set
		StartTrigger,  ///< Start trigger
		StopTrigger,   ///< Stop trigger
  		StartStopTrigger, ///< Start/Stop trigger
		EventTrigger ///< Event trigger
	};

	/**@brief Default Constructor */
	TriggerPort( void );
	
	/**@brief Destructor */
	virtual ~TriggerPort( void );
	
    void onInitInstance();
    void onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const Pt::uint8_t* data);
	
	/**@brief Gets the trigger type.
	*
	* @return The trigger type.
	*/
	Pt::uint8_t triggerType() const;

	/**@brief Sets the trigger type.
	*
	* @param type The trigger type.
	*/
	void setTriggerType( Pt::uint8_t type);

	/**@brief Gets the pre time in seconds.
	*
	* @return The time in seconds.
	*/
	double preEventTime() const;

	/**@brief Sets the pre time in seoconds
	*
	* @param time The time in seconds.
	*/
	void setPreEventType(double time );	

	/**@brief Gets the post time in seconds.
	*
	* @return The time in seconds.
	*/
	double postEventTime() const;

	/**@brief Sets the post time in seoconds
	*
	* @param time The time in seconds.
	*/
	void setPostEventType(double time );	
	
	/**@brief Flag for one signal Start/Stop trigger.
	*
	* @return True if is an one signal Start/Stop trigger.
	*/	
	bool isOneStartStopSignal() const;

	/**@brief Sets the flag for one signal Start/Stop trigger.
	*
	* @param one The flag.
	*/		
	void setOneStartStopSignal(bool one );	

	/**@brief On start trigger signal. */
    Pt::Signal<> onStartTrigger;
	
	/**@brief On stop trigger signal. */
    Pt::Signal<> onStopTrigger;

	/**@brief On event trigger signal. */
    Pt::Signal<> onEventTrigger;
 
	/**@brief Starts the trigger port. */
    void start()
    { _storing = false; }
	
private:
	TriggerType		_triggerType;
	double			_preEventTime;
	double			_postEventTime;
	bool			_oneStartStopSignal;
    

    Pt::uint8_t	 _compBuffer[8];	
    bool _storing;
    mps::core::Signal*          _triggerSignal;
   	Pt::uint32_t						_triggerSignalPos;
    Pt::uint32_t						_triggerOffsetInSrc;	
	Pt::uint32_t						_triggerSource;

    mps::core::Signal*          _stopSignal;
    Pt::uint32_t						_stopSignalPos;
	Pt::uint32_t						_stopSource;	
	Pt::uint32_t						_stopOffsetInSrc;
};

}}

#endif
