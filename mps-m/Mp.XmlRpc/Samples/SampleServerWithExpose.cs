namespace Sample
{
  using System;
  using System.Diagnostics;
  using Nwc.XmlRpc;

  ///<summary>
  /// Server that utilizes XmlRpcExposed attribute.
  ///</summery>
  ///Run the client.exe against server.exe and then expose.exe to see difference.
  [XmlRpcExposed]		// Indicate this class uses XmlRpcExposed to restrict method access
  class SampleServerWithExpose
  {
    const int PORT = 5050;

    /// <summary>Simple logging to Console.</summary>
    static public void WriteEntry(String msg, EventLogEntryType type)
      {
	if (type != EventLogEntryType.Information) // ignore debug msgs
	  Console.WriteLine("{0}: {1}", type, msg);
      }
    
    /// Application starts here.
    public static void Main() 
      {
	// Use the console logger above.
	Logger.Delegate = new Logger.LoggerDelegate(WriteEntry);

	XmlRpcServer server = new XmlRpcServer(PORT);
	server.Add("sample", new SampleServerWithExpose());
	Console.WriteLine("Web Server Running on port {0} ... Press ^C to Stop...", PORT);
	server.Start();
      }

    /// <summary>A method that returns the current time.</summary>
    [XmlRpcExposed]		// Make this method XML-RPC exposed
    public DateTime Ping()
      {
	return DateTime.Now;
      }

    /// <summary>A method that echos back it's arguement.</summary>
    /// !!!!!!! Note this is public BUT NOT XML-RPC exposed! !!!!!!!!!
    public String Echo(String arg)
      {
	return arg;
      }
  }
}
