using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading;
using Discord.Commands.Builders;
using Discord.Addons.Interactive;
using System.Collections;
namespace Botmoww.UserCommands
{
    public class UserCommands : ModuleBase<SocketCommandContext>
    {
        Program p = new Program();

        #region status
        [Command("status")]
        public async Task Status()
        {
            await Context.Channel.SendMessageAsync($">>> {Context.Message.Author.Mention} :ping_pong: \n**Status**: {Globals.botStatus} \n**Latency**: {p.BotLatency()} Milliseconds");
        }
        #endregion

        #region trash
        [Command("trash")]
        public async Task Trash(string varUser)
        {
            string trashEmoji = "";
            int varNum = Globals.random.Next(0, 100);
            if (Globals.varList.Contains(varUser.ToLower() + "+")) varNum = 0;
            if (Globals.varList.Contains(varUser.ToLower() + "-")) varNum = 100;
            if (varNum <= 33 & varNum > 0) trashEmoji = ":thinking:";
            else if (varNum <= 66 & varNum > 33) trashEmoji = ":wastebasket:";
            else if (varNum == 0) trashEmoji = ":sunglasses:"; else trashEmoji = ":sick::face_vomiting:";
            await Context.Channel.SendMessageAsync($">>> **{varUser}** is {varNum}% trash {trashEmoji}");
        }
        [Command("trash")]
        public async Task TrashAuthor()
        {
            string trashEmoji = "";
            int varNum = Globals.random.Next(0, 100);
            if (Context.Message.Author.Id == 298816368231448588) varNum = 0;
            if (varNum <= 33 & varNum > 0) trashEmoji = ":thinking:";
            else if (varNum <= 66 & varNum > 33) trashEmoji = ":wastebasket:";
            else if (varNum == 0) trashEmoji = ":sunglasses:"; else trashEmoji = ":sick::face_vomiting:";
            await Context.Channel.SendMessageAsync($">>> **{Context.Message.Author.Username}** is {varNum}% trash {trashEmoji}");
        }
        #endregion

        #region gei
        [Command("gei")]
        public async Task Gei(string varUser)
        {
            string geiEmoji = "";
            int varNum = Globals.random.Next(0, 100);
            if (Globals.varList.Contains(varUser.ToLower() + "+")) varNum = 0;
            if (Globals.varList.Contains(varUser.ToLower() + "-")) varNum = 100;
            if (varNum <= 33 & varNum > 0) geiEmoji = ":thinking:";
            else if (varNum <= 66 & varNum > 33) geiEmoji = ":flushed:";
            else if (varNum == 0) geiEmoji = ":sunglasses:"; else geiEmoji = ":rainbow_flag:";
            await Context.Channel.SendMessageAsync($">>> **{varUser}** is {varNum}% gei {geiEmoji}");
        }
        [Command("gei")]
        public async Task GeiAuthor()
        {
            string geiEmoji = "";
            int varNum = Globals.random.Next(0, 100);
            if (Context.Message.Author.Id == 298816368231448588) varNum = 0;
            if (varNum <= 33 & varNum > 0) geiEmoji = ":thinking:";
            else if (varNum <= 66 & varNum > 33) geiEmoji = ":flushed:";
            else if (varNum == 0) geiEmoji = ":sunglasses:"; else geiEmoji = ":rainbow_flag:";
            await Context.Channel.SendMessageAsync($">>> **{Context.Message.Author.Username}** is {varNum}% gei {geiEmoji}");
        }
        #endregion

        #region whitelist       

        [Command("whitelist")]
        public async Task WhiteList(IGuildUser whiteListUser)
        {
            if (Globals.blackList.Contains(whiteListUser.Id))
            {
                await Context.Channel.SendMessageAsync($">>> {whiteListUser.Username} was whitelisted! :police_car: \n**User**: {whiteListUser} \n**ID**: {whiteListUser.Id}");
                Globals.blackList.Remove(whiteListUser.Id);
            }
            else await Context.Channel.SendMessageAsync($">>> {whiteListUser.Username} is already whitelisted! :red_car: \n**User**: {whiteListUser} \n**ID**: {whiteListUser.Id}");
        }
        [Command("whitelistid")]
        public async Task IdWhiteList(string whiteListIdInput)
        {
            ulong whiteListId;
            if (ulong.TryParse(whiteListIdInput, out whiteListId))
            {
                if (whiteListId > 1000000000000000000 || whiteListId < 100000000000000000) await Context.Channel.SendMessageAsync(">>> An ID should contain 18 numbers! :red_car:");
                else if (Globals.blackList.Contains(whiteListId))
                {
                    await Context.Channel.SendMessageAsync($">>> ID was whitelisted! :police_car: \n**ID**: {whiteListId}");
                    Globals.blackList.Remove(whiteListId);
                }
                else await Context.Channel.SendMessageAsync($">>> ID is already whitelisted! :red_car: \n**ID**: {whiteListId}");
            }
            else await Context.Channel.SendMessageAsync(">>> An ID should contain 18 numbers! :red_car:");
        }
        #endregion

