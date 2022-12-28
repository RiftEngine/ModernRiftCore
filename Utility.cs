using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rift.ModernRift.Core
{
    #region Exceptions
    public class ConfigurationFileNotFoundException : Exception
    {
        public ConfigurationFileNotFoundException() { }
        public ConfigurationFileNotFoundException(string message) : base(message) { }
        public ConfigurationFileNotFoundException(string message, Exception inner) : base(message, inner) { }
    }

    public class ConfigurationIncompleteException : Exception
    {
        public ConfigurationIncompleteException() { }
        public ConfigurationIncompleteException(string message) : base(message) { }
        public ConfigurationIncompleteException(string message, Exception inner) : base(message, inner) { }
    }

    public class InvalidAliasException : Exception
    {
        public InvalidAliasException() { }
        public InvalidAliasException(string message) : base(message) { }
        public InvalidAliasException(string message, Exception inner) : base(message, inner) { }
    }

    public class ProtectedCommandException : Exception
    {
        public ProtectedCommandException() { }
        public ProtectedCommandException(string message) : base(message) { }
        public ProtectedCommandException(string message, Exception inner) : base(message, inner) { }
    }

    public class ProtectedDirectiveException : Exception
    {
        public ProtectedDirectiveException() { }
        public ProtectedDirectiveException(string message) : base(message) { }
        public ProtectedDirectiveException(string message, Exception inner) : base(message, inner) { }
    }

    public class FilePathInvalidException : Exception
    {
        public FilePathInvalidException() { }
        public FilePathInvalidException(string message) : base(message) { }
        public FilePathInvalidException(string message, Exception inner) : base(message, inner) { }
    }

    public class GameReferenceNullException : Exception
    {
        public GameReferenceNullException() { }
        public GameReferenceNullException(string message) : base(message) { }
        public GameReferenceNullException(string message, Exception inner) : base(message, inner) { }
    }
    public class GameAlreadyInitializedException : Exception
    {
        public GameAlreadyInitializedException() { }
        public GameAlreadyInitializedException(string message) : base(message) { }
        public GameAlreadyInitializedException(string message, Exception inner) : base(message, inner) { }
    }

    public class GameAlreadyRunningException : Exception
    {
        public GameAlreadyRunningException() { }
        public GameAlreadyRunningException(string message) : base(message) { }
        public GameAlreadyRunningException(string message, Exception inner) : base(message, inner) { }
    }

    public class GameNotRunningException : Exception
    {
        public GameNotRunningException() { }
        public GameNotRunningException(string message) : base(message) { }
        public GameNotRunningException(string message, Exception inner) : base(message, inner) { }
    }

    public class GameNotInitializedException : Exception
    {
        public GameNotInitializedException() { }
        public GameNotInitializedException(string message) : base(message) { }
        public GameNotInitializedException(string message, Exception inner) : base(message, inner) { }
    }

    public class ConsoleColorInvalidException : Exception
    {
        public ConsoleColorInvalidException() { }
        public ConsoleColorInvalidException(string message) : base(message) { }
        public ConsoleColorInvalidException(string message, Exception inner) : base(message, inner) { }
    }

    public static class ConsoleManager
    {
        public static ConsoleColor StringToConsoleColor(string color)
        {
            color = color.ToLower();
            return color switch
            {
                "black" => ConsoleColor.Black,
                "darkblue" => ConsoleColor.DarkBlue,
                "darkgreen" => ConsoleColor.DarkGreen,
                "darkcyan" => ConsoleColor.DarkCyan,
                "darkred" => ConsoleColor.DarkRed,
                "darkmagenta" => ConsoleColor.DarkMagenta,
                "darkyellow" => ConsoleColor.DarkYellow,
                "gray" => ConsoleColor.Gray,
                "darkgray" => ConsoleColor.DarkGray,
                "blue" => ConsoleColor.Blue,
                "green" => ConsoleColor.Green,
                "cyan" => ConsoleColor.Cyan,
                "red" => ConsoleColor.Red,
                "magenta" => ConsoleColor.Magenta,
                "yellow" => ConsoleColor.Yellow,
                "white" => ConsoleColor.White,
                _ => throw new ConsoleColorInvalidException("The color specified was not a valid ConsoleColor.")
            };
        }
    }
    #endregion
}
