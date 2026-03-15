//    MeaProcess - Meaurement and Automation framework.
//    Copyright (C) 2015  Laurentiu-Gheorghe Crisan
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;
using System.Globalization;
using Mp.Visual.HTML;
using Mp.Utils;

namespace Mp.Runtime.Sdk
{
    internal class TextViewPS : ProcessStation
    {
        private TextView _viewCtrl;        
        private double[] _values;
        private Timer _update = new Timer();
        private Event _onStartEvent = null;
        private Hashtable _signals2Event = new Hashtable();

        private class Event
        {
            public enum Operation
            {
                NE,
                EQ,
                LS,
                LE,
                GR,
                GE
            }
            public double Limit;
            public Operation Op;
            public string Text;
            public Signal Signal;
            public int SignalIndex = -1;
            public bool Active;
        }

        

        public TextViewPS()
        {
            _update.Interval = 500;
            _update.Tick += new EventHandler(OnUpdate);

        }


        private void OnUpdate(object sender, EventArgs e)
        {
            bool trigered = false;

            foreach (DictionaryEntry entry in _signals2Event)
            {
                List<Event> events = (List<Event>)entry.Value;
                foreach (Event ev in events)
                {
                    if (ev.SignalIndex == -1)
                        continue;

                    if (ev.Active)
                    {
                        trigered = true;
                        _viewCtrl.CurrentText = GetText(ev.Text);
                        break;
                    }
                }
            }

            if (trigered == false && _onStartEvent != null)
            {
                if(_onStartEvent.Text != "")
                    _viewCtrl.CurrentText = GetText(_onStartEvent.Text);
            }
        }

        
        private string ReplaceProperties(string args)
        {
            bool propBegin = false;
            int lbreakClose = 0;
            Point position = new Point();
            string prop = "";

            for(int i = 0; i < args.Length; ++i)
            {
                if (args[i] == '$' && i < (args.Length - 1))                    
                {
                    if (args[i + 1] == '(')
                    {
                        position.X = i;
                        propBegin = true;
                        ++i;
                        lbreakClose++;
                        continue;
                    }
                }

                if (propBegin)
                    prop += args[i];

                if (args[i] == '(' && propBegin)
                    lbreakClose++;

                if (args[i] == ')' && propBegin)
                {
                    lbreakClose--;
                    if (lbreakClose == 0)
                    {
                        position.Y = i;                
                        string propValue = GetPropertyValueFromKey(prop.Remove(prop.Length -1));

                        string part1 = args.Substring(0, position.X);
                        string part2 = args.Substring(position.Y + 1);

				        args = part1 +  propValue + part2;
                        i = position.X + propValue.Length -1;
                        prop = "";
                        propBegin = false;
                    }
                }
            }

            if (propBegin)
            {
                position.Y = args.Length - 1;
        
                string propValue = GetPropertyValueFromKey(prop.Remove(prop.Length - 1));
                
                string part1 = args.Substring(0, position.X);
                string part2 = args.Substring(position.Y + 1);

                args = part1 + propValue + part2;
            }

            return args;
        }


        private string GetPropertyValueFromKey(string prop)
        {
            RuntimeEngine runtime = RuntimeEngine.Instance();
            return runtime.GetPropertyValue(prop);
        }


        private List<string> GetTemplateProperties(string template)
        {
            List<string> properties = new List<string>();

            bool propBegin = false;
            string prop = "";
            int lbreakClose = 0;

            for(int i = 0; i < template.Length; ++i)
            {
                if (template[i] == '$' && i < (template.Length - 1))                    
                {
                    if (template[i + 1] == '(')
                    {
                        propBegin = true;
                        ++i;
                        lbreakClose++;
                        continue;
                    }
                }

                if (propBegin)
                    prop += template[i];

                if (template[i] == '(' && propBegin)
                    lbreakClose++;

                if (template[i] == ')' && propBegin)
                {
                    lbreakClose--;
                    if (lbreakClose == 0)
                    {
                        properties.Add(prop.TrimEnd(')'));
                        prop = "";
                        propBegin = false;
                    }
                }
            }

            if (propBegin)
                properties.Add(prop);

            return properties;
        }