        #region blacklist        
        [Command("blacklist")]
        public async Task BlacklistUser(IGuildUser blackListUser)
        {                   
                if (Globals.blackList.Contains(blackListUser.Id))
                {
                    await Context.Channel.SendMessageAsync($">>> {blackListUser.Username} is already blacklisted! :red_car: \n**User**: {blackListUser} \n**ID**: {blackListUser.Id} ");
                }
                else
                {
                    await Context.Channel.SendMessageAsync($">>> {blackListUser.Username} was blacklisted! :police_car: \n**User**: {blackListUser} \n**ID**: {blackListUser.Id}");
                    Globals.blackList.Add(blackListUser.Id);
                }        
        }
        [Command("blacklistid")]
        public async Task BlacklistId(string blackListIdInput)
        {
            ulong blackListId;
            if (ulong.TryParse(blackListIdInput, out blackListId))
            {
                if (blackListId > 1000000000000000000 || blackListId < 100000000000000000) await Context.Channel.SendMessageAsync(">>> An ID should contain 18 numbers! :red_car:");
                else if (Globals.blackList.Contains(blackListId)) await Context.Channel.SendMessageAsync($">>> ID is already blacklisted! :red_car: \n**ID**: {blackListId}");
                else { await Context.Channel.SendMessageAsync($">>> ID was blacklisted! :police_car: \n**ID**: {blackListId}");
                Globals.blackList.Add(blackListId); }
            }
            else await Context.Channel.SendMessageAsync(">>> An ID should contain 18 numbers! :red_car:");
        }
        [Command("blacklist")]
        public async Task BlackList()
        {
            await Context.Channel.SendMessageAsync($">>> **Blacklist** | **{Globals.blackList.Count}** \n{string.Join("\n", Globals.blackList)}");
        }      
        #endregion

        #region addvar
        [Command("addvar")]
        public async Task AddVar(string varWord)
        {
            if (varWord.EndsWith("-") || varWord.EndsWith("+"))
            {
                if (varWord.Length >= 30) await Context.Channel.SendMessageAsync($">>> The word cannot be over 30 characters! :red_car:");
                else
                {
                    if (Globals.varList.Contains(varWord.TrimEnd('-', '+') + "+") || Globals.varList.Contains(varWord.TrimEnd('-', '+') + "-"))
                    { await Context.Channel.SendMessageAsync($">>> The word **{varWord.TrimEnd('-', '+')}** already exists! :red_car:"); }
                    else { await Context.Channel.SendMessageAsync($">>> The word **{varWord}** was added :blue_car:"); Globals.varList.Add(varWord.ToLower()); }
                }
            }
            else await Context.Channel.SendMessageAsync($">>> The word needs to have **+** or **-** as the last character! :red_car:");
        }
        #endregion

        #region removevar 
        [Command("removevar")]
        public async Task RemoveVar(string remVarWord)
        {
            if (Globals.varList.Contains(remVarWord + "+") || Globals.varList.Contains(remVarWord + "-"))
            {
                await Context.Channel.SendMessageAsync($">>> The word **{remVarWord}** was removed :blue_car:"); Globals.varList.Remove(remVarWord.ToLower() + "+"); Globals.varList.Remove(remVarWord.ToLower() + "-");
            }
            else await Context.Channel.SendMessageAsync($">>> The word **{remVarWord}** doesn't exist! :red_car:");
        }
        #endregion

        #region varlist
        [Command("varlist")]
        public async Task VarList()
        {
            await Context.Channel.SendMessageAsync($">>> **Variable list** | **{Globals.varList.Count}** \n{string.Join("\n", Globals.varList)}");
        }
        #endregion

        #region say
        [Command("say")]
        public async Task Say(IMessageChannel channel, [Remainder] string message)
        {
            if (message.EndsWith("!a"))
            {
                channel.SendMessageAsync($"{message.Substring(0, message.Length - 3)}");
            }
            else
            {
                channel.SendMessageAsync($"**{Context.Message.Author}:** {message}");
            }
        }
        #endregion

        #region dm
        [Command("dm")]
        public async Task Dm(IGuildUser user, [Remainder] string message)
        {
            IDMChannel channel = (IDMChannel)user.GetOrCreateDMChannelAsync();
            channel.SendMessageAsync(message);
        }
        #endregion

        #region ping
        [Command("ping")]
        public async Task Ping(IGuildUser user, int times)
        {
            if (times > 5)
            {
                await Context.Channel.SendMessageAsync("dont abuse it shitass");
            }
            else
            {
                for (int i = 0; i < times; i++)
                {
                    await Context.Channel.SendMessageAsync(user.Mention);
                }
            }
        }
        #endregion

    }
    public class InterUserCommands : InteractiveBase
    {
        
    }
}

