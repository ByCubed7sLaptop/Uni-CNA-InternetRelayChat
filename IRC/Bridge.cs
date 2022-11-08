using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Links the events from the Server to the IRC Chat
public class Bridge
{
    public IRC.Chat chat;

    private Bridge(IRC.Chat chat)
    {
        this.chat = chat;
    }

    static public Bridge CreateServerBridge(IRC.Chat chat)
    {
        Bridge newBridge = new Bridge(chat);

        chat.MessageSent += newBridge._Chat_MessageSent;
        chat.MemberJoined += newBridge._Chat_MemberJoined;
        chat.MemberLeft += newBridge._Chat_MemberLeft;

        return newBridge;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    private void _Chat_MessageSent(object? sender, IRC.MessageSentEventArgs e)
    {
        IRC.Message message = e.Message;
        Console.WriteLine("[CLIENT] [" + message.SentAt.ToString("HH:mm:ss") + "] " + message.Author.UserName + ": " + message.Body);

    }

    private void _Chat_MemberJoined(object? sender, IRC.MemberJoinedEventArgs e)
    {
        Console.WriteLine("[Client] [" + DateTime.Now.ToString("HH:mm:ss") + "] " + e.Member.UserName + " Joined!");
    }

    private void _Chat_MemberLeft(object? sender, IRC.MemberLeftEventArgs e)
    {
        Console.WriteLine("[Client] [" + DateTime.Now.ToString("HH:mm:ss") + "] " + e.Member.UserName + " Left!");
    }
}
