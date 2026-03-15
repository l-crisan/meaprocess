namespace Sample
{
  using System;
  using System.Collections;
  using System.Diagnostics;
  using Nwc.XmlRpc;

  class SampleClient
  {

    /// <summary>Simple logging to Console.</summary>
    static public void WriteEntry(String msg, EventLogEntryType type)
      {
	if (type != EventLogEntryType.Information) // ignore debug msgs
	  Console.WriteLine("{0}: {1}", type, msg);
      }

    /// Series of calls to the sample server.
    public static void Main() 
      {
	XmlRpcResponse response;

	// Use the console logger above.
	Logger.Delegate = new Logger.LoggerDelegate(WriteEntry);

	// Send the sample.Ping RPC
	XmlRpcRequest client = new XmlRpcRequest();
	client.MethodName = "sample.Ping";
	Console.WriteLine(client);
	response = client.Send("http://127.0.0.1:5050");
	if (response.IsFault)
	  Console.WriteLine("Fault {0}: {1}", response.FaultCode, response.FaultString);
	else
	  Console.WriteLine("Response: " + response.Value);

	// Send the sample.Echo RPC
	client.MethodName = "sample.Echo";
	client.Params.Clear();
	client.Params.Add("Hello");
	Console.WriteLine(client);
	response = client.Send("http://127.0.0.1:5050");
	if (response.IsFault)
	  Console.WriteLine("Fault {0}: {1}", response.FaultCode, response.FaultString);
	else
	  Console.WriteLine("Response: " + response.Value);

	// Send the sample.Broken RPC
	client.MethodName = "sample.Broken";
	client.Params.Clear();
	response = client.Send("http://127.0.0.1:5050");
	if (response.IsFault)
	  Console.WriteLine("Fault {0}: {1}", response.FaultCode, response.FaultString);
	else
	  Console.WriteLine("Response: " + response.Value);
      }
  }
}
