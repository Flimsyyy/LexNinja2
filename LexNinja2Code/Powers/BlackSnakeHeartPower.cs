using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LexNinja2.LexNinja2Code.Powers;

public class BlackSnakeHeartPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<IntangiblePower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar("Card", "滚木")];

    public override string CustomPackedIconPath => "BlackSnakeHeartPower.png".PowerImagePath();
    public override string? CustomBigIconPath => "BlackSnakeHeartPower.png".BigPowerImagePath();

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        string currentTitle = cardPlay.Card.Title.TrimEnd('+');
        string lastTitle = _lastPlayedCard?.Title.TrimEnd('+');
        if (
            lastTitle != null
            && currentTitle == lastTitle
            && cardPlay.Card.Owner == base.Owner.Player
        )
        {
            NinjaAudio.Play("res://LexNinja2/audio/BlackSnakeHeart.mp3");
            await CardCmd.Exhaust(context, cardPlay.Card);
            await PowerCmd.Apply<IntangiblePower>(
                new ThrowingPlayerChoiceContext(),
                Owner,
                Amount,
                Owner,
                null
            );
        }
        CardToStore(LastCard());
    }

    private CardModel LastCard()
    {
        CardModel card = CombatManager
            .Instance.History.CardPlaysFinished.LastOrDefault(
                delegate(CardPlayFinishedEntry e)
                {
                    bool flag = e.CardPlay.Card.Owner == base.Owner.Player;
                    bool flag2 = flag;
                    // if (flag2)
                    // {
                    //     CardType type = e.CardPlay.Card.Type;
                    //     bool flag3 = (uint)(type - 1) <= 1u;
                    //     flag2 = flag3;
                    // }
                    return flag2 && !e.CardPlay.Card.IsDupe;
                }
            )
            ?.CardPlay.Card;
        return card;
    }

    private void CardToStore(CardModel card)
    {
        ((StringVar)DynamicVars["Card"]).StringValue = card.Title.TrimEnd('+');
        _lastPlayedCard = card;
    }

    private CardModel _lastPlayedCard = null;
}
