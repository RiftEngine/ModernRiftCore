using Microsoft.Extensions.Configuration;


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace Rift.ModernRift.Core
{
    public class Configuration
    {
        public string Name { get; } // Mandatory

        public string Description { get; } // Optional

        public string CreatorName { get; } // Optional

        public string Version { get; } // Mandatory

        public string WindowTitle { get; } // Optional

        public string Prompt { get; } // Optional

        public string BackgroundColor { get; } // Optional

        public string TextColor { get; } // Optional

        private Configuration(ConfigurationBuilder builder)
        {
            Name = builder.name;
            Version = builder.version;
            if (builder.description != null) Description = builder.description; else Description = "";
            if (builder.creatorName != null) CreatorName = builder.creatorName; else CreatorName = "";
            if (builder.windowTitle != null) WindowTitle = builder.windowTitle; else WindowTitle = Name;
            if (builder.prompt != null) Prompt = builder.prompt; else Prompt = Name + " > ";
            if (builder.backgroundColor != null) BackgroundColor = builder.backgroundColor; else BackgroundColor = "Black";
            if (builder.textColor != null) TextColor = builder.textColor; else TextColor = "White";
        }

        private Configuration() { }

        public class ConfigurationBuilder
        {
            public string name; // Mandatory

            public string description; // Optional

            public string creatorName; // Optional

            public string version; // Mandatory

            public string windowTitle; // Optional

            public string prompt; // Optional

            public string backgroundColor;
            
            public string textColor;

            public ConfigurationBuilder(string name, string version)
            {
                this.name = name;
                this.version = version;
            }

            public ConfigurationBuilder()
            {

            }

            public ConfigurationBuilder Name(string name)
            {
                this.name = name;
                return this;
            }

            public ConfigurationBuilder Version(string version)
            {
                this.version = version;
                return this;
            }

            public ConfigurationBuilder Description(string description)
            {
                this.description = description;
                return this;
            }

            public ConfigurationBuilder CreatorName(string creatorName)
            {
                this.creatorName = creatorName;
                return this;
            }

            public ConfigurationBuilder WindowTitle(string windowTitle)
            {
                this.windowTitle = windowTitle;
                return this;
            }

            public ConfigurationBuilder Prompt(string prompt)
            {
                this.prompt = prompt;
                return this;
            }

            public ConfigurationBuilder BackgroundColor(string backgroundColor)
            {
                this.backgroundColor = backgroundColor;
                return this;
            }
            
            public ConfigurationBuilder TextColor(string textColor)
            {
                this.textColor = textColor;
                return this;
            }

            public ConfigurationBuilder FromConfigFile(string filePath)
            {
                try
                {
                    var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile
                        (filePath).Build()
                            .GetSection("Config");
                    name = (string)config.GetValue(typeof(string), "name");
                    description = (string)config.GetValue(typeof(string), "description");
                    creatorName = (string)config.GetValue(typeof(string), "creatorName");
                    version = (string)config.GetValue(typeof(string), "version");
                    windowTitle = (string)config.GetValue(typeof(string), "windowTitle");
                    prompt = (string)config.GetValue(typeof(string), "prompt");
                    backgroundColor = (string)config.GetValue(typeof(string), "backgroundColor");
                    textColor = (string)config.GetValue(typeof(string), "textColor");
                    return this;
                } catch (Exception inner)
                {
                    throw new ConfigurationFileNotFoundException("The file path \"" + filePath + "\" was not a valid json file.", inner);
                }
            }

            public Configuration Build()
            {
                Configuration configuration = new(this);
                Validate(configuration);
                return configuration;
            }

            private static void Validate(Configuration configuration)
            {
                if(configuration.Name == null || configuration.Version == null)
                {
                    throw new ConfigurationIncompleteException("The configuration is incomplete");
                }
            }
        }
    }
}
