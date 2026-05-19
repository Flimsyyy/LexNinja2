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
    private const string CardKey = "Card";
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<IntangiblePower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar(CardKey, "滚木")];
    
    protected override object InitInternalData() => new Data();

    public override string CustomPackedIconPath => "BlackSnakeHeartPower.png".PowerImagePath();
    public override string? CustomBigIconPath => "BlackSnakeHeartPower.png".BigPowerImagePath();

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        var currentCard = cardPlay.Card;
        var lastCard = GetInternalData<Data>().LastCard;
        if (lastCard == null)
        {
            return;
        }
        if (currentCard.GetType() == lastCard.GetType())
        {
            NinjaAudio.Play("res://LexNinja2/audio/BlackSnakeHeart.mp3");
            await CardCmd.Exhaust(context, cardPlay.Card);
            await PowerCmd.Apply<IntangiblePower>(
                context,
                Owner,
                Amount,
                Owner,
                null
            );
        }
        CardToStore(lastCard);
    }

    private void CardToStore(CardModel card)
    {
        ((StringVar)DynamicVars[CardKey]).StringValue = card.Title.TrimEnd('+');
        GetInternalData<Data>().LastCard = card;
    }

    private class Data
    {
        public CardModel? LastCard;
    }
}
