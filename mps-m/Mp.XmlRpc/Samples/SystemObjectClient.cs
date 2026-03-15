namespace Sample
{
  using System;
  using System.Collections;
  using Nwc.XmlRpc;

  class SystemObjectClient
  {
    const String SERVER_URL = "http://127.0.0.1:5050";

    /// Series of calls to the sample server.
    public static void Main() 
      {
	XmlRpcResponse response, response2;
	ArrayList methods = null;
	XmlRpcRequest client = new XmlRpcRequest();

	/*
	 * Get the list of methods.
	 */
	client.MethodName = "system.listMethods";
	client.Params.Clear();
	response = client.Send(SERVER_URL);
	if (response.IsFault)
	  {
	    Console.WriteLine("Fault {0}: {1}", response.FaultCode, response.FaultString);
	    return;
	  }
	else
	  {
	    methods = (ArrayList)response.Value;
	  }

	/*
	 * Iterate through the methods getting signatures and help.
	 */
	foreach (String method in methods)
	  {
	    client.MethodName = "system.methodSignature";
	    client.Params.Clear();
	    client.Params.Add(method);
	    response = client.Send(SERVER_URL);
	    if (response.IsFault)
	      Console.WriteLine("Fault {0}: {1}", response.FaultCode, response.FaultString);
	    else
	      {
		client.MethodName = "system.methodHelp";
		foreach (ArrayList signature in (ArrayList)response.Value)
		  {
		    Console.Write(signature[0]);
		    Console.Write(" " + method + "(");		
		    for (int x = 1; x < signature.Count; x++)
		      {
			if (x > 1)
			  Console.Write(", ");
			Console.Write(signature[x]);
		      }
		    Console.WriteLine(")");
		    response2 = client.Send(SERVER_URL);
		    if (response2.IsFault)
		      Console.WriteLine("Fault {0}: {1}", response.FaultCode, response.FaultString);
		    else
		      Console.WriteLine(response2.Value);
		  }
	      }
	  }
      }
  }
}
