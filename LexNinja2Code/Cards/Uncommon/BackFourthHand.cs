using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards;

public class BackFourthHand()
    : LexNinja2Card(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new BlockVar(8, ValueProp.Move), new CardsVar(2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Hand];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/BackFourthHand.mp3");
        await NinjaAnim.TriggerCastAnim(this);
        await CommonActions.CardBlock(this, play);
        var selectCount = Math.Min(
            DynamicVars.Cards.IntValue,
            10 - PileType.Hand.GetPile(Owner).Cards.Count
        );
        if (selectCount <= 0)
            return;
        var cardsToDraw = await CommonActions.SelectCards(
            this,
            SelectionScreenPrompt,
            choiceContext,
            PileType.Discard,
            count: selectCount
        );
        await CardPileCmd.Add(cardsToDraw, PileType.Hand);
        var cardsToDiscard = await CommonActions.SelectCards(
            this,
            CardSelectorPrefs.DiscardSelectionPrompt,
            choiceContext,
            PileType.Hand,
            count: 2
        );
        await CardCmd.Discard(choiceContext, cardsToDiscard);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
    }

    public override string CustomPortraitPath => $"BackFourthHand_p.png".BigCardImagePath();
    public override string PortraitPath => $"BackFourthHand.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/BackFourthHand.png".CardImagePath();
}