        private string GetText(string template)
        {
            string retVal = template;

            try
            {
                SignalList signals = base.GetPortSignals(0);
                bool propBegin = false;
                string signalText = "";
                int beginPos = -1;
                int lbreakClose = 0;

                for (int i = 0; i < retVal.Length; ++i)
                {
                    if (retVal[i] == '§' && i < (retVal.Length - 1))                    
                    {
                        if (retVal[i + 1] == '(')
                        {
                            beginPos = i;
                            propBegin = true;
                            ++i;
                            lbreakClose++;
                            continue;
                        }
                    }

                    if (propBegin)
                        signalText += retVal[i];

                    if (retVal[i] == '(' && propBegin)
                        lbreakClose++;

                    if (retVal[i] == ')' && propBegin)
                    {
                        lbreakClose--;
                        if (lbreakClose == 0)
                        {
                            signalText = signalText.TrimEnd(')');
                            string[] arraySigData = signalText.Split(',');

                            int signalIndex = Convert.ToInt32(arraySigData[0]);

                            if (signalIndex < 0 || signalIndex >= signals.Count)
                                throw new Exception("Wrong Signal");

                            Signal signal = signals[signalIndex];

                            switch (arraySigData[1].ToUpper())
                            {
                                case "VALUE":
                                {
                                    retVal = retVal.Remove(beginPos, i - beginPos + 1);
                                    string value = "";

                                    if (arraySigData.Length == 3)
                                    {
                                        value = _values[signalIndex].ToString("F" + arraySigData[2]);
                                    }
                                    else
                                    {
                                        value = Convert.ToString(_values[signalIndex]);
                                    }
                                    retVal = retVal.Insert(beginPos, value);
                                    i = beginPos + value.Length - 1;
                                }
                                break;
                                case "NAME":
                                {
                                    retVal = retVal.Remove(beginPos, i - beginPos + 1);
                                    retVal = retVal.Insert(beginPos, signal.Name);
                                    i = beginPos + signal.Name.Length - 1;
                                }
                                break;
                                case "COMMENT":
                                {
                                    retVal = retVal.Remove(beginPos, i - beginPos + 1);
                                    retVal = retVal.Insert(beginPos, signal.Comment);
                                    i = beginPos + signal.Comment.Length - 1;
                                }
                                break;
                                case "UNIT":
                                {
                                    retVal = retVal.Remove(beginPos, i - beginPos + 1);
                                    retVal = retVal.Insert(beginPos, signal.Unit);
                                    i = beginPos + signal.Unit.Length - 1;
                                }
                                break;
                                case "SAMPLERATE":
                                {
                                    retVal = retVal.Remove(beginPos, i - beginPos + 1);
                                    string value = signal.SampleRate.ToString("F0");
                                    retVal = retVal.Insert(beginPos, value);
                                    i = beginPos + value.Length - 1;
                                }
                                break;
                                case "MINIMUM":
                                {
                                    retVal = retVal.Remove(beginPos, i - beginPos + 1);
                                    string value = "";

                                    if (arraySigData.Length == 3)
                                    {
                                        value = signal.Minimum.ToString("F" + arraySigData[2]);
                                    }
                                    else
                                    {
                                        value = Convert.ToString(signal.Minimum);
                                    }
                                    retVal = retVal.Insert(beginPos, value);
                                    i = beginPos + value.Length - 1;
                                }
                                break;
                                case "MAXIMUM":
                                {
                                    retVal = retVal.Remove(beginPos, i - beginPos + 1);
                                    string value = "";

                                    if (arraySigData.Length == 3)
                                    {
                                        value = signal.Maximum.ToString("F" + arraySigData[2]);
                                    }
                                    else
                                    {
                                        value = Convert.ToString(signal.Maximum);
                                    }
                                    retVal = retVal.Insert(beginPos, value);
                                    i = beginPos + value.Length - 1;
                                }
                                break;
                            }
                            beginPos = -1;
                            signalText = "";
                            propBegin = false;
                        }
                    }
                }


                //Replace the properties
                retVal = ReplaceProperties(retVal);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return retVal;
        }

        public override void OnStart()
        {
            base.OnStart();
            _update.Start();            
        }

        public override void OnStop()
        {
            _update.Stop();
            base.OnStop();
        }

        protected override void OnUpdateDataValue(byte[] data, int sourceIdx, int portIdx, int records)
        {
            List<Signal> source = GetSource(0, sourceIdx);

            int recSize = GetSourceSize(0, sourceIdx);

            for (int rec = 0; rec < records; ++rec)
            {
                double value;
                Signal signal;
                for (int sigIdxInSrc = 0; sigIdxInSrc < source.Count; ++sigIdxInSrc)
                {
                    signal = source[sigIdxInSrc];
                    value = ExtractValue(data, 0, sigIdxInSrc, sourceIdx, rec * recSize, true);
                    _values[signal.SignalIndex] = value;

                    if (_signals2Event.Contains(signal.SignalID))
                    {
                        List<Event> events = (List<Event>) _signals2Event[signal.SignalID];
                        foreach(Event ev in events)
                        {
                            switch (ev.Op)
                            {
                                case Event.Operation.NE:
                                    ev.Active = ev.Limit != _values[ev.SignalIndex];
                                    break;
                                case Event.Operation.EQ:
                                    ev.Active = ev.Limit == _values[ev.SignalIndex];
                                    break;
                                case Event.Operation.GE:
                                    ev.Active = _values[ev.SignalIndex] >= ev.Limit;
                                    break;
                                case Event.Operation.GR:
                                    ev.Active = _values[ev.SignalIndex] > ev.Limit;
                                    break;
                                case Event.Operation.LE:
                                    ev.Active = _values[ev.SignalIndex] <= ev.Limit;
                                    break;
                                case Event.Operation.LS:
                                    ev.Active = _values[ev.SignalIndex] < ev.Limit;
                                    break;
                            }
                        }
                    }
                }
            }
        }


        protected override void OnSetupTheControls()
        {
            _viewCtrl = (TextView) Controls[0];

            string data = XmlHelper.GetParam(XmlRep, "events");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);

            NumberFormatInfo info = new NumberFormatInfo();
            info.NumberDecimalSeparator = ".";

            foreach (XmlElement xmlEvent in xmlDoc.DocumentElement.ChildNodes)
            {
                Event ev = new Event();
                ev.Op = (Event.Operation)Convert.ToInt32(xmlEvent["operation"].InnerText);
                ev.Limit = Convert.ToDouble(xmlEvent["limit"].InnerText,info);
                ev.Text = xmlEvent["text"].InnerText;
                uint id = Convert.ToUInt32(xmlEvent["signal"].InnerText);
                ev.Signal = GetSignalByID(id);

                if (ev.Signal != null)
                    ev.SignalIndex = ev.Signal.SignalIndex;
                else
                    _onStartEvent = ev;

                if (!_signals2Event.Contains(id))
                {
                    List<Event> events = new List<Event>();
                    events.Add(ev);

                    _signals2Event[id] = events;
                }
                else
                {
                    List<Event> events = (List<Event>) _signals2Event[id];
                    events.Add(ev);
                }
            }

            _values = new double[base.GetPortSignals(0).Count];
        }


        private Signal GetSignalByID(uint id)
        {
            SignalList signals = base.GetPortSignals(0);

            foreach (Signal sig in signals)
            {
                if (sig.SignalID == id)
                    return sig;
            }

            return null;
        }
    }
}
