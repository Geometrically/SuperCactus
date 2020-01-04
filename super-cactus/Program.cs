using System;
using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;

namespace super_cactus
{
    public class Program
    {
        private static DiscordSocketClient _client;
        
        private CommandService _commands;
        private IServiceProvider _services; 
        
        private static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        public async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            JobManager.Initialize(new Registry());
            
            _client.Log += Log;
            JobManager.JobException += info =>
                Log(new LogMessage(LogSeverity.Error, info.Name, info.Exception.Message, info.Exception));
            
            await RegisterCommandsAsync();
            
            await _client.LoginAsync(TokenType.Bot, ConfigurationManager.AppSettings["BotToken"]);

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
                    
                    Console.WriteLine(result.Error);

                    await msg.Channel.SendMessageAsync("", false, error.Build());
                }
            }
        }

        public static void SendMessage(ulong channelId, EmbedBuilder embed)
        {
            var channel = _client.GetChannel(channelId) as SocketTextChannel;

            channel.SendMessageAsync("", false, embed.WithAuthor(_client.CurrentUser).Build());
        }
    }
}