using System.Collections.Generic;
using System.Linq;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

#nullable enable

namespace ImbaManagerPlugin;
public class ImbaManagerPlugin : BasePlugin
{
    private string prefix = "IMBAv1";
    private Dictionary<ulong, string> pendingNames = new Dictionary<ulong, string>()
    {
        { 76561198031632056, "barracode" },
    };

    public override string ModuleName => "ImbaManagerPlugin";
    public override string ModuleVersion => "0.0.2";
    public override string ModuleAuthor => "barracode";
    public override string ModuleDescription => "ImbaManagerPlugin";

    void PrintToAll(string message) {
        foreach (var player in Utilities.GetPlayers().Where(x => x.IsValid)) {
            player.PrintToChat($" {ChatColors.Green}[{prefix}] {ChatColors.Default}{message}");
        }
    }

    [ConsoleCommand("imba_say", "Say a message to all players")]
    [CommandHelper(minArgs: 1, usage: "[text]", whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void OnCommandReplySayToAll(CCSPlayerController? player, CommandInfo command) {
        var text = command.GetArg(1);

        PrintToAll(text);
    }

    [ConsoleCommand("imba_set_prefix", "Set the prefix for the plugin")]
    [CommandHelper(minArgs: 1, usage: "[prefix]", whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void OnCommandReplySetPrefix(CCSPlayerController? player, CommandInfo command) {
        var text = command.GetArg(1);

        prefix = text;
        command.ReplyToCommand($"Prefix set to {text}");
    }

    [GameEventHandler]
public HookResult OnPlayerConnect(EventPlayerConnect @event, GameEventInfo info)
{
    // Userid will give you a reference to a CCSPlayerController class.
    // Before accessing any of its fields, you must first check if the Userid
    // handle is actually valid, otherwise you may run into runtime exceptions.
    // See the documentation section on Referencing Players for details.
    if (@event.Userid.IsValid) {
        if (pendingNames.ContainsKey(@event.Userid.SteamID)) {
            var name = pendingNames[@event.Userid.SteamID];
            @event.Userid.PlayerName = name;
        }
    }

    return HookResult.Continue;
}
}
