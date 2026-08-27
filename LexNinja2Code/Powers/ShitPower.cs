using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Powers;
using LexNinja2.LexNinja2Code.Cards.Commons;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Powers;

public class ShitPower : LexNinja2Power, IHasSecondAmount
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [LexKela.HoverTip()];

    public override string CustomIconPath => "ShitPower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "ShitPower84.png".BigPowerImagePath();

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(0)];

    public override async Task AfterEnergyReset(Player player)
    {
        if (player != Owner.Player)
            return;
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, player);
        await LexKela.Gain(player, Amount, this);
        await PowerCmd.Remove(this);
    }

    public string GetSecondAmount() => $"{DynamicVars.Energy.IntValue}";

    public void UpgradeEnergyValue(decimal addend)
    {
        NinjaHelper.UpgradeDynamicVarValue(DynamicVars.Energy, addend);
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
        if (power != this)
        {
            return Task.CompletedTask;
        }
        if (cardSource is GonnaEatShit)
        {
            UpgradeEnergyValue(1);
            return Task.CompletedTask;
        }

        var ratio = amount / (Amount - amount);
        UpgradeEnergyValue(ratio * DynamicVars.Energy.BaseValue);
        return Task.CompletedTask;
    }
}
