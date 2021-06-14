using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Addons.Interactive;
using Botmoww.AdminCommands;
using Microsoft.Extensions.DependencyInjection;

namespace Botmoww
{
    abstract class Botlatenci
    {
        public abstract int Latency { get; protected set; }
    }
    public static class Globals
    {
        public static string botStatus;
        public static string botPrefix = "!";
        public static Random random = new Random();
        public static List<string> varList = new List<string>();
        public static List<ulong> blackList = new List<ulong>();
        public static string userErrorReason;
        public static IMessageChannel modLogChannel;
        public static int tipChance;
        public static int modCase;
        public static string time = $"{DateTime.UtcNow.Hour}:{DateTime.UtcNow.Minute}:{DateTime.UtcNow.Second}";
        public static string date = $"{DateTime.UtcNow.Day}/{DateTime.UtcNow.Month}/{DateTime.UtcNow.Year}";
        public static List<ulong> banList = new List<ulong>();
        public static List<string> filterList = new List<string>() {"assmow"};
        public static IMessageChannel swearChannel;
    }

    class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private DiscordSocketClient client;
        private CommandService commands;
        private IServiceProvider services;
        public async Task RunBotAsync()
        {
            client = new DiscordSocketClient();
            commands = new CommandService();
            services = new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton(commands)
                .AddSingleton<InteractiveService>()
                .BuildServiceProvider();

            string token = "NzQzNDY0MzYwMTQzMjI0ODkz.XzVDJA.xWw44rI7HpxKVdJQR2GorFJGtkY";
            client.Log += _client_Log;
            await RegisterCommand();
            await client.LoginAsync(TokenType.Bot, token); Globals.botStatus = "Online";
            await client.StartAsync();
            await Task.Delay(-1);

        }
        private Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }
        public int BotLatency()
        {
            return client.Latency;
        }
        public async Task WelcomeUser(SocketGuildUser newUser)
        {
            if (Globals.banList.Contains(newUser.Id)) 
            {
                await newUser.Guild.AddBanAsync(newUser);
                await Globals.modLogChannel.SendMessageAsync($">>> **{Globals.modCase}** | {Globals.date} | {Globals.time} \n:hammer: Banned user **{newUser}** tried joining and was banned \n:clock4: Length: **Permanent** \n:envelope: Reason: **---** \n:bust_in_silhouette: ID: **{newUser.Id}**"); 
            }
        }
        private async Task MessageFilter(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(client, message);

            if (message.Channel.Id != 840968965395972146)
            {
                if (!message.Content.StartsWith("!say"))
                {
                    await client.GetGuild(646960091332870155).GetTextChannel(840968965395972146).SendMessageAsync($"**{message.Channel.Name}** | **{message.Author.Id}** | **{message.Author}** {message.Content}");
                }
            }

            if (message.Content.Contains("don't fucking swear") & message.Author.Id == 743464360143224893) { Thread.Sleep(2000); await context.Message.DeleteAsync(); }
            if (message.Author.IsBot) return;            
            if (message == null) return;
            if (Globals.blackList.Contains(message.Author.Id)) return;

            if (message.Content.StartsWith("<@") && message.Content.Contains("298816368231448588") && message.Author.Id != 743464360143224893)
            {
                await context.Channel.SendMessageAsync(context.Message.Author.Mention + " ur kinda sussy baka"); 
                await context.Message.DeleteAsync(); 
            }

            if (message.Content.StartsWith("<@") && message.Content.Contains("743464360143224893") && message.Author.Id != 743464360143224893)
            {
                await context.Channel.SendMessageAsync(context.Message.Author.Mention + " piss off mate");
                await context.Message.DeleteAsync();
            }

            if (Globals.filterList.Any(message.Content.ToLower().Replace(" ","").Contains)) 
            { 
                await context.Channel.SendMessageAsync("don't fucking swear");
                await context.Message.DeleteAsync();
                await client.GetGuild(646960091332870155).GetTextChannel(840964001768407041).SendMessageAsync($"**{message.Author}** {message.Content}");
            }

            if (message.Content.ToLower().Contains("nig") || message.Content.ToLower().Contains("n word"))
            {
                await context.Channel.SendMessageAsync("https://tenor.com/view/bruh-n-word-meme-admin-gif-17665471");
                await context.Message.DeleteAsync();
            }

            if (message.Content.ToLower().Contains("discord.gg"))
            {
                await context.Channel.SendMessageAsync("stfu mate literally no one cares");
                await context.Message.DeleteAsync();
            }
        }

        private async Task MessageLogs(IMessage message, ISocketMessageChannel channel)
        {

        }

        public async Task RegisterCommand() 
        {
            client.MessageReceived += HandleCommand;
            client.MessageReceived += MessageFilter;
            client.UserJoined += WelcomeUser;
            //client.MessageDeleted += MessageLogs;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
        }
        private async Task HandleCommand(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(client, message);
            //if (message.Author.IsBot) return;
            if (Globals.blackList.Contains(message.Author.Id)) return;
            int argPos = 0; if (message.HasStringPrefix(Globals.botPrefix, ref argPos))
            {
                var result = await commands.ExecuteAsync(context, argPos, services);
                if (!result.IsSuccess)
                {
                    Console.WriteLine(result.ErrorReason);
                    if (!result.IsSuccess & result.ErrorReason == "User not found." & message.Content.StartsWith($"{Globals.botPrefix}blacklist")) await message.Channel.SendMessageAsync($">>> User not found! :red_car: \nTry using {Globals.botPrefix}blacklistid instead!");
                    if (!result.IsSuccess & result.ErrorReason == "User not found." & message.Content.StartsWith($"{Globals.botPrefix}whitelist")) await message.Channel.SendMessageAsync($">>> User not found! :red_car: \nTry using {Globals.botPrefix}whitelistid instead!");
                    if (!result.IsSuccess & result.ErrorReason == "User not found." & message.Content.StartsWith($"{Globals.botPrefix}ban")) { var tempMessage = message.Content; tempMessage = tempMessage.Substring(Globals.botPrefix.Length+4); if (tempMessage.Length == 18)
                        { try { ulong banUserId = Convert.ToUInt64(tempMessage); Globals.banList.Add(banUserId); await message.Channel.SendMessageAsync($">>> :hammer: **{banUserId}** has been banned **permanently**! :police_car:"); Globals.modCase += 1;
                                try { await Globals.modLogChannel.SendMessageAsync($">>> **{Globals.modCase}** | {Globals.date} | {Globals.time} \n:hammer: **{message.Author}** banned **{banUserId}** \n:clock4: Length: **Permanent** \n:envelope: Reason: **---** \n:bust_in_silhouette: ID: **{banUserId}**"); } 
                                catch (NullReferenceException) { Globals.tipChance += 1; if (Globals.tipChance > 5) { Thread.Sleep(10); await message.Channel.SendMessageAsync($">>> **Tip**: You can set a moderation log channel by using **{Globals.botPrefix}setchannel** :blue_car:"); Globals.tipChance = 0; } } }
                        catch (FormatException) { await message.Channel.SendMessageAsync($">>> User or ID incorrect! :red_car:"); } } else await message.Channel.SendMessageAsync($">>> User or ID incorrect! :red_car:"); }
                    if (result.Error.Equals(CommandError.UnmetPrecondition)) await message.Channel.SendMessageAsync($">>> {result.ErrorReason} :red_car:");
                }
            }
        }
    }
}