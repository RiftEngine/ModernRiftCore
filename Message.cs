using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rift.ModernRift.Core
{
    public class Message
    {
        public string content;

        public bool isProtected = false;

        public uint delayCount = 0;

        public Message(string content)
        {
            this.content = content.ToLower();
        }
        public Message(string content, bool isProtected)
        {
            this.content = content.ToLower();
            this.isProtected = isProtected;
        }
        public Message(string content, uint delayCount)
        {
            this.content = content.ToLower();
            this.delayCount = delayCount;
        }
        public Message(string content, uint delayCount, bool isProtected)
        {
            this.content = content.ToLower();
            this.delayCount = delayCount;
            this.isProtected = isProtected;
        }
    }

    public static class MessageHandler
    {
        private static List<Message> unreadMessages = new List<Message>();

        private static List<Directive> directives = new List<Directive>();

        public static void SendMessages()
        {
            List<Message> messagesToRemove = new List<Message>();
            foreach (Directive d in directives)
            {
                foreach (Message m in unreadMessages)
                {
                    if(m.delayCount != 0)
                    {
                        m.delayCount--;
                        break;
                    }
                    d.ReceiveMessage(m.content);
                    if (!m.isProtected)
                    {
                        messagesToRemove.Add(m);
                    }
                }
            }
            foreach (Message m in messagesToRemove)
            {
                unreadMessages.Remove(m);
            }
        }

        public static void AddMessage(string message, bool isProtected = false)
        {
            foreach (Message m in unreadMessages)
            {
                if (m.content == message)
                {
                    return;
                }
            }
            unreadMessages.Add(new Message(message, isProtected));
        }

        public static void AddDelayedMessage(string message, uint delayCount, bool isProtected = false)
        {
            foreach (Message m in unreadMessages)
            {
                if (m.content == message)
                {
                    return;
                }
            }
            unreadMessages.Add(new Message(message, delayCount, isProtected));
        }

        public static void SendMessage(string message)
        {
            foreach (Directive d in directives)
            {
                d.ReceiveMessage(message);
            }
        }

        public static void AddDirective(Directive directive)
        {
            directives.Add(directive);
        }

        public static void AddDirective(Directive directive, string trigger)
        {
            directive.AddTrigger(trigger);
            directives.Add(directive);
        }

        public static void AddDirective(Directive directive, List<string> triggers)
        {
            foreach (string t in triggers)
            {
                directive.AddTrigger(t);
            }
            directives.Add(directive);
        }

        public static void RemoveDirective(Directive directive)
        {
            if (!directive.isProtected)
            {
                directives.Remove(directive);
            }
            else
            {
                throw new ProtectedDirectiveException("The directive " + directive.ToString() + " could not be deleted.");
            }
        }
    }
}
