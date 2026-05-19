using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Powers;

public class DoubleDamagePower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public override string CustomPackedIconPath => "DoubleDamagePower.png".PowerImagePath();
    public override string? CustomBigIconPath => "DoubleDamagePower.png".BigPowerImagePath();

    protected override object InitInternalData() => new Data();

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
        if (internalData.CommandToModify != null)
            return Task.CompletedTask;
        internalData.CommandToModify = command;
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
            return 1M;
        var internalData = GetInternalData<Data>();
        return
            internalData.CommandToModify != null
            && cardSource != internalData.CommandToModify.ModelSource
            ? 1
            : 2;
    }

    public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        var power = this;
        var internalData = power.GetInternalData<Data>();
        if (command != internalData.CommandToModify)
            return;
        internalData.CommandToModify = null;
        await PowerCmd.Remove(power);
    }

    private class Data
    {
        public AttackCommand? CommandToModify;
    }
}
