using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rift.ModernRift.Core
{
    /// <summary>
    /// Runs a list of Directives when a specific string is typed into the console
    /// </summary>
    public class Command
    {
        /// <summary>
        /// The text that needs to be entered to trigger the command
        /// </summary>
        internal string Data;

        /// <summary>
        /// The list of directives to execute when the command is executed
        /// </summary>
        public List<Directive> Directives;

        /// <summary>
        /// The description of the command
        /// </summary>
        public string Description;

        /// <param name="data">The text that needs to be entered to trigger the command</param>
        /// <param name="directives">The list of directives to be executed when the command is run</param>
        public Command(string data, params Directive[] directives)
        {
            Directives = new();
            foreach (Directive d in directives)
            {
                Directives.Add(d);
            }
            Data = data;
            Description = "No description provided";
        }

        /// <param name="data">The text that needs to be enterd to trigger the command</param>
        /// <param name="description">The description of the command</param>
        /// <param name="directives">The list of directives to be executed when the command is run</param>
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

        /// <summary>
        /// Returns the value of Data
        /// </summary>
        /// <returns>The text that needs to be entered to trigger the command</returns>
        public string Get()
        {
            return Data;
        }

        /// <summary>
        /// Returns the value of Description
        /// </summary>
        /// <returns>The description of the command</returns>
        public string GetDescription()
        {
            return Description;
        }

        /// <summary>
        /// Execute all of the directives in sequence
        /// </summary>
        private void Execute()
        {
            foreach(Directive directive in Directives)
            {
                directive.Execute();
            }
        }

        /// <summary>
        /// Compare a prompt to the required text, and execute the directives if the two match
        /// </summary>
        /// <param name="command">The text to compare the required text to; usually something typed by the user.</param>
        public void HandlePrompt(string command)
        {
            string relevantCommand = command;
            if (relevantCommand == Data.ToLower().Trim())
            {
                Execute();
            }
        }

        /// <summary>
        /// Change the description of the command
        /// </summary>
        /// <param name="newDescription">What the description should be changed to</param>
        public void UpdateDescription(string newDescription)
        {
            Description = newDescription;
        }

        /// <summary>
        /// Add the command to the engine
        /// </summary>
        public void Add()
        {
            Engine.GameReference.AddCommand(this);
        }
    }

    /// <summary>
    /// Command that safely stops the program
    /// </summary>
    public class QuitCommand : Command
    {
        /// <summary>
        /// What should be typed if the user wants the command to be something other than "quit"
        /// </summary>
        private string alias;

        public QuitCommand() : base("quit", "Safely stops the program.", new QuitDirective())
        {
            alias = "quit";
        }

        /// <param name="alias">What should be typed to execute the command</param>
        public QuitCommand(string alias) : base(alias, "Safely stops the program.", new QuitDirective()) {
            this.alias = alias;
        }

        /// <summary>
        /// Change what should be typed to execute the command
        /// </summary>
        /// <param name="alias">What should be typed to execute the command</param>
        /// <exception cref="InvalidAliasException">You are trying to set two different commands to have the same alias, or are trying to set the alias to null or ""</exception>
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

    /// <summary>
    /// Lists all of the commands in the game along with the descriptions of the commands
    /// </summary>
    public class HelpCommand : Command
    {
        /// <summary>
        /// What should be typed in order to execute the command
        /// </summary>
        private string alias;

        /// <summary>
        /// Whether this command should be able to be executed by the user
        /// </summary>
        private bool enabled = true;

        public HelpCommand() : base("help", "Writes a brief description of each command.", new HelpDirective())
        {
            alias = "help";
        }

        /// <param name="alias">What should be typed to execute the command</param>
        public HelpCommand(string alias) : base(alias, "Writes a brief description of each command.", new HelpDirective())
        {
            this.alias = alias;
        }

        /// <summary>
        /// Change what should be typed to execute the command
        /// </summary>
        /// <param name="alias">What should be typed to execute the command</param>
        /// <exception cref="InvalidAliasException">You are trying to set two different commands to have the same alias</exception>
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

        /// <summary>
        /// Prevent the user from executing the command
        /// </summary>
        public void Disable()
        {
            HelpDirective.enabled = false;
            enabled = false;
        }
        
        /// <summary>
        /// Allow the user to execute the command
        /// </summary>
        public void Enable()
        {
            HelpDirective.enabled = true;
            enabled = true;
        }
    }

    public class ClearCommand : Command
    {
        /// <summary>
        /// What should be typed in order to execute the command
        /// </summary>
        private string alias;

        /// <summary>
        /// Whether this command should be able to be executed by the user
        /// </summary>
        private bool enabled = true;

        public ClearCommand() : base("clear", "Clears the window.", new ClearDirective())
        {
            alias = "clear";
        }

        /// <param name="alias">What should be typed to execute the command</param>
        public ClearCommand(string alias) : base(alias, "Clears the window.", new ClearDirective())
        {
            this.alias = alias;
        }

        /// <summary>
        /// Change what should be typed to execute the command
        /// </summary>
        /// <param name="alias">What should be typed to execute the command</param>
        /// <exception cref="InvalidAliasException">You are trying to set two different commands to have the same alias</exception>
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

        /// <summary>
        /// Prevent the user from executing the command
        /// </summary>
        public void Disable()
        {
            ClearDirective.enabled = false;
            enabled = false;
        }

        /// <summary>
        /// Allow the user to execute the command
        /// </summary>
        public void Enable()
        {
            ClearDirective.enabled = true;
            enabled = true;
        }
    }
}
