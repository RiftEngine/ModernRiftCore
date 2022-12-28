using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rift.ModernRift.Core
{
    public class Command
    {
        internal string Data;

        public List<Directive> Directives;

        public string Description;

        public string Get()
        {
            return Data;
        }

        public string GetDescription()
        {
            return Description;
        }

        private void Execute()
        {
            foreach(Directive directive in Directives)
            {
                directive.Execute();
            }
        }

        public void HandlePrompt(string command)
        {
            string relevantCommand = command;
            if (relevantCommand == Data)
            {
                Execute();
            }
        }

        public void UpdateDescription(string newDescription)
        {
            Description = newDescription;
        }

        public void Add()
        {
            Engine.GameReference.AddCommand(this);
        }

        public Command(string data, params Directive[] directives)
        {
            Directives = new();
            foreach(Directive d in directives)
            {
                Directives.Add(d);
            }
            Data = data;
            Description = "No description provided";
        }

        public Command(string data, string description, params Directive[] directives)
        {
            Directives = new();
            foreach (Directive d in directives)
            {
                Directives.Add(d);
            }
            Data = data;
            Description = description;
        }
    }

    public class QuitCommand : Command
    {
        private string alias;

        public QuitCommand(string alias) : base(alias, "Safely stops the program.", new QuitDirective()) {
            this.alias = alias;
        }

        public QuitCommand() : base("quit", "Safely stops the program.", new QuitDirective())
        {
            alias = "quit";
        }

        public void UpdateAlias(string alias)
        {
            foreach (Command c in Engine.GameReference.Commands)
            {
                if (alias == c.Get())
                {
                    if (c.GetType() != typeof(QuitCommand))
                    {
                        throw new InvalidAliasException("Two different commands cannot have the same alias.");
                    }
                }
            }
            if (alias != "")
            {
                this.alias = alias;
                Data = alias;
            } else
            {
                throw new InvalidAliasException("You cannot set the alias of QuitCommand to be null.");
            }
        }
    }

    public class HelpCommand : Command
    {
        private string alias;
        private bool enabled = true;

        public HelpCommand(string alias) : base(alias, "Writes a brief description of each command.", new HelpDirective())
        {
            this.alias = alias;
        }

        public HelpCommand() : base("help", "Writes a brief description of each command.", new HelpDirective())
        {
            alias = "help";
        }

        public void UpdateAlias(string alias)
        {
            foreach (Command c in Engine.GameReference.Commands)
            {
                if (alias == c.Get())
                {
                    if (c.GetType() != typeof(HelpCommand))
                    {
                        throw new InvalidAliasException("Two different commands cannot have the same alias.");
                    }
                }
            }
            if (alias != null)
            {
                this.alias = alias;
                Data = alias;

                HelpDirective.enabled = true;
                enabled = true;
            }
            else
            {
                enabled = false;
                HelpDirective.enabled = false;
            }
        }

        public void Disable()
        {
            HelpDirective.enabled = false;
            enabled = false;
        }
        
        public void Enable()
        {
            HelpDirective.enabled = true;
            enabled = true;
        }
    }

    public class ClearCommand : Command
    {
        private string alias;
        private bool enabled = true;

        public ClearCommand(string alias) : base(alias, "Clears the window.", new ClearDirective())
        {
            this.alias = alias;
        }

        public ClearCommand() : base("clear", "Clears the window.", new ClearDirective())
        {
            alias = "clear";
        }

        public void UpdateAlias(string alias)
        {
            foreach(Command c in Engine.GameReference.Commands)
            {
                if(alias == c.Get())
                {
                    if(c.GetType() != typeof(ClearCommand)) {
                        throw new InvalidAliasException("Two different commands cannot have the same alias.");
                    }
                }
            }
            if (alias != null)
            {
                this.alias = alias;
                Data = alias;

                ClearDirective.enabled = true;
                enabled = true;
            }
            else
            {
                enabled = false;
                ClearDirective.enabled = false;
            }
        }

        public void Disable()
        {
            ClearDirective.enabled = false;
            enabled = false;
        }

        public void Enable()
        {
            ClearDirective.enabled = true;
            enabled = true;
        }
    }
}
