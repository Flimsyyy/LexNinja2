using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards.Rares;

public class PastHasGoneHand() : LexNinja2Card(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CalculationBaseVar(0),
            new CalculationExtraVar(4),
            new CalculatedBlockVar(ValueProp.Move).WithMultiplier(
                (card, _) => PileType.Discard.GetPile(card.Owner).Cards.Count
            ),
        ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(CardKeyword.Exhaust), HoverTipFactory.FromPower<Lexkela>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Hand];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/PastHasGoneHand.mp3");
        var cardToExhaust = PileType.Discard.GetPile(Owner).Cards.ToList();
        await NinjaHelper.AddLexKela(choiceContext, this, cardToExhaust.Count);
        await CommonActions.CardBlock(this, play);
        foreach (var card in cardToExhaust)
        {
            await CardCmd.Exhaust(choiceContext, card, false, true);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2);
    }

    public override string CustomPortraitPath => $"PastHasGoneHand_p.png".BigCardImagePath();
    public override string PortraitPath => $"PastHasGoneHand.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/PastHasGoneHand.png".CardImagePath();
}
