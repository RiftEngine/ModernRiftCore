﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rift.ModernRift.Core
{
    public class Game
    {
        public readonly Configuration configuration;

        private List<Command> commands;

        public List<Command> Commands
        {
            get { return commands; }
        }

        private bool running = false;

        /// <summary>
        /// Initializes a new Game with a specific configuration
        /// </summary>
        /// <param name="configuration">The configuration to use.</param>
        public Game(Configuration configuration)
        {
            this.configuration = configuration;
            commands = new List<Command>() { new ClearCommand(), new HelpCommand(), new QuitCommand() };
        }
        
        public Game(Configuration configuration, List<Command> commands)
        {
            this.configuration = configuration;
            this.commands = commands;
            bool hasQuitCommand = false;
            foreach(Command c in commands)
            {
                if(c.GetType() == typeof(QuitCommand))
                {
                    hasQuitCommand = true;
                }
            }

            if(!hasQuitCommand)
            {
                this.commands.Add(new QuitCommand());
            }
        }

        public void AddCommand(Command command)
        {
            commands.Add(command);
        }

        public void RemoveCommand(Command command)
        {
            if(command.GetType() != typeof(QuitCommand))
            {
                commands.Remove(command);
            }
            else
            {
                throw new ProtectedCommandException("You cannot remove the QuitCommand from the list of commands");
            }
        }

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
            MessageHandler.AddDirective(new InlineConsoleDirective(() => { return configuration.Prompt; }, true), "tick");
            while (running)
            {
                Update();
                MessageHandler.AddMessage("tick");
                MessageHandler.SendMessages();

                string command = Console.ReadLine();
                foreach(Command c in commands)
                {
                    c.HandlePrompt(command);
                }
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
    }
}
