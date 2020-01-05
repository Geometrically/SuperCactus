using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FluentScheduler;
using super_cactus.Preconditions;

namespace super_cactus.Modules
{
    public class Add : ModuleBase<SocketCommandContext>
    {
        [Command("add")]
        [RequireRole("Calendar Helper")]
        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [Summary("A command to add an event to the calendar")]
        public async Task AddAsync(string type, string date, ISocketMessageChannel reminderChannel, string className, string name, [Remainder] string description)
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

            DateTimeOffset parsedDate;
            
            if (DateTimeOffset.TryParse(date, out parsedDate))
            {
                if((parsedDate - DateTime.Now).Days + 1 >= 5)
                    JobManager.AddJob(
                        () => SendEventMessage(type, parsedDate.Date, reminderChannel, className, name, description), 
                        s => s.ToRunOnceAt(parsedDate.Date.AddDays(-5)));
                if((parsedDate - DateTime.Now).Days + 1 >= 3)
                    JobManager.AddJob(
                        () => SendEventMessage(type, parsedDate.Date, reminderChannel, className, name, description), 
                        s => s.ToRunOnceAt(parsedDate.Date.AddDays(-5)));
                if((parsedDate - DateTime.Now).Days + 1 >= 1)
                    JobManager.AddJob(
                        () => SendEventMessage(type, parsedDate.Date, reminderChannel, className, name, description), 
                        s => s.ToRunOnceAt(parsedDate.Date.AddDays(-5)));
                    
                JobManager.AddJob(
                    () => SendEventMessage(type, parsedDate.Date, reminderChannel, className, name, description), 
                    s => s.ToRunOnceAt(parsedDate.Date));


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

        public void SendEventMessage(string type, DateTime eventDate, ISocketMessageChannel reminderChannel, string className, string name, string description)
        {
            Program.SendMessage(reminderChannel.Id, new EmbedBuilder()
                .WithTitle($"{char.ToUpper(type[0]) + type.Substring(1)} in {(eventDate - DateTime.Now).Days + 1} days!")
                .AddField(new EmbedFieldBuilder()
                    .WithName("Name:")
                    .WithValue(name)
                )
                .AddField(new EmbedFieldBuilder()
                    .WithName("Description:")
                    .WithValue(description)
                )
                .AddField(new EmbedFieldBuilder()
                    .WithName("Class:")
                    .WithValue(className)
                )
                .AddField(new EmbedFieldBuilder()
                    .WithName("Date:")
                    .WithValue(eventDate.ToShortDateString())
                )
                .WithColor(Color.DarkGreen)
                .WithFooter("Created by Jai", null)
                .WithTimestamp(DateTimeOffset.Now));
        } 
    }
}