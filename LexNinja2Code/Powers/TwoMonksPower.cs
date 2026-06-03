using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Random;

namespace LexNinja2.LexNinja2Code.Powers;

public class TwoMonksPower : CustomPowerModel
{
    private bool _wasOwnerPartOfLastPlayerTurn = true;
    private bool _isEffective = true;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "TwoMonksPower.png".PowerImagePath();
    public override string? CustomBigIconPath => "TwoMonksPower.png".BigPowerImagePath();

    private bool WasOwnerPartOfLastPlayerTurn
    {
        get => _wasOwnerPartOfLastPlayerTurn;
        set
        {
            AssertMutable();
            _wasOwnerPartOfLastPlayerTurn = value;
        }
    }

    public override bool ShouldTakeExtraTurn(Player player)
    {
        return WasOwnerPartOfLastPlayerTurn && player == Owner.Player && _isEffective;
    }

    public override Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> creatures,
        ICombatState combatState
    )
    {
        if (side != Owner.Side)
            return Task.CompletedTask;
        if (Owner.Player != null)
            WasOwnerPartOfLastPlayerTurn = CombatManager.Instance.IsPartOfPlayerTurn(Owner.Player);
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
        await PowerCmd.Decrement(this);
        if (this.Amount >= 1)
        {
            return;
        }
        _isEffective = false;
    }

    public override async Task AfterAutoPrePlayPhaseEnteredLate(
        PlayerChoiceContext choiceContext,
        Player player
    )
    {
        if (player != base.Owner.Player)
        {
            return;
        }
        ICombatState combatState = player.Creature.CombatState;
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
                CardPile pile = PileType.Hand.GetPile(base.Owner.Player);
                CardModel card = pile.Cards.FirstOrDefault((CardModel c) => c.CanPlay());
                if (card == null)
                {
                    break;
                }
                Creature target = GetTarget(card, combatState);
                await card.SpendResources();
                await CardCmd.AutoPlay(
                    choiceContext,
                    card,
                    target,
                    AutoPlayType.Default,
                    skipXCapture: true
                );
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
        Rng combatTargets = base.Owner.Player.RunState.Rng.CombatTargets;
        return card.TargetType switch
        {
            TargetType.AnyEnemy => combatState.HittableEnemies.FirstOrDefault(),
            TargetType.AnyAlly => combatTargets.NextItem(
                combatState.Allies.Where(
                    (Creature c) => c != null && c.IsAlive && c.IsPlayer && c != base.Owner
                )
            ),
            TargetType.AnyPlayer => base.Owner,
            _ => null,
        };
    }
}
