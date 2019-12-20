using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace super_cactus.Modules
{
    public class Ping : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Summary("A testing command for testing purposes")]
        public async Task PingAsync()
        {
            var builder = new EmbedBuilder()
                .WithTitle("Test")
                .WithDescription("This is a testing message")
                .WithColor(Color.Green)
                .WithFooter("Created by Jai")
                .WithAuthor(Context.User);

            await ReplyAsync("", false, builder.Build());
        }
    }
}