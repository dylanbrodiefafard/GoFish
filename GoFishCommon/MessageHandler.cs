using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GoFishCommon
{
    public class MessageHandler
    {
        private StreamWriter logfile;

        public MessageHandler(String logname)
        {
            try
            {
                logfile = new StreamWriter("logname");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Unable to open " + logname + " for writing.");
            }
        }

        protected void DebugMessage(String message)
        {
            if (logfile != null) logfile.WriteLine("[" + DateTime.Now +"] " + message);
        }


        public String Send_ConnectMessage(String name)
        {
            StringBuilder message = new StringBuilder();
            message.Append("connect,");
            message.Append(name);
            DebugMessage(message.ToString());
            return message.ToString();
        }

        public String Recieve_ConnectMessage(String message)
        {
            DebugMessage(message);
            String[] fields = message.Split(',');
            if (fields.Length > 1) return fields[2];
            else return "default";
        }
        
    }
}
