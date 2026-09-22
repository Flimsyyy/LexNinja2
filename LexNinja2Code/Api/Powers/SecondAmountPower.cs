using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using LexNinja2.LexNinja2Code.Api.Interface;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Scaffolding.Content;

namespace LexNinja2.LexNinja2Code.Api.Powers;

public abstract class SecondAmountPower : ModPowerTemplate, IHasSecondAmount
{
    protected abstract IReadOnlyList<string> GetDisplayAmountVarKeys();

    protected DynamicVar MainSecondAmountVar => DynamicVars[GetDisplayAmountVarKeys()[0]];

    public string GetSecondAmount() => $"{MainSecondAmountVar.IntValue}";

    public void UpgradeSecondAmount(decimal addend) => UpgradeVar(MainSecondAmountVar, addend);

    protected void UpgradeVar(DynamicVar v, decimal addend)
    {
        NinjaHelper.UpgradeDynamicVarValue(v, addend);
        this.InvokeSecondAmountChanged();
    }

    protected void ApplyProportionalUpgrade(decimal ratio)
    {
        foreach (var key in GetDisplayAmountVarKeys())
        {
            var v = DynamicVars[key];
            NinjaHelper.UpgradeDynamicVarValue(v, ratio * v.IntValue);
        }
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
        if (
            power != this
            || cardSource is ISecondAmountPowerUpgradeProvider provider
                && provider.TryUpgradeSecondAmount(this)
        )
            return Task.CompletedTask;

        var previous = Amount - amount;
        if (previous == 0)
            return Task.CompletedTask;

        ApplyProportionalUpgrade(amount / previous);
        return Task.CompletedTask;
    }
}
