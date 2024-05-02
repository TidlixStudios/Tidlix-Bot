using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tidlix_Bot.Config
{
    public class ConfigReader
    {
        public string username { get; set; } = "placeholder_username";
        public string channelID { get; set; } = "placeholder_channelID";
        public string clientID { get; set; } = "placeholder_clientID";
        public string clientSecret { get; set; } = "placeholder_clientSecret";

        public void ReadConfig ()
        {
            using (StreamReader sr = new StreamReader($"{AppDomain.CurrentDomain.BaseDirectory}config.json"))
            {
                var json = sr.ReadToEnd();

                ConfigStructure? data = JsonConvert.DeserializeObject<ConfigStructure>(json);

                if (data == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"[ERROR] Config is empty or not in \"{AppDomain.CurrentDomain.BaseDirectory}\"!");
                    Console.ResetColor();

                    return;
                }

                this.username = data.username;
                this.channelID = data.channelID;
                this.clientID = data.clientID;
                this.clientSecret = data.clientSecret;
            }
        }

        private class ConfigStructure
        {
            public string username { get; set; } = "placeholder_username";
            public string channelID { get; set; } = "placeholder_channelID";
            public string clientID { get; set; } = "placeholder_clientID";
            public string clientSecret { get; set; } = "placeholder_clientSecret";
        }
    }
}
