using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Powers;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Powers;

public class InfernoDragonPower : LexNinja2Power
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new("dian", 0)];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [LexKela.HoverTip()];

    public override string CustomIconPath => "InfernoDragonPower.png".PowerImagePath();
    public override string? CustomBigIconPath => "InfernoDragonPower.png".BigPowerImagePath();

    private decimal CalculateExtraDamage()
    {
        decimal kela = LexKela.Get(Owner.Player!);
        DynamicVars["dian"].BaseValue = Amount * kela;
        return DynamicVars["dian"].BaseValue;
    }

    public override decimal ModifyDamageMultiplicative(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource
    )
    {
        var extraDamage = CalculateExtraDamage() / 100;
        return
            dealer != Owner && !Owner.Pets.Contains(dealer)
            || !props.IsPoweredAttack()
            || cardSource == null
            ? 1
            : 1 + extraDamage;
    }
}
