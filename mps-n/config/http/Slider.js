/* 
 * Slider.js
 * Schieberegler fuer HTML-Seiten
			
 * Synopsis:			          hoehe   maxWert   minStep    hookFunktion 
  mySlider=new Slider('test', 300, 20,  1,  10,   5,   1,   5,  'myFunct()');
                        id   breite   minWert  initWert   bigStep
 * akt = mySlider.wert (oder akt = Slider.instanz['test'].wert);
 * Slider.instanz['test'].neuerWert(15);
 * Slider.instanz['test'].neuerBereich(-5,      5,      2);
 									   minWert	maxWert neuerWert
 * Werden Parameter weggelassen oder statt dessen NaN uebergeben, werden
 * Default-Werte genommen (new Slider), bzw. bestehende Werte nicht geaendert 
 * (neuer...())
 * Auf das Aussehen kann mit den Stylesheet Klassen sliderBg (Hintergrund) und
 * sliderFg, sliderFgHov, sliderFgAct, sliderBtn Einfluss genommen werden
 * (ausgenommen Rahmenart) 
 */

function Slider (id, breite, hoehe, min, max, wert, schritt, grossSchritt, hook)
{
	//Parameter speichern
	this.id=id;
	this.hoehe=isNaN(hoehe) ? 15 : parseInt(hoehe);
	this.breite=isNaN(breite) ? 350 : parseInt(breite-=hoehe*2);
	this.min=isNaN(min) ? 1 : parseInt(min);
	this.max=isNaN(max) ? 10 : parseInt(max);
	this.wert=isNaN(wert) ? this.min : parseInt(wert);
	this.schritt=isNaN(schritt) ? 1 : parseInt(schritt);
	this.grossSchritt=isNaN(grossSchritt) ? 5 : parseInt(grossSchritt);
	this.hook=hook;

	//Weitere Variablen
	var ie = navigator.appName=='Microsoft Internet Explorer';
	this.schieben=-1	//Abstand zum Rand des Knopfs beim Schieben (-1=inaktiv)
	this.imHook=0		//Verhindert rekursives Aufrufen der Hooks
	Slider.instanz[id]=this;

	//HTML Elemente erzeugen
	document.write("<div style='width:" + (breite+hoehe*2+4) + "'>");
	document.write("<input type='button' value='&lt;' class='sliderBtn' " +
		"style='width:" + hoehe + "; height:" + (hoehe + (ie ? 0 : 2)) +
		";float:left' " +
		"onClick=\"Slider.instanz['" + id + "'].schritt(-1)\">");
	document.write("<input type='button' value='&gt;' class='sliderBtn' " +
		"style='width:" + hoehe + "; height:" + (hoehe + (ie ? 0 : 2)) + 
		";float:left;position:relative;left:" + breite + ";z-order:5' " +
		"onClick=\"Slider.instanz['" + id + "'].schritt(1)\">");
	document.write("<div id='" + id + "_bg' class='sliderBg'>");
	document.write("<div id='" + id + "_fg' class='sliderFg' " +
		"onMouseup=\"Slider.instanz['" + id + "'].onMouseup()\" " +
		"onMouseover=\"Slider.instanz['" + id + "'].onMouseover()\" " +
		"></div></div></div>");

	//Den Elementen weitere Eigenschaften zuweisen
	this.fg=document.getElementById(id + '_fg');
	this.bg=document.getElementById(id + '_bg');
	this.fg.style.position='relative';
	this.bg.style.position='relative';
	this.fg.style.zIndex='2';
	this.bg.style.zIndex='1';
	this.fg.style.borderStyle='outset';
	this.bg.style.borderStyle='inset';
	this.fg.style.borderWidth='1px';
	this.bg.style.borderWidth='1px';
	this.fg.style.fontSize='4px';
	this.bg.style.fontSize='4px';
	this.bg.style.left=ie ? -hoehe-3 : hoehe;

	//Beim Ueberfahren mit der Maus Hover darstellen
	this.onMouseover=function()
	{
		if(this.schieben<0) this.fg.className='sliderFgHov';
	}
	//Beim Verlassen der Maus ggf. normales Aussehen
	this.onMouseout=function(erg, id)
	{
		var s=Slider.instanz[id?id:this.id.substr(0, this.id.length-3)];
		if (s.schieben<0) s.fg.className='sliderFg';
	}
	//Beim Mausklick Verschieben starten
	this.onMousedown=function(erg)
	{
		if(!erg) erg=window.event;
		var s=Slider.instanz[this.id.substr(0, this.id.length-3)];
		s.fg.style.borderStyle='solid';
		s.fg.className='sliderFgAct';
		s.schieben=erg.clientX-parseInt(s.fg.style.left)-s.links;
		Slider.aktiv=this.id.substr(0, this.id.length-3);
	}
	//Beim Loslassen Schieben beenden
	this.onMouseup=function()
	{
		this.fg.style.borderStyle='outset';
		this.schieben=-1;
		Slider.aktiv="";
		this.onMouseout('',this.id);
	}
	//Beim Schieben Wert aktualisieren
	this.onMousemove=function(erg, id)
	{
		if(!erg) erg=window.event;
		var s=Slider.instanz[id?id:this.id.substr(0, this.id.length-3)];
		if(s.schieben<0) return;

		s.wert = s.max!=s.min ? 
			((erg.clientX - s.schieben - s.links - 1) /
			(s.breite - s.aspekt - 2) *
			(s.max - s.min) + s.min).toFixed(0)
			:
			s.min;
		s.wert=parseInt(s.wert);
		s.zeichnen();
		if(s.hook) eval(s.hook);
	}
	//Beim Klick auf den Hintergrund
	this.onClick = function(erg)
	{
		if(this.schieben>=0) return;
		var s=Slider.instanz[this.id.substr(0, this.id.length-3)];
		
		if(!erg) erg=window.event;
		
		var x=erg.clientX-s.links;
		if(x<parseInt(s.fg.style.left))
			s.wert -= s.grossSchritt
		else if (x>parseInt(s.fg.style.left)+s.aspekt)
			s.wert+=grossSchritt;
		s.zeichnen();
		if(s.hook) eval(s.hook);
	}
	//Wert relativ ändern
	this.schritt = function(wert)
	{
		this.wert+=wert;
		this.zeichnen();
		if(this.hook) eval(this.hook);
	}
	//HTML Elemente positionieren/skalieren
	this.init = function()
	{
		var obj=this.bg, x=0, y=0;
		while(obj.tagName!="BODY") {
			x+=obj.offsetLeft;
			y+=obj.offsetTop;
			obj=obj.offsetParent;
		}
		this.links=x;
		this.oben=y;

		this.bg.style.width=this.breite;
		this.bg.style.minWidth=this.breite;
		this.bg.style.height=this.hoehe;
		this.fg.style.height=(this.hoehe-2);
		this.aspekt=this.breite/(this.max-this.min+1)
		if(this.aspekt < 10) this.aspekt=10;
		this.fg.style.width=this.aspekt;
		this.fg.style.top=0;
		this.zeichnen();
	}
	//Slider-Knopf positionieren
	this.zeichnen = function()
	{
		if(this.wert < this.min) this.wert=this.min;
		if(this.wert > this.max) this.wert=this.max;
		if(this.min==this.max) {this.fg.style.left=0; return;}
		this.fg.style.left=(
			(
			 ((this.wert - this.min) / (this.max-this.min)) *
			 (this.breite-this.aspekt-2)
			)
		);
	}
	//Neuen Wert absolut setzen
	this.neuerWert=function(wert)
	{
		if(!isNaN(wert)) this.wert=parseInt(wert);
		this.zeichnen();
		this.imHook=1;
		if(this.hook) eval(this.hook);
		this.imHook=0;
	}
	//Grenzen des Sliders neu setzen
	this.neuerBereich=function(min, max, wert)
	{
		if (!isNaN(min)) this.min=parseInt(min);
		if (!isNaN(max)) this.max=parseInt(max);
		if (!isNaN(wert)) this.wert=parseInt(wert);
		this.init();
		if(this.imHook) return;
		this.imHook=1;
		if(this.hook) eval(this.hook);
		this.imHook=0;
	}

	//Ereignisse registrieren
	this.init();
	this.fg.onmousemove=this.onMousemove;
	this.fg.onmousedown=this.onMousedown;
	this.fg.onmouseout=this.onMouseout;
	this.bg.onmousemove=this.onMousemove;
	this.bg.onmousedown=this.onClick;
	var obj=this.bg;
	while(obj.tagName!="BODY") {
		obj=obj.offsetParent;
	}
	//Ereignisse ausserhalb des Sliders registrieren
	obj.onmouseup=Slider.schiebeStopp;
	obj.onmousemove=Slider.schieben;
}
//Ereignis von Ausserhalb: Maus losgelassen
Slider.schiebeStopp=function()
{
	if(Slider.aktiv) Slider.instanz[Slider.aktiv].onMouseup();
}
//Ereignis von Ausserhalb: Maus bewegt
Slider.schieben=function(erg)
{
	if(Slider.aktiv) Slider.instanz[Slider.aktiv].onMousemove(erg,Slider.aktiv);
}
//Array fuer Instanzen
Slider.instanz = new Object();
