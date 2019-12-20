using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace super_cactus.Modules
{
    public class Help : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _service;

        public Help(CommandService service)
        {
            _service = service;
        }

        [Command("help")]
        [Summary("A help command to show every command in the bot")]
        public async Task HelpAsync()
        {
            var prefix = "!";

            var builder = new EmbedBuilder()
            {
                Color = Color.Green,
                Title = "__Commands__",
                Description = "The list of all the commands on the Super Cactus Bot!\n  \n Required arguments: []\nOptional arguments: <>",
            };
            builder.WithFooter($"For more information about a command (such as aliases), do {prefix}help <command>.");

            foreach (var module in _service.Modules)
            {
                string description = null;
                foreach (var cmd in module.Commands)
                {
                    if (!string.IsNullOrEmpty(module.Group))
                        description +=  $"**{prefix}{module.Group} {cmd.Name}**";
                    else
                        description +=  $"**{prefix}{cmd.Name}**";


                    if (cmd.Parameters.Count > 0)
                    {
                        description += " ";

                        foreach (var param in cmd.Parameters)
                        {
                            if (param.IsOptional)
                                description += $"<{param.Name}>";
                            else
                                description += $"[{param.Name}]";
                            description += " ";
                        }
                    }

                    description += "\n";

                    if (cmd.Summary != null)
                        description += cmd.Summary + "\n";
                    
                    description += "\n";
                }

                if (!string.IsNullOrWhiteSpace(description))
                {
                    builder.AddField(x =>
                    {
                        x.Name = char.ToUpper(module.Name[0]) + module.Name.Substring(1);
                        x.Value = description;
                        x.IsInline = false;
                    });
                }
            }

            await ReplyAsync("", false, builder.Build());
        }
        
        
        [Command("help"), Alias("commands", "cmds")]
        [Summary("Gets a command's information")]
        public async Task HelpAsync(string command)
        {
            var result = _service.Search(command.ToLower());

            if (!result.IsSuccess)
            {
                var error = new EmbedBuilder()
                    .WithTitle("Error")
                    .WithDescription("Command Not Found!")
                    .WithColor(Color.Red);
                
                await ReplyAsync("", embed: error.Build());
                return;
            }

            var prefix = "!";
            var embed = new EmbedBuilder
            {
                Color = new Color(42, 141, 222),
                Title = "Command Information",
                Description = $"Information about the command \"**{command}**\":"
            };

            foreach (var match in result.Commands)
            {
                var cmd = match.Command;
                var aliasesList = cmd.Aliases.ToList();

                if (aliasesList.Any())
                {
                    aliasesList.RemoveAt(0);
                    var aliasesListPrefixes = aliasesList.Select(alias => $"~{alias}").ToList();
                    aliasesList = aliasesListPrefixes;
                }
                else
                    aliasesList.Add("(None)");

                var aliases = aliasesList.ToArray();

                var paramList = cmd.Parameters.ToList();
                string parameters;

                if (paramList.Any())
                {
                    parameters = string.Join(", ", cmd.Parameters.Select(p => $"{p.Name} ({p.Type.Name})"));
                }
                else
                    parameters = "(None)";

                embed.AddField(x =>
                {
                    x.Name = prefix + cmd.Name;
                    x.Value = $"Aliases: {string.Join(", ", aliases)}\n" +
                              $"Parameters: {parameters}\n" +
                              $"Description: {cmd.Summary}";
                    x.IsInline = false;
                });
            }

            await ReplyAsync("", embed: embed.Build());
        }
    }
}