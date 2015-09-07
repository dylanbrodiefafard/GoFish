using System;

namespace GoFishCommon
{
    public delegate void Client_Received_Handler(Client sender, GenericEventArgs<String> e);
    public delegate void Client_Sending_Handler(Object sender, GenericEventArgs<String> e);
    public delegate void Client_Disconnected_Handler(Client sender, EventArgs e);
    public delegate void Client_Connected_Handler(Client sender, GenericEventArgs<String> e);
    
    public interface IClientHandler
    {
        event Client_Sending_Handler On_Client_Sending;
        void Client_Received(Client sender, GenericEventArgs<String> e);
        void Client_Connected(Client sender, GenericEventArgs<String> e);
        void Client_Disconnected(Client sender, EventArgs e);
    }
}
