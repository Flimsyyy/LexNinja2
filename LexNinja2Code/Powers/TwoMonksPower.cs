using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

namespace LexNinja2.LexNinja2Code.Powers;

public class TwoMonksPower : LexNinja2Power
{
    private bool _isEffective;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomIconPath => "TwoMonksPower.png".PowerImagePath();
    public override string? CustomBigIconPath => "TwoMonksPower.png".BigPowerImagePath();

    public override bool ShouldTakeExtraTurn(Player player)
    {
        return player == Owner.Player;
    }

    public override Task AfterTakingExtraTurn(Player player)
    {
        if (player != Owner.Player)
        {
            return Task.CompletedTask;
        }
        _isEffective = true;
        return Task.CompletedTask;
    }

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> creatures
    )
    {
        if (side != Owner.Side)
        {
            return;
        }
        if (_isEffective)
        {
            await PowerCmd.Decrement(this);
        }
    }

    public override async Task AfterAutoPrePlayPhaseEnteredLate(
        PlayerChoiceContext choiceContext,
        Player player
    )
    {
        if (player != Owner.Player)
        {
            return;
        }
        var combatState = player.Creature.CombatState;
        Flash();
        using (CardSelectCmd.PushSelector(new VakuuCardSelector()))
        {
            int cardsPlayed;
            for (cardsPlayed = 0; cardsPlayed < 30; cardsPlayed++)
            {
                if (CombatManager.Instance.IsOverOrEnding)
                {
                    break;
                }
                if (CombatManager.Instance.IsPlayerReadyToEndTurn(player))
                {
                    break;
                }
                var pile = PileType.Hand.GetPile(Owner.Player);
                var card = pile.Cards.FirstOrDefault(c => c.CanPlay());
                if (card == null)
                {
                    break;
                }
                var target = GetTarget(card, combatState!);
                await card.SpendResources();
                await CardCmd.AutoPlay(choiceContext, card, target, skipXCapture: true);
            }
            if (cardsPlayed == 0)
            {
                return;
            }
            PlayerCmd.EndTurn(Owner.Player, false);
        }
    }

    private Creature? GetTarget(CardModel card, ICombatState combatState)
    {
        var combatTargets = Owner.Player!.RunState.Rng.CombatTargets;
        return card.TargetType switch
        {
            TargetType.AnyEnemy => combatState.HittableEnemies.ToList().FirstOrDefault(),
            TargetType.AnyAlly => combatTargets.NextItem(
                combatState.Allies.Where(c => c is { IsAlive: true, IsPlayer: true } && c != Owner)
            ),
            TargetType.AnyPlayer => Owner,
            _ => null,
        };
    }
}
