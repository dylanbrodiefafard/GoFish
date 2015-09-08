using System;
using System.Collections.Generic;

namespace GoFishCommon
{
    public static class MessageHandler
    {
        public const String denied = "denied";
        public delegate void process_message(String message);
        private static Dictionary<String, process_message> processors = new Dictionary<string, process_message>();

        public static void Handle(String message)
        {
            if (message == null) throw new ArgumentNullException("Message can't be null.");
            String[] fields = message.Split(':');
            if (fields == null) throw new FormatException("Missing ':' separators.");
            if (fields.Length < 3) throw new MissingFieldException("Missing payload field.");
            String source = fields[0];
            String type = fields[1];
            String payload = fields[2];
            if (!processors.ContainsKey(source + type)) throw new NotImplementedException("No processor registered for message: " + source + type + ".");
            processors[source + type](payload);
        }

        public static void Register_Processor(String source, String type, process_message processor)
        {
            processors.Add(source + type, processor);
        }
    }
}
