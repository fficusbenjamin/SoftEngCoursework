﻿using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;

namespace BusinessLayer
{
    class Sms : Message
    {
        private readonly string _messageType;
        private string _messageID, _sender, _subject,_body;

        public Sms(string messageID, string sender, string subject, string body)
        {
            _messageType = "SMS";
            _messageID = messageID;
            _sender = sender;
            _subject = null;
            _body = body;
        }
        

        public override string MessageType
        {
            get { return _messageType; }
        }
        public override string ID
        {
            get { return _messageID; }
            set { _messageID = value; }
        }
        public override string Sender
        {
            get { return _sender; }
            set 
            {
                valSender(value);
                _sender = value; 
            }

            
        }
       
        
        public override string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }
        public override string Body
        {
            get { return _body; }
            set 
            {
                //valBd(value);
                _body = valBd(value); 
            }
        }


        public string valSender(string val)
        {
            System.Text.RegularExpressions.Regex rNmbr = new System.Text.RegularExpressions.Regex(@"(([+][(]?[0-9]{1,3}[)]?)|([(]?[0-9]{4}[)]?))\s*[)]?[-\s\.]?[(]?[0-9]{1,3}[)]?([-\s\.]?[0-9]{3})([-\s\.]?[0-9]{3,4})");
            if (val == "")
            {
                val = null;
                throw new Exception("Field cannot be blank, number");

            }
            if (!rNmbr.IsMatch(val))
            {
                val = null;
                throw new Exception("Field is not a valid mobile number");

            }
            return val;
        }


        public string valBd(string val)
        {
            if (val == "")
            {
                val = null;
                throw new Exception("Field cannot be blank, text");

            }
            if (val.Length > 140)
            {
                val = null;
                throw new Exception("Text cannot be longer than 140 characters");

            }

            Dictionary<string, string> textspeak = new Dictionary<string, string>();

            foreach (string line in File.ReadAllLines(@"..\..\..\..\SoftEngCoursework\textwords.csv"))
            {
                string[] keyvalue = line.Split(',');
                if (keyvalue.Length == 2)
                {
                    textspeak.Add(keyvalue[0], keyvalue[1]);
                }
            }

            Regex rSplit = new Regex(@"[\d\s]");
            string[] msgSplit = rSplit.Split(val);

            foreach (var entry in textspeak)
            {
                val = val.Replace(entry.Key, entry.Key + " <" + entry.Value + ">");
            }
            
            return val;
        }
    }
}
