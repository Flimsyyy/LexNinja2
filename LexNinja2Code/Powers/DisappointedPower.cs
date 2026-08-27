using System.Threading.Tasks;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Powers;

public class DisappointedPower : LexNinja2Power
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override object InitInternalData() => new Data();

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type != CardType.Attack || cardPlay.Card.Owner != Owner.Player)
        {
            return;
        }
        var internalData = GetInternalData<Data>();
        if (internalData.CardPlayed == null || internalData.CardPlayed != cardPlay.Card)
        {
            return;
        }

        internalData.CardPlayed = null;
        var weakSelf = await PowerCmd.Apply<WeakPower>(context, Owner, 1, null, null);
        weakSelf!.SkipNextDurationTick = false;
        await PowerCmd.Decrement(this);
    }

    public override Task BeforeAttack(AttackCommand command)
    {
        if (
            command.ModelSource is not CardModel modelSource
            || modelSource.Owner.Creature != Owner
            || modelSource.Type != CardType.Attack
            || !command.DamageProps.IsPoweredAttack()
        )
            return Task.CompletedTask;
        var internalData = GetInternalData<Data>();
        if (internalData.CardPlayed != null)
            return Task.CompletedTask;
        internalData.CardPlayed = (CardModel)command.ModelSource;
        return Task.CompletedTask;
    }

    public override decimal ModifyDamageMultiplicative(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource
    )
    {
        if (cardSource == null || cardSource.Owner.Creature != Owner || !props.IsPoweredAttack())
            return 1;
        var internalData = GetInternalData<Data>();
        return internalData.CardPlayed != null && cardSource != internalData.CardPlayed ? 1 : 3;
    }

    private class Data
    {
        public CardModel? CardPlayed;
    }

    public override string CustomIconPath => "DisappointedPower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "DisappointedPower84.png".BigPowerImagePath();
}
