using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Discord.Addons.Interactive;
namespace Botmoww
{
    abstract class Botlatenci
    {
        public abstract int Latency { get; protected set; }
    }
    class BotLatency : Botlatenci
    {       
        public override int Latency { get; protected set; }
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

            string token = "NzQzNDY0MzYwMTQzMjI0ODkz.XzVDJA.xj-zk1VVeVrYr_O3luLBaLypjJ0";
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
        private async Task MessageFilter(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(client, message);
            if (message.Author.IsBot) return; if (message == null) return;
            if (Globals.blackList.Contains(message.Author.Id)) return;
            if (message.Content.StartsWith("<@") && message.Content.Contains("298816368231448588"))
                await context.Channel.SendMessageAsync(context.Message.Author.Mention + " :ping_pong:");
        }
        public async Task RegisterCommand()
        {
            client.MessageReceived += HandleCommand;
            client.MessageReceived += MessageFilter;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
        }
        private async Task HandleCommand(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(client, message);
            if (message.Author.IsBot) return;
            if (Globals.blackList.Contains(message.Author.Id)) return;
            int argPos = 0; if (message.HasStringPrefix(Globals.botPrefix, ref argPos))
            {
                var result = await commands.ExecuteAsync(context, argPos, services);
                if (!result.IsSuccess)
                {
                    Console.WriteLine(result.ErrorReason);
                    if (!result.IsSuccess & result.ErrorReason == "User not found." & message.Content.StartsWith($"{Globals.botPrefix}blacklist")) await message.Channel.SendMessageAsync($">>> User not found! :red_car: \nTry using {Globals.botPrefix}blacklistid instead!");
                    if (!result.IsSuccess & result.ErrorReason == "User not found." & message.Content.StartsWith($"{Globals.botPrefix}whitelist")) await message.Channel.SendMessageAsync($">>> User not found! :red_car: \nTry using {Globals.botPrefix}whitelistid instead!");
                    if (result.Error.Equals(CommandError.UnmetPrecondition)) await message.Channel.SendMessageAsync($">>> {result.ErrorReason} :red_car:");
                }
            }
        }
    }
}