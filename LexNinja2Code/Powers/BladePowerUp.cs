using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Powers;

public class BladePowerUp : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override bool AllowNegative => true;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(NinjaKeyword.Blade)];

    public override string CustomPackedIconPath => "BladePowerUp32.png".PowerImagePath();
    public override string? CustomBigIconPath => "BladePowerUp84.png".BigPowerImagePath();

    public override Decimal ModifyDamageAdditive(
        Creature? target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource
    )
    {
        return
            this.Owner != dealer
            || !props.IsPoweredAttack()
            || cardSource == null
            || (
                !cardSource.Keywords.Contains(NinjaKeyword.Blade)
                && !cardSource.Tags.Contains(CardTag.Shiv)
            )
            ? 0M
            : (Decimal)this.Amount;
    }
}
