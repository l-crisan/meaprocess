

function meaAppGetSignalValue(index, control)
{
	var value = 0;
	
	try
	{
		var params = new Array();
		params[0] = index;
	
		
	    xmlrpc("/MeaProcess", "GetSignalValue", params,

        function (ret) 
        {
        	control.value = ret;
        }
        ,
        function (err) {
        },

        function () {
        }
        );
	}
	catch(ex)
	{
	    control.value = error;
	}
}

function meaAppSetSignalValue(index, value)
{
	try
	{
		var params = new Array();
		params[0] = index;
		params[1] = value;
	
		
	    xmlrpc("/MeaProcess", "SetSignalValue", params,

        function (ret) 
        {
        }
        ,
        function (err) {
        },

        function () {
        }
        );
	}
	catch(ex)
	{
	}
}

function meaAppOutputSignal(value)
{
	meaAppSetSignalValue(0,value);
}

function meaAppGetSignalData() 
{
	try
	{
	    xmlrpc("/MeaProcess", "GetNoOfOutputSignals", null,

        function (ret) 
        {
			for( var i = 0; i < ret ; ++i)
			{			
				switch( i)
				{
					case 0:
						sigValueCtrl = document.forms["measurementForm"].elements["signal1"];
						meaAppGetSignalValue(i, sigValueCtrl);				
					break;
					
					case 1:
						sigValueCtrl = document.forms["measurementForm"].elements["signal2"];
						meaAppGetSignalValue(i, sigValueCtrl);						
					break;
					
					case 2:
						sigValueCtrl = document.forms["measurementForm"].elements["signal3"];
						meaAppGetSignalValue(i, sigValueCtrl);
					break;
				}				
				
			}
        }
        ,
        function (err) 
        {
        },

        function () {
        }
        );
        
       }catch(ex)
       {
       }
}

function readLocalFile(fname) {

    var httpRequest = new Object();

    try 
    {
        httpRequest = new ActiveXObject("MSXML2.XMLHTTP.3.0");
        
    }
    catch (err) 
    {
        httpRequest = new XMLHttpRequest();        
    }    

    fname = fname.replace(/\//gi, "");
    fname = fname.replace(/%20/gi, " ");
    fname = fname.replace(/\\/gi, "\/");
    var url = "file://" + fname;    
    httpRequest.open("GET", url, false);    
    httpRequest.send(null);
    return httpRequest.responseText;
}


function meaAppGetStatus() {

    var statusCtrl = document.forms["statusForm"].elements["statusText"];
    var schemaIDCtrl = document.forms["statusForm"].elements["schemaText"];


    xmlrpc("/MeaProcess", "GetStatus", null,

        function (ret) 
        {
            switch(ret)
            {
            	case 0:
            		statusCtrl.value = "Ready";
            	break;

            	case 1:
					statusCtrl.value = "Schema loaded";
				break;
				
				case 2:
					statusCtrl.value = "Initialized";
				break;

				case 3:
					statusCtrl.value = "Running";
				break;						            	
            }
        }
        ,
        function (err) {
            alert("MeaProcess request error occurred: " + err);
        },

        function () {
        }
        );



    xmlrpc("/MeaProcess", "GetSchema", null,

    function (ret) {
            schemaIDCtrl.value = ret;
    }
    ,
    function (err) {
        alert("MeaProcess request error occurred: " + err);
    },

    function () {
    }
    );
        
}

function meaAppDeploy() 
{
    try {
    
		try
		{
			netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
		}
		catch(exc)
		{
			alert(exc);	
		}
    

        var fileNameCtrl = document.forms["deployForm"].elements["schemaFile"];
        var schemaData = readLocalFile(fileNameCtrl.value);
        var filename = fileNameCtrl.value.replace(/^.*[\\\/]/, '');
        var date = new Date();
        var ticks = date.getTime();
        
        var parameter = new Array(filename, ticks, schemaData);
        alert(parameter);

        xmlrpc("/MeaProcess", "LoadScheme",  parameter,
        
        function (ret) {
            meaAppGetStatus();
        }        
        ,
        function (err) 
        {
        alert("MeaProcess request error occurred: " + err);
        },

        function () 
        {
        }
        );

    }
    catch(ex) 
    {
        alert(ex);
    }
}


function meaAppInitialize() {

    xmlrpc("/MeaProcess", "Reinitialize", null,

        function (ret) {
            meaAppGetStatus();
        }
        ,
        function (err) {
            alert("MeaProcess request error occurred: " + err);
        },

        function () {
        }
        );
    }


function meaAppStart() {

    xmlrpc("/MeaProcess", "Start", null,

    function (ret) {
        meaAppGetStatus();
    }
    ,
    function (err) {
        alert("MeaProcess request error occurred: " + err);
    },

    function () {
    }
    );

}

function meaAppStop() {

    xmlrpc("/MeaProcess", "Stop", null,

    function (ret) {
        meaAppGetStatus();
    }
    ,
    function (err) {
        alert("MeaProcess request error occurred: " + err);
    },

    function () {
    }
    );

}
