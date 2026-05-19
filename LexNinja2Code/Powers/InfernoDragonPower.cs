using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Powers;

public class InfernoDragonPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new("dian", 0)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<Lexkela>()];

    public override string CustomPackedIconPath => "InfernoDragonPower.png".PowerImagePath();
    public override string? CustomBigIconPath => "InfernoDragonPower.png".BigPowerImagePath();

    private decimal CalculateExtraDamage()
    {
        var lexKela = Owner.GetPower<Lexkela>();
        if (lexKela != null)
        {
            decimal kela = lexKela.Amount;
            DynamicVars["dian"].BaseValue = Amount * kela;
        }
        else
        {
            DynamicVars["dian"].BaseValue = 0;
        }
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
