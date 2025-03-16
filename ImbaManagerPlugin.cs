using System;
using System.Collections.Generic;
using CounterStrikeSharp.API.Core;

namespace ImbaManagerPlugin
public class ImbaManagerPlugin : BasePlugin
{
    private string prefix = "[IMBA]";
    private Dictionary<string, string> pendingNames = new Dictionary<string, string>();

    public override void OnEnable()
    {
        RegisterCommand("imba_prefix", OnSetPrefix);
        RegisterCommand("imba_say", OnSayMessage);
        RegisterCommand("imba_set_name", OnSetName);
        Game.OnPlayerConnect += OnPlayerConnect;
    }

    private void OnSetPrefix(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Usage: imba_prefix [prefix]");
            return;
        }
        prefix = args[0];
        Console.WriteLine($"Prefix set to: {prefix}");
    }

    private void OnSayMessage(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Usage: imba_say [text]");
            return;
        }
        string message = string.Join(" ", args);
        Game.Say($"{prefix} {message}");
    }

    private void OnSetName(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: imba_set_name [steamid32] [name]");
            return;
        }
        string steamId = args[0];
        string name = args[1];
        var player = Game.GetPlayerBySteamId(steamId);
        if (player != null)
        {
            player.Name = name;
        }
        else
        {
            pendingNames[steamId] = name;
        }
    }

    private void OnPlayerConnect(Player player)
    {
        if (pendingNames.TryGetValue(player.SteamId, out string name))
        {
            player.Name = name;
            pendingNames.Remove(player.SteamId);
        }
    }
}
