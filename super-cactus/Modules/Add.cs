using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using super_cactus.Models;

namespace super_cactus.Modules
{
    public class Add : ModuleBase<SocketCommandContext>
    {
        [Command("add")]
        [Summary("A command to add an event to the calendar")]
        public async Task AddAsync(string type, string className, string name, string description, string date)
        {
            if (type.ToLower() != "quiz" && type.ToLower() != "test" && type.ToLower() != "event")
            {
                var error = new EmbedBuilder()
                    .WithTitle("Command Failed :(")
                    .WithDescription(type + " is not a valid type!")
                    .WithFooter("Created by Jai")
                    .WithColor(Color.Red);
                
                await ReplyAsync("", false, error.Build());
                return;
            }

            DateTime parsedDate;
            if (DateTime.TryParse(date, out parsedDate))
            {
                await Program.AddEventAsync(new Event
                {
                    Type = type,
                    ClassName = className,
                    Name = name,
                    Description = description,
                    Date = parsedDate,
                    ServerId = Context.Guild.Id
                });
                
                var builder = new EmbedBuilder()
                    .WithTitle(char.ToUpper(type[0]) + type.Substring(1) + " Added")
                    .WithDescription($"{char.ToUpper(type[0]) + type.Substring(1)} added for {className} on {parsedDate}!")
                    .AddField(new EmbedFieldBuilder().WithName(name).WithValue(description))
                    .WithColor(Color.Green)
                    .WithFooter("Created by Jai")
                    .WithAuthor(Context.User);

                await ReplyAsync("", false, builder.Build());
            }
            else
            {
                var error = new EmbedBuilder()
                    .WithTitle("Command Failed :(")
                    .WithDescription("Invalid Date!")
                    .WithFooter("Created by Jai")
                    .WithColor(Color.Red);
                
                await ReplyAsync("", false, error.Build());
            }
        }
    }
}