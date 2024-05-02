using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartFormat;

namespace Tidlix_Bot.Commands
{
    public class UserCommands
    {
        public void TryCommands(string cmd, string args, string user)
        {
            var Client = Program.Client;
            var API = Program.API;

            switch (cmd)
            {
                case "dsize":
                        var random = new Random();
                        var size = random.Next(-10, 50);

                    switch (size)
                    {
                        case < 0:
                            Client.SendMessage("tidlix", $"{user} hat eine Dsize von {size}cm! peepoGiggles");
                            break;
                        case <10: 
                            Client.SendMessage("tidlix", $"{user} hat eine durchschnitts Dsize von {size}cm! SadChamp");
                            break;
                        case < 20:
                            Client.SendMessage("tidlix", $"{user} hat eine Dsize von {size}cm! seemsGood");
                            break;
                        case < 30:
                            Client.SendMessage("tidlix", $"Die Dsize von {user} beträgt {size}cm! kok");
                            break;
                        case < 40:
                            Client.SendMessage("tidlix", $"{user} hat einen kok von {size}cm! NOWAYING");
                            break;
                        case >40:
                            Client.SendMessage("tidlix", $"{user} hat eine übermenschliche Dsize von {size}cm! GIGACHAD");
                            break;
                    } // send Message
                    break;

                default:
                    var custom = new CustomCommands();
                    string? getResponse = custom.getCommandResponse(cmd);
                        
                    if (getResponse == null) 
                    {
                        return;
                    }
                    
                    string response = Smart.Format(getResponse, new{user});

                    Client.SendMessage("tidlix", response);
                         
                    break;// Custom Commands
            }
        }
    }
}
