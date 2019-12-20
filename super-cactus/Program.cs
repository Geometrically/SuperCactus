using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
namespace super_cactus
{
    public class Program
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        
        private const string _token = "NjU1OTUxMjIyMTEyNTE4MTY0.Xfbs0Q.Iitov1BSpltzNL7Ukw01QMtdCdw";

        private static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        public async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();
            
            _client.Log += Log; 
            await RegisterCommandsAsync();

            _client.JoinedGuild += AddServerAsync;
            
            await _client.LoginAsync(TokenType.Bot, _token);

            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg);
            
            return Task.CompletedTask;
            
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage msg)
        {
            var message = msg as SocketUserMessage;

            if (message == null || message.Author.IsBot) return;

            var argPos = 0;

            if (message.HasStringPrefix("!", ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(_client, message);

                var result = await _commands.ExecuteAsync(context, argPos, _services);

                if (!result.IsSuccess)
                {
                    var error = new EmbedBuilder()
                        .WithTitle("Command Failed :(")
                        .WithDescription(result.ErrorReason)
                        .WithFooter("Created by Jai")
                        .WithColor(Color.Red);

                    await msg.Channel.SendMessageAsync("", false, error.Build());
                }
            }
        }

        private async Task AddServerAsync(SocketGuild guild)
        {
            
        }
    }
}