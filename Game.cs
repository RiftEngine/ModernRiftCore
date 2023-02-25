using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rift.ModernRift.Core
{
    public class Game
    {
        public Configuration configuration;

        private List<Command> commands;

        public List<Command> Commands
        {
            get { return commands; }
        }

        private bool running = false;

        /// <summary>
        /// Initializes a new Game
        /// </summary>
        public Game()
        {
            commands = new List<Command>() { new ClearCommand(), new HelpCommand(), new QuitCommand() };
        }

        /// <summary>
        /// Adds a command to the list of commands
        /// </summary>
        public void AddCommand(Command command)
        {
            commands.Add(command);
        }

        /// <summary>
        /// Removes a command from the list of commands
        /// </summary>
        public void RemoveCommand(Command command)
        {
            if (command.GetType() != typeof(QuitCommand))
            {
                commands.Remove(command);
            }
            else
            {
                throw new ProtectedCommandException("You cannot remove the QuitCommand from the list of commands");
            }
        }

        #region Runtime Methods

        /// <summary>
        /// Backend initialization method, called by the game engine. You should never need to reference this method.
        /// </summary>
        public void _Initialize()
        {
            Initialize();
        }
        
        /// <summary>
        /// Override this method to run code when the game is being initialized. Do not directly call this method, instead use Engine.Initialize().
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Backend start method, called by the game engine. You should never need to reference this method.
        /// </summary>
        public void _Start()
        {
            running = true;
            Start();
            // Add the prompt directive to the messagehandler
            MessageHandler.AddDirective(new InlineConsoleDirective(() => { return configuration.Prompt; }, true), "update");
            while (running)
            {
                MessageHandler.SendMessage("update");
                Update();

                string command = Console.ReadLine();
                foreach(Command c in commands)
                {
                    c.HandlePrompt(command.ToLower().Trim());
                }

                MessageHandler.SendMessages();
            }
        }

        /// <summary>
        /// Override this method to run code when the game is being started. Do not directly call this method, instead use Engine.Start().
        /// </summary>
        public virtual void Start() { }

        /// <summary>
        /// This is the main game loop. It will be called once per loop iteration, or "tick". In command-based games, it is called every time a command is entered. The command can be referenced from inside the method.
        /// </summary>
        public virtual void Update() { }
        
        /// <summary>
        /// Backend stop method, called by the game engine. You should never need to reference this method.
        /// </summary>
        public void _Stop()
        {   
            Stop();
            running = false;
        }

        /// <summary>
        /// Override this method to run code when the game is being stopped. Do not directly call this method, instead use Engine.Stop().
        /// </summary>
        public virtual void Stop() { }

        #endregion
    }
}
