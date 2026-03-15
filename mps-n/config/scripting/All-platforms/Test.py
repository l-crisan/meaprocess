import xmlrpc.client

# The function set available over scripting interface

# int GetStatus()
# --------------------------------------------------------------------------
# return the Status :
# Setup = 0,        ///< Runtime is setup
# Loaded = 1,       ///< Runtime is loaded
# Initialised = 2,  ///< Runtime is initialized
# Running = 3       ///< Runtime is running	


# string GetSchema()
# --------------------------------------------------------------------------
# Returns the schame ID sets by teh user

# bool LoadSchema(string schemeID, string timeStamp, string xmlSchemeString)
# --------------------------------------------------------------------------
# string schemeID : a user defined schema ID
# string timeStamp : a user defined timestamp
# string xmlSchemeString : the scheme coded into an XMl String, this
# can be generated with the MeaProcess-Deploy dialog.

# void Stop()
# --------------------------------------------------------------------------

# void Start()
# --------------------------------------------------------------------------

# bool Reinitialize()
# --------------------------------------------------------------------------

# string GetMessages()
# --------------------------------------------------------------------------

# int GetNoOfInputSignals()
# --------------------------------------------------------------------------
# Gets the number of input signals defined by the schema

# int GetNoOfoutputSignals()
# --------------------------------------------------------------------------
# Gets the number of output signals defined by the schema

# string GetSignalName(int index, bool inputSignal)
# --------------------------------------------------------------------------
# Gets the signal name
# int index : The signal index
# bool inputSignal : true => The signal is into the input list.

# string GetSignalComment(int index, bool inputSignal)
# --------------------------------------------------------------------------
# Gets the signal comment
# int index : The signal index
# bool inputSignal : true => The signal is into the input list.

# string GetSignalUnit(int index, bool inputSignal)
# --------------------------------------------------------------------------
# Gets the signal unit
# int index : The signal index
# bool inputSignal : true => The signal is into the input list.

# double GetSignalValue(int index)
# --------------------------------------------------------------------------
# Gets the signal value from the input list
# int index : The signal index


# void SetSignalValue(int index, double value)
# --------------------------------------------------------------------------
# Sets the signal value from the output list
# int index : The signal index
# double value : Teh signal value

# double GetSignalMin(int index, bool inputSignal)
# --------------------------------------------------------------------------
# Gets the signal minimumn
# int index : The signal index
# bool inputSignal : true => The signal is into the input list.

# double GetSignalMax(int index, bool inputSignal)
# --------------------------------------------------------------------------
# Gets the signal maximum
# int index : The signal index
# bool inputSignal : true => The signal is into the input list.


#The port is configured in MCM in to the file "mea.conf"
objMea = xmlrpc.client.ServerProxy("http://192.168.1.99:808/MeaProcess", allow_none=True)

#Start the schema
objMea.Start()

#Do something usefull
print(objMea.GetNoOfOutputSignals())
signalName = objMea.GetSignalName(0, 0)
print(signalName)
signalValue = objMea.GetSignalValue(0)
print(signalValue)

#Stop the schema
objMea.Stop()
