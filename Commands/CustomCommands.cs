using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tidlix_Bot.Commands
{
    public class CustomCommands
    {
        public string? getCommandResponse (string CommandKey)
        {
            using (StreamReader sr = new StreamReader($"{AppDomain.CurrentDomain.BaseDirectory}CommandList.json"))
            {
                string json = sr.ReadToEnd();
                Dictionary<string, string>? commands = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                if (commands == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[Error] Commandlist is empty or not in \"{AppDomain.CurrentDomain.BaseDirectory}\"");
                    return null;
                }

                commands.TryGetValue(CommandKey, out string? command);

                return command;
            }
        }


        public void createCommandResponse(string CommandKey, string Content)
        {
            Dictionary<string, string>? commands;

            // Read JSON
            using (StreamReader sr = new StreamReader($"{AppDomain.CurrentDomain.BaseDirectory}CommandList.json"))
            {
                string _json = sr.ReadToEnd();
                commands = JsonConvert.DeserializeObject<Dictionary<string, string>>(_json);

                if (commands == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[Error] Commandlist is empty or not in \"{AppDomain.CurrentDomain.BaseDirectory}\"");
                    return;
                }
            }

            // Write JSON
            string json = "{\n//";
            json = $"{json}, \n\"{CommandKey}\": \"{Content}\"";
            foreach (var command in commands)
            {
                json = $"{json}, \n\"{command.Key}\": \"{command.Value}\"";
            }
            json = $"{json} \n" + "}";
            using (StreamWriter sw = new StreamWriter($"{AppDomain.CurrentDomain.BaseDirectory}CommandList.json"))
            {
                sw.Write(json);
            }
        }

        public void updateCommandResponse(string CommandKey, string newContent)
        {
            Dictionary<string, string>? commands;

            // Read JSON
            using (StreamReader sr = new StreamReader($"{AppDomain.CurrentDomain.BaseDirectory}CommandList.json"))
            {
                string _json = sr.ReadToEnd();
                commands = JsonConvert.DeserializeObject<Dictionary<string, string>>(_json);

                if (commands == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[Error] Commandlist is empty or not in \"{AppDomain.CurrentDomain.BaseDirectory}\"");
                    return;
                }
            }
            
            // Update Dictionary
            foreach (var command in commands.ToList())
            {
                if (command.Key == CommandKey)
                {
                    commands.Remove(command.Key);

                    commands.Add(CommandKey, newContent);
                }
            }

            // Write JSON
            string json = "{\n//";
            foreach (var command in commands)
            {
                json = $"{json}, \n\"{command.Key}\": \"{command.Value}\"";
            }
            json = $"{json} \n" + "}";
            using (StreamWriter sw = new StreamWriter($"{AppDomain.CurrentDomain.BaseDirectory}CommandList.json"))
            {
                sw.Write(json);
            }
        }

        public void deleteCommandResponse(string CommandKey)
        {
            Dictionary<string, string>? commands;

            // Read JSON
            using (StreamReader sr = new StreamReader($"{AppDomain.CurrentDomain.BaseDirectory}CommandList.json"))
            {
                string _json = sr.ReadToEnd();
                commands = JsonConvert.DeserializeObject<Dictionary<string, string>>(_json);

                if (commands == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[Error] Commandlist is empty or not in \"{AppDomain.CurrentDomain.BaseDirectory}\"");
                    return;
                }
            }

            // Update Dictionary
            foreach (var command in commands)
            {
                if (command.Key == CommandKey)
                {
                    commands.Remove(command.Key);
                }
            }

            // Write JSON
            string json = "{\n//";
            foreach (var command in commands)
            {
                json = $"{json}, \n\"{command.Key}\": \"{command.Value}\"";
            }
            json = $"{json} \n" + "}";
            using (StreamWriter sw = new StreamWriter($"{AppDomain.CurrentDomain.BaseDirectory}CommandList.json"))
            {
                sw.Write(json);
            }
        }
    }
}
