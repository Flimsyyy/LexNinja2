using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Powers;

public class WePeacePower : CustomPowerModel
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override string CustomPackedIconPath => "power.png".PowerImagePath();
    public override string? CustomBigIconPath => "power.png".BigPowerImagePath();

    // public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    // {
    //     foreach (CardModel card in Owner.Player.PlayerCombatState.AllCards.Where<CardModel>((Func<CardModel, bool>) (c => c.Type == CardType.Attack)))
    //     {
    //         await CardCmd.Afflict<Entangled>(card, 1M);
    //     }
    // }
    //
    // public override async Task AfterCardEnteredCombat(CardModel card)
    // {
    //     if (card.Owner != this.Owner.Player || card.Affliction != null || card.Type != CardType.Attack)
    //         return;
    //     await CardCmd.Afflict<Entangled>(card, 1M);
    // }

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> creatures
    )
    {
        if (side != Owner.Side)
            return;
        Flash();
        await PowerCmd.Remove(this);
    }

    // public override Task AfterRemoved(Creature oldOwner)
    // {
    //     foreach (CardModel card in oldOwner.Player.PlayerCombatState.AllCards.Where<CardModel>((Func<CardModel, bool>) (c => c.Affliction is Entangled)))
    //         CardCmd.ClearAffliction(card);
    //     return Task.CompletedTask;
    // }

    public override bool ShouldPlay(CardModel card, AutoPlayType _)
    {
        return card.Owner != Owner.Player || card.Type != CardType.Attack;
    }
}
