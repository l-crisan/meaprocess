'Unter x64 WINDOWS execute with "C:\Windows\Syswow64\wscript.exe  DemoScripting.vbs"
'Create a MeaProcess Runtime object
Set obMea =  WScript.CreateObject("MeaProcessApp.MeaProcess")

' Open a scheme, give the runtime a port for communication with this script object

obMea.OpenScheme "C:\Users\cr\Desktop\scripting\local\test.mpw", 4040

'Start the measurement
obMea.Start

'Read  some signal data out and show a message box
for j = 0 to 3
	for i = 0 to obMea.OutputSignals -1
		    Set signal = obMea.GetSignal(i, false)

		    WScript.echo  signal.Value
		    WScript.sleep 1000
	next
next


'Write a signal into the system
for j = 0 to 10
	for i = 0 to obMea.InputSignals -1
   		 Set signal = obMea.GetSignal(i, true)
   		 signal.Value = 1
    		 WScript.sleep 500
    		 signal.Value = 0
   		 WScript.sleep 500
	next
next

' Stop the measurement 
obMea.Stop

