namespace Rift.ModernRift.Core
{
    public class Directive
    {
        public Action directive;

        public List<string> triggers = new();

        public bool isProtected = false;

        public Directive(Action directive)
        {
            this.directive = directive;
        }

        public Directive(Action directive, bool isProtected)
        {
            this.directive = directive;
            this.isProtected = isProtected;
        }

        public void AddTrigger(string trigger)
        {
            triggers.Add(trigger);
        }

        public void OverrideTriggers(List<string> triggers)
        {
            this.triggers = triggers;
        }

        public void Execute()
        {
            directive();
        }

        public bool ReceiveMessage(string message)
        {
            bool result = false;
            foreach(string trigger in triggers)
            {
                if(message.ToLower() == trigger.ToLower())
                {
                    Execute();
                    result = true;
                }
            }
            return result;
        }

        public void Add()
        {
            MessageHandler.AddDirective(this);
        }
    }

    public class ConsoleDirective : Directive {
        
        string content;

        public ConsoleDirective(Func<string> content) : base(() => { Console.WriteLine(content.Invoke()); })
        {
            this.content = content.Invoke();
        }
        public ConsoleDirective(Func<string> content, bool isProtected) : base(() => { Console.WriteLine(content.Invoke()); }, isProtected)
        {
            this.content = content.Invoke();
        }
    }

    public class InlineConsoleDirective : Directive
    {
        string content;

        public InlineConsoleDirective(Func<string> content) : base(() => { Console.Write(content.Invoke()); }) {
            this.content = content.Invoke();
        }
        public InlineConsoleDirective(Func<string> content, bool isProtected) : base(() => { Console.Write(content.Invoke()); }, isProtected)
        {
            this.content = content.Invoke();
        }
    }

    public class ConsoleFromFileDirective : Directive
    {
        string path;
        List<Func<object>> parameters = new();
        bool usingParameters = false;

        public ConsoleFromFileDirective(string path) : base(() => { Console.WriteLine(GetValue(path)); })
        {
            this.path = path;
        }

        public ConsoleFromFileDirective(string path, bool isProtected) : base(() => { Console.WriteLine(GetValue(path)); }, isProtected)
        {
            this.path = path;
        }

        public static string GetValue(string path)
        {
            try
            {
                return File.ReadAllText(path.ToString());
            }
            catch (Exception inner)
            {
                throw new FilePathInvalidException("The file path \"" + path.ToString() + "\" was not a valid file.", inner);
            }
        }

        public void AddParameter(Func<object> parameter)
        {
            parameters.Add(parameter);
        }

        public void RemoveParameter(int index)
        {
            parameters.RemoveAt(index);
        }

        public void RemoveParameter(Func<object> parameter)
        {
            parameters.Remove(parameter);
        }

        public void ToggleUseParameters()
        {
            if (!usingParameters)
            {
                usingParameters = true;
            } else
            {
                usingParameters = false;
            }

            if (usingParameters)
            {
                directive = () =>
                {
                    List<object> list = new List<object>();
                    foreach(Func<object> parameter in parameters)
                    {
                        list.Add(parameter.Invoke());
                    }
                    Console.WriteLine(GetValue(path), list.ToArray());
                };
            } else
            {
                directive = () =>
                {
                    Console.WriteLine(GetValue(path));
                };
            }
        }
    }

    public sealed class QuitDirective : Directive
    {
        public QuitDirective() : base(() => { Engine.Stop(); }, true) { }
    }

    public sealed class HelpDirective : Directive
    {
        public static bool enabled = true;

        public HelpDirective() : base(() => { if (enabled) Console.WriteLine(HelpData()); }) { }

        public HelpDirective(bool isProtected) : base(() => { if (enabled) Console.WriteLine(HelpData()); }, isProtected) { }

        private static string HelpData()
        {
            string result = "Game created with the ModernRift Game Engine. For more information, visit the RiftEngine GitHub profile.\n";
            List<string> names = new List<string>();
            List<string> descriptions = new List<string>();
            Dictionary<string, string> items = new();
            foreach (Command c in Engine.GameReference.Commands)
            {
                names.Add(c.Get());
                descriptions.Add(c.GetDescription());
                items.Add(c.Get(), c.GetDescription());
            }
            int maxLength = 0;
            foreach(string n in items.Keys)
            {
                if(n.Length > maxLength) maxLength = n.Length;
            }

            foreach (string n in items.Keys)
            {
                string name = n;
                for (int i = 0; i < maxLength - n.Length + 1; i++)
                {
                    name += " ";
                }
                result += name + "| " + items[n] + "\n";
            }
            return result;
        }
    }

    public sealed class ClearDirective : Directive
    {
        public static bool enabled = true;

        public ClearDirective() : base(() => { if(enabled) Console.Clear(); }) { }

        public ClearDirective(bool isProtected) : base(() => { if (enabled) Console.Clear(); }, isProtected) { }
    }
}
