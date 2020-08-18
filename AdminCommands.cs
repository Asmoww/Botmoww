using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Addons.Interactive;

namespace Botmoww.AdminCommands
{
    public class AdminCommands : ModuleBase<SocketCommandContext>
    {
        #region ban
        [Command("ban")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task BanUserLengthReason(IGuildUser banUser, int banLenght, [Remainder] string banReason)
        {
            //await Context.Guild.AddBanAsync(banUser, banLenght, banReason); }
            await Context.Message.Channel.SendMessageAsync($">>> :hammer: **{banUser}** has been banned for **{banLenght} days** :police_car:"); Globals.modCase += 1;
            try { await Globals.modLogChannel.SendMessageAsync($">>> **{Globals.modCase}** | {Globals.date} | {Globals.time} \n:hammer: **{Context.Message.Author}** banned **{banUser}** \n:clock4: Length: **{banLenght} days** \n:envelope: Reason: **{banReason}**"); }
            catch (NullReferenceException) { Globals.tipChance += 1; if (Globals.tipChance == 5) { Thread.Sleep(10); await Context.Message.Channel.SendMessageAsync($">>> **Tip**: You can set a moderation log channel by using **{Globals.botPrefix}setchannel** :blue_car:"); Globals.tipChance = 0; } }
        }
        [Command("ban")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task BanUserLength(IGuildUser banUser, int banLenght)
        {
            //await Context.Guild.AddBanAsync(banUser, banLenght); }
            await Context.Message.Channel.SendMessageAsync($">>> :hammer: **{banUser}** has been banned for **{banLenght} days** :police_car:"); Globals.modCase += 1;
            try { await Globals.modLogChannel.SendMessageAsync($">>> **{Globals.modCase}** | {Globals.date} | {Globals.time} \n:hammer: **{Context.Message.Author}** banned **{banUser}** \n:clock4: Length: **{banLenght} days** \n:envelope: Reason: **---**"); }
            catch (NullReferenceException) { Globals.tipChance += 1; if (Globals.tipChance == 5) { Thread.Sleep(10); await Context.Message.Channel.SendMessageAsync($">>> **Tip**: You can set a moderation log channel by using **{Globals.botPrefix}setchannel** :blue_car:"); Globals.tipChance = 0; } }
        }
        [Command("ban")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task BanUser(IGuildUser banUser)
        {
            //await Context.Guild.AddBanAsync(banUser); }
            await Context.Message.Channel.SendMessageAsync($">>> :hammer: **{banUser}** has been banned **permanently** :police_car:"); Globals.modCase += 1;
            try { await Globals.modLogChannel.SendMessageAsync($">>> **{Globals.modCase}** | {Globals.date} | {Globals.time} \n:hammer: **{Context.Message.Author}** banned **{banUser}** \n:clock4: Length: **Permanent** \n:envelope: Reason: **---**"); }
            catch (NullReferenceException) { Globals.tipChance += 1; if (Globals.tipChance == 5) { Thread.Sleep(10); await Context.Message.Channel.SendMessageAsync($">>> **Tip**: You can set a moderation log channel by using **{Globals.botPrefix}setchannel** :blue_car:"); Globals.tipChance = 0; } }
        }
        [Command("ban")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task BanUserReason(IGuildUser banUser, [Remainder] string banReason)
        {
            //await Context.Guild.AddBanAsync(banUser, banReason); }
            await Context.Message.Channel.SendMessageAsync($">>> :hammer: **{banUser}** has been banned **permanently** :police_car:"); Globals.modCase += 1;
            try { await Globals.modLogChannel.SendMessageAsync($">>> **{Globals.modCase}** | {Globals.date} | {Globals.time} \n:hammer: **{Context.Message.Author}** banned **{banUser}** \n:clock4: Length: **Permanent** \n:envelope: Reason: **{banReason}**"); }
            catch (NullReferenceException) { Globals.tipChance += 1; if (Globals.tipChance == 5) { Thread.Sleep(10); await Context.Message.Channel.SendMessageAsync($">>> **Tip**: You can set a moderation log channel by using **{Globals.botPrefix}setchannel** :blue_car:"); Globals.tipChance = 0; } }
        }
        #endregion

        #region banid

        #endregion

        #region unban
        [Command("unban")]
        public async Task Unban(IGuildUser unBanUser)
        {
            await Context.Guild.RemoveBanAsync(unBanUser);

        }

        #endregion

        #region kick

        #endregion

        #region mute 

        #endregion

        #region unmute

        #endregion

        #region strike

        #endregion

        #region pardon

        #endregion
    }
    public class InterAdminCommands : InteractiveBase
    {
        #region prefix
        [Command("prefix", RunMode = RunMode.Async)]
        public async Task Prefix()
        {
            await Context.Channel.SendMessageAsync(">>> Enter the new prefix:");
            var response = await NextMessageAsync(); if (response != null)
            {
                if (response.Content.Length > 6) { await ReplyAsync(">>> Prefix must be 6 characters or shorter! :red_car:"); }
                else { await ReplyAsync($">>> New prefix: **{response.Content}**"); Globals.botPrefix = response.Content; }
            }
            else await ReplyAsync("Timeout");
        }
        #endregion

        #region setchannel
        [Command("setchannel")]
        public async Task SetChannel(IMessageChannel modlogchannel)
        {
            Globals.modLogChannel = modlogchannel;
            await Context.Message.Channel.SendMessageAsync($">>> New moderation log channel set! :blue_car: \n<#{Globals.modLogChannel.Id}>");
        }
        #endregion
    }
}
