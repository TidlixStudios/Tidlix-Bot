using Tidlix_Bot.Config;
using Tidlix_Bot.Tokens;
using TwitchLib.Api;
using TwitchLib.Api.Services;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using TwitchLib.Communication.Interfaces;
using static System.Formats.Asn1.AsnWriter;
using TwitchLib.Api.Helix.Models.Search;
using Tidlix_Bot.Commands;

namespace Tidlix_Bot
{
    public class Program
    {
        public static ConfigReader Config { get; set; } = new ConfigReader();
        public static TwitchClient Client { get; set; } = new TwitchClient();
        public static TwitchAPI API { get; set; } = new TwitchAPI();


        public static string? AccessToken { get; set; } = null;
        private static List<string> scopes = new List<string> { "chat:read", "whispers:read", "whispers:edit", "chat:edit", "channel:moderate" };


        static async Task Main(string[] args)
        {
            Config.ReadConfig();

            API = new TwitchAPI();

            await getAccessToken(API);

            API.Settings.ClientId = Config.clientID;
            API.Settings.AccessToken = AccessToken;


            ConnectionCredentials credentials = new ConnectionCredentials(Config.username, AccessToken);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            Client = new TwitchClient(customClient);

            Client.Initialize(credentials, "tidlix");

            Client.OnConnected += OnConnect;
            Client.OnConnectionError += OnConnectionError;
            Client.OnJoinedChannel += JoinedChannel;
            Client.OnMessageReceived += MessageRecived;
            Client.OnChatCommandReceived += CommandRecieved;
            Client.OnRaidNotification += RaidRecieved;
            

            Client.Connect();

            await Task.Delay(-1);
        }




        private static void OnConnect(object? sender, OnConnectedArgs e)
        {
            Console.WriteLine("Connected");
        }
        private static void OnConnectionError(object? sender, OnConnectionErrorArgs e)
        {
            Console.WriteLine($"Connection Error: {e.Error.Message}");
        }
        private static void JoinedChannel(object? sender, OnJoinedChannelArgs e)
        {
            Console.WriteLine($"Joined channel: {e.Channel}");
        }


        private static void MessageRecived(object? sender, OnMessageReceivedArgs e)
        {
            //Console.WriteLine("Message: " + e.ChatMessage.Message);
        }
        private static void CommandRecieved(object? sender, OnChatCommandReceivedArgs e)
        {
            ModCommands modCmd = new ModCommands();
            UserCommands userCmd = new UserCommands();


            string command = e.Command.CommandText.ToLower();

            switch (command)
            {
                case "yt":
                    command = "youtube";
                    break;
                case "dc":
                    command = "discord";
                    break;
                case "createcmd":
                    command = "createcommand";
                    break;
                case "editcmd":
                    command = "editcommand";
                    break;
                case "deletecmd":
                    command = "deletecommand";
                    break;
            }

            string args = e.Command.ArgumentsAsString;
            string username = e.Command.ChatMessage.Username;

            if (e.Command.ChatMessage.IsModerator || e.Command.ChatMessage.IsBroadcaster)
            {
                modCmd.TryCommands(command, args, username);
                userCmd.TryCommands(command, args, username);
            }
            else
            {
                userCmd.TryCommands(command, args, username);
            }
        }
        private static void RaidRecieved(object? sender, OnRaidNotificationArgs e)
        {
            string raider = e.RaidNotification.DisplayName;
            string viewer = e.RaidNotification.MsgParamViewerCount.ToString();

            Client.SendMessage("tidlix", $"{raider} Raidet den Stream mit {viewer} Zuschauenern GIGACHAD");
        }



        private static async Task getAccessToken(TwitchAPI api)
        {
            var server = new WebServer("http://localhost:8080/redirect/");

            Console.WriteLine($"Please Authorize your Twitch Account here: {getAuthorizationCodeUrl(Config.clientID, "http://localhost:8080/redirect/", scopes)}");

            var auth = await server.Listen();
            var resp = await api.Auth.GetAccessTokenFromCodeAsync(auth.Code, Config.clientSecret, "http://localhost:8080/redirect/", Config.clientID);

            var newToken = await api.Auth.RefreshAuthTokenAsync(resp.RefreshToken, Config.clientSecret, Config.clientID);

            AccessToken = newToken.AccessToken;
        }

        private static string getAuthorizationCodeUrl(string clientId, string redirectUri, List<string> scopes)
        {
            var scopesStr = string.Join("+", scopes);

            return "https://id.twitch.tv/oauth2/authorize?" +
            $"client_id={clientId}&" +
            $"redirect_uri={System.Web.HttpUtility.UrlEncode(redirectUri)}&" +
            "response_type=code&" +
            $"scope={scopesStr}";
        }

    }
}
