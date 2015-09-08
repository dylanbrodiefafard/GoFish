using System;
using System.Collections.Generic;
using System.Reflection;

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

        public static void Server_Register_Processor(IMessageProcessor server)
        {
            registerManager(server, "client");
        }

        public static void Client_Register_Processor(IMessageProcessor client)
        {
            registerManager(client, "server");
        }

        private static void registerManager(IMessageProcessor p, String source)
        {
            MethodInfo[] minfos = typeof(IMessageProcessor).GetMethods();
            foreach (MethodInfo minfo in minfos)
            {
                Console.WriteLine(minfo.Name);
                String methodName = minfo.Name;
                String[] fields = methodName.Split('_');
                String type = fields[1];

                processors[source + type] = Delegate.CreateDelegate(typeof(process_message), p, minfo.Name) as process_message;// p.GetType().GetMethod(minfo.Name).DeclaringType;

            }
        }
    }
}
