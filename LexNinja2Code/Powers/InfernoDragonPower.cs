using System.Collections.Generic;
using System.Linq;
using BaseLib.Cards.Variables;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Powers;

public class InfernoDragonPower : LexNinja2Power
{
    private const string BuffAmount = "BuffAmount";
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new(BuffAmount + "Base", 0),
            new(BuffAmount + "Extra", 1),
            new CustomCalculatedVar(BuffAmount).WithMultiplier(
                (power, _) => CalculateExtraDamage(power)
            ),
        ];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [LexKela.HoverTip()];

    public override string CustomIconPath => "InfernoDragonPower.png".PowerImagePath();
    public override string? CustomBigIconPath => "InfernoDragonPower.png".BigPowerImagePath();

    private static decimal CalculateExtraDamage(PowerModel powerModel)
    {
        decimal kela = powerModel is { IsMutable: true, Owner.Player: not null }
            ? LexKela.Get(powerModel.Owner.Player)
            : 1;
        return powerModel.Amount * kela;
    }

    public override decimal ModifyDamageMultiplicative(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource,
        CardPlay? cardPlay
    )
    {
        var extraDamage = CalculateExtraDamage(this) / 100;
        return
            dealer != Owner && !Owner.Pets.Contains(dealer)
            || !props.IsPoweredAttack()
            || cardSource == null
            ? 1
            : 1 + extraDamage;
    }
}
