using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace LexNinja2.LexNinja2Code.Powers;

public class HeavenCrossPower : LexNinja2Power
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [LexKela.HoverTip(), HoverTipFactory.FromKeyword(NinjaKeyword.RenShu)];

    public override string CustomIconPath => "HeavenCrossPower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "HeavenCrossPower84.png".BigPowerImagePath();

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player)
        {
            return;
        }

        var card = cardPlay.Card;
        if (!card.Tags.Contains(NinjaTags.Ninjutsu))
        {
            return;
        }
        await LexKela.Gain(Owner.Player, Amount, this);
    }
}
