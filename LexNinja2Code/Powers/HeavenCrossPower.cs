using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace LexNinja2.LexNinja2Code.Powers;

public class HeavenCrossPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<Lexkela>(), HoverTipFactory.FromKeyword(NinjaKeyword.Renshu)];

    public override string CustomPackedIconPath => "HeavenCrossPower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "HeavenCrossPower84.png".BigPowerImagePath();

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player)
        {
            return;
        }

        var card = cardPlay.Card;
        if (card is not LexNinja2Card ninjaCard || !card.Tags.Contains(NinjaTags.Ninjutsu))
        {
            return;
        }
        await NinjaHelper.AddLexKela(context, Owner, Amount, null);
    }
}
