using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Extensions;
using MegaCrit.Sts2.Core.Combat;
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

public class DoubleDamagePower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public override string CustomPackedIconPath => "DoubleDamagePower.png".PowerImagePath();
    public override string? CustomBigIconPath => "DoubleDamagePower.png".BigPowerImagePath();

    protected override object InitInternalData() => (object)new DoubleDamagePower.Data();

    public override Task BeforeAttack(AttackCommand command)
    {
        if (
            !(command.ModelSource is CardModel modelSource)
            || modelSource.Owner.Creature != this.Owner
            || modelSource.Type != CardType.Attack
            || !command.DamageProps.IsPoweredAttack()
        )
            return Task.CompletedTask;
        DoubleDamagePower.Data internalData = this.GetInternalData<DoubleDamagePower.Data>();
        if (internalData.commandToModify != null)
            return Task.CompletedTask;
        internalData.commandToModify = command;
        return Task.CompletedTask;
    }

    public override Decimal ModifyDamageMultiplicative(
        Creature? target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource
    )
    {
        if (
            cardSource == null
            || cardSource.Owner.Creature != this.Owner
            || !props.IsPoweredAttack()
        )
            return 1M;
        DoubleDamagePower.Data internalData = this.GetInternalData<DoubleDamagePower.Data>();
        return
            internalData.commandToModify != null
            && cardSource != internalData.commandToModify.ModelSource
            ? 1
            : 2;
    }

    public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        DoubleDamagePower power = this;
        DoubleDamagePower.Data internalData = power.GetInternalData<DoubleDamagePower.Data>();
        if (command != internalData.commandToModify)
            return;
        internalData.commandToModify = (AttackCommand)null;
        await PowerCmd.Remove((PowerModel)power);
    }

    private class Data
    {
        public AttackCommand? commandToModify;
    }
}
