using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Extensions;
using TwitchLib.PubSub.Models.Responses.Messages.AutomodCaughtMessage;

namespace Tidlix_Bot.Commands
{
    public class ModCommands
    {
        CustomCommands custom = new CustomCommands();
        TwitchClient Client = Program.Client;

        public void TryCommands (string cmd, string args, string username)
        {
            switch(cmd)
            {
                // Custom Commands
                case "createcommand":
                    CreateCommand(args, username);
                    break;
                case "editcommand":
                    EditCommand(args, username);
                    break;
                case "deletecommand":
                    DeleteCommand(args, username);
                    break;

                // Moderation Commands
            }
        }

        // Custom Commands
        private void CreateCommand (string args, string username)
        {
            string[] argsArray = args.Split(" ", 2);
            string? key = argsArray[0];
            string? content = argsArray[1];

            if (key == null)
            {
                Client.SendMessage("tidlix", $"{username} Wie soll der Command heißen? Bitte nutze !createcommand *Command* *Content*");
                return;
            }
            if (content == null)
            {
                Client.SendMessage("tidlix", $"{username} Was soll der Command senden? Bitte nutze !createcommand *Command* *Content*");
                return;
            }

            string? tryCommand = custom.getCommandResponse(key);

            if (tryCommand != null)
            {
                Client.SendMessage("tidlix", $"@{username} Der Befehl, den du erstellen möchtest, gibt es bereits! Falls du diesen Befehl bearbeiten möchtest, nutze !editcommand {key} {content}");
            }
            else
            {
                custom.createCommandResponse(key, content);
                Client.SendMessage("tidlix", $"@{username} Der Befehl !{key} wurde erstellt!");
            }
        }
        private void EditCommand(string args, string username)
        {
            string[] argsArray = args.Split(" ", 2);
            string? key = argsArray[0];
            string? content = argsArray[1];

            if (key == null)
            {
                Client.SendMessage("tidlix", $"{username} Welchen Command möchtest du bearbeiten? Bitte nutze !editcommand *Command* *Content*");
                return;
            }
            if (content == null)
            {
                Client.SendMessage("tidlix", $"{username} Was soll der Command senden? Bitte nutze !edítcommand *Command* *Content*");
                return;
            }

            string? tryCommand = custom.getCommandResponse(key);

            if (tryCommand == null)
            {
                Client.SendMessage("tidlix", $"@{username} Der Befehl, den du bearbeiten möchtest, gibt es nicht! Falls du einen Befehl erstellen möchtest, nutze !createcommand {key} {content}");
            }
            else
            {
                custom.updateCommandResponse(key, content);
                Client.SendMessage("tidlix", $"@{username} Der Befehl !{key} wurde bearbeitet!");
            }
        }
        private void DeleteCommand(string args, string username)
        {
            string? tryCommand = custom.getCommandResponse(args);

            if (tryCommand == null)
            {
                Client.SendMessage("tidlix", $"@{username} Der Befehl, den du löschen möchtest, gibt es nicht!");
            }
            else
            {
                custom.deleteCommandResponse(args);
                Client.SendMessage("tidlix", $"@{username} Der Befehl !{args} wurde gelöscht!");
            }
        }

        // Moderation Commands
    } 
}
