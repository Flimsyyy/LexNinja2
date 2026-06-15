using BaseLib.Abstracts;
using BaseLib.Extensions;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Cards.Uncommons;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Powers;

public class BladeDefencePower : CustomPowerModel, IHasSecondAmount
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "ParryPower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "ParryPower84.png".BigPowerImagePath();

    protected override IEnumerable<DynamicVar> CanonicalVars => [new LexKelaVar(0)];

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants
    )
    {
        if (side == Owner.Side)
            return;
        await PowerCmd.Remove(this);
    }

    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource
    )
    {
        if (
            target != Owner
            || !props.IsPoweredAttack()
            || dealer == null
            || !result.WasFullyBlocked
        )
            return;
        NinjaAudio.Play("res://LexNinja2/audio/BladeDefence.mp3");
        await PowerCmd.Apply<Lexkela>(
            choiceContext,
            Owner,
            DynamicVars.LexKela().BaseValue,
            Owner,
            null
        );
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
        if (
            dealer == null
            || dealer != Owner && dealer.PetOwner?.Creature != Owner
            || !props.IsPoweredAttack()
            || result.TotalDamage <= 0
        )
            return;
        await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
    }

    public string GetSecondAmount() => $"{DynamicVars.LexKela().IntValue}";

    public void UpgradeLexKelaValue(decimal addend)
    {
        DynamicVars.LexKela().UpgradeValueBy(addend);
        this.InvokeSecondAmountChanged();
    }

    public override Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource
    )
    {
        if (power is not BladeDefencePower)
        {
            return Task.CompletedTask;
        }
        if (cardSource is BladeDefence)
        {
            UpgradeLexKelaValue(1);
            return Task.CompletedTask;
        }

        var ratio = amount / (Amount - amount);
        UpgradeLexKelaValue(ratio * DynamicVars.LexKela().BaseValue);
        return Task.CompletedTask;
    }
}
