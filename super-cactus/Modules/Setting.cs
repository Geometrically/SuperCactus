using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace super_cactus.Modules
{
    [Group("setting")]
    public class Setting : ModuleBase<SocketCommandContext>
    {
        [RequireOwner]
        [Command("calendar_channel")]
        [Summary("Sets the channel where calendar messages are displayed")]
        public async Task CalendarChannelAsync(IChannel channel)
        {
            var builder = new EmbedBuilder()
                .WithTitle("Calendar Channel Set")
                .WithDescription($"The calendar channel was set to #{channel}!")
                .WithColor(Color.Green)
                .WithFooter("Created by Jai")
                .WithAuthor(Context.User);

            await ReplyAsync("", false, builder.Build());
        }
        
        [RequireOwner]
        [Command("mod_group")]
        [Summary("Sets role of mods, who can add calendar items")]
        public async Task ModGroupAsync(IRole role)
        {
            var builder = new EmbedBuilder()
                .WithTitle("Mod Role Set")
                .WithDescription($"The mod role was set to {role.Name}!")
                .WithColor(Color.Green)
                .WithFooter("Created by Jai")
                .WithAuthor(Context.User);

            await ReplyAsync("", false, builder.Build());
        }
    }
}