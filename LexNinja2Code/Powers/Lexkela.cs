using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Powers;

public class Lexkela : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "LexKela.png".PowerImagePath();
    public override string? CustomBigIconPath => "LexKela.png".BigPowerImagePath();

    public override bool TryModifyEnergyCostInCombat(
        CardModel card,
        decimal originalCost,
        out decimal modifiedCost
    )
    {
        if (card.Owner.Creature != Owner || !card.Keywords.Contains(NinjaKeyword.Science))
        {
            modifiedCost = originalCost;
            return false;
        }
        modifiedCost = originalCost - Amount;
        return true;
    }

    public override async Task AfterDamageGiven(
        PlayerChoiceContext choiceContext,
        Creature? dealer,
        DamageResult result,
        ValueProp props,
        Creature target,
        CardModel? cardSource
    )
    {
        if (dealer != Owner || cardSource == null)
        {
            return;
        }
        if (cardSource.Tags.Contains(NinjaTags.Holy))
        {
            NinjaAudio.Play("res://LexNinja2/audio/YEEART.mp3", 0.5f);
        }
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (
            cardPlay.Card.Keywords.Contains(NinjaKeyword.Science)
            && cardPlay.Card.Owner == Owner.Player
        )
        {
            await PowerCmd.Apply<Lexkela>(context, Owner, -1, Owner, null);
        }
    }

    private int flag = 0; // 这是干啥的

    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource
    )
    {
        var lexkela = power as Lexkela;
        if (
            Owner.GetPower<Lexkela>() != null
            && power == lexkela
            && amount < 0
            && power.Owner == Owner
        )
        {
            flag = 1;
        }
    }

    public override async Task AfterPlayerTurnStart(
        PlayerChoiceContext choiceContext,
        Player player
    )
    {
        if (player != Owner.Player)
            return;
        flag = 0;
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Side)
            return;
        if (flag == 1)
            return;
        if (Owner.HasPower<Pain>())
            return;
        Flash();
        await PowerCmd.Apply<Lexkela>(choiceContext, Owner, 1, null, null);
    }
}
