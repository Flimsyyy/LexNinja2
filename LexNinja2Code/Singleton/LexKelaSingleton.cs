using System.Collections.Generic;
using System.Threading.Tasks;
using LexNinja2.LexNinja2Code.Api;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Combat.SecondaryResources;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models;

namespace LexNinja2.LexNinja2Code.Singleton;

[RegisterSingleton]
public class LexKelaSingleton()
    : HookedSingletonModel(HookType.Combat),
        ISecondaryResourceHookListener
{
    private readonly Dictionary<Player, bool> _isActive = new();
    private readonly Dictionary<Player, bool> _usedLexKela = new();

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        _usedLexKela[player] = false;
        if (_isActive.ContainsKey(player))
            return Task.CompletedTask;
        _isActive[player] = player.Character is Character.LexNinja2;
        return Task.CompletedTask;
    }

    public Task AfterSecondaryResourceChanged(SecondaryResourceChangeContext context)
    {
        if (context.Definition.Id != LexKela.Id)
        {
            return Task.CompletedTask;
        }
        var player = context.Player;
        _isActive[player] = true;
        if (context.Delta < 0)
        {
            _usedLexKela[context.Player] = true;
        }
        return Task.CompletedTask;
    }

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants
    )
    {
        if (side != CombatSide.Player)
        {
            return;
        }

        foreach (var player in CurrentCombatState!.Players)
        {
            if (_usedLexKela[player])
            {
                _usedLexKela[player] = false;
                continue;
            }
            if (_isActive[player])
            {
                await LexKela.Gain(player, 1, this);
            }
        }
    }
}
