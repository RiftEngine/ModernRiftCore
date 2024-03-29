﻿namespace Rift.ModernRift.Core
{
    public static class Engine
    {
        private static Game? gameReference;

        /// <summary>
        /// The public variable used to get the game reference. Will throw a GameReferenceNullException if accessed before setting the game reference.
        /// </summary>
        public static Game GameReference
        {
            get
            {
                if(gameReference == null)
                {
                    throw new GameReferenceNullException("The game reference is null.");
                }

                return gameReference;
            }

            set
            {
                gameReference = value;
            }
        }

        private static bool isInitialized;

        private static bool isStarted;

        /// <summary>
        /// Initializes the game reference & engine.
        /// </summary>
        public static void Initialize()
        {
            if (!isInitialized)
            {
                GameReference._Initialize();
                if (GameReference.configuration != null)
                {
                    isInitialized = true;
                    Console.BackgroundColor = ConsoleManager.StringToConsoleColor(GameReference.configuration.BackgroundColor);
                    Console.ForegroundColor = ConsoleManager.StringToConsoleColor(GameReference.configuration.TextColor);
                    Console.Clear();
                    return;
                }

                throw new ConfigurationNotSetException("The configuration was not set.");
            }

            throw new GameAlreadyInitializedException("The game has already been initialized.");
        }

        /// <summary>
        /// Starts the game reference & engine.
        /// </summary>
        public static void Start()
        {
            if (!isInitialized)
            {
                throw new GameNotInitializedException("The game has not been initialized.");
            }

            if (!isStarted)
            {
                isStarted = true;
                GameReference._Start();
                return;
            }

            throw new GameAlreadyRunningException("The game is already running.");
        }

        /// <summary>
        /// Stops the game reference & engine
        /// </summary>
        public static void Stop()
        {
            if (isStarted)
            {
                isStarted = false;
                GameReference._Stop();
                return;
            }

            throw new GameNotRunningException("The game is not running");
        }

        public static void PrintSystemMessage(string message) {
            Console.WriteLine("ModernRift System Message: "+message);
        }

        public static void PrintSystemError(string error) {
            Console.Error.WriteLine("ModernRift System Error: " + error);
        }
        
        public static void PrintSystemError(Exception error) {
            Console.Error.WriteLine("ModernRift System Error: " + error.Message);
        }
    }
}