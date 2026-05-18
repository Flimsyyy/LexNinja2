using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace LexNinja2.LexNinja2Code.Cards.Tokens;

[Pool(typeof(TokenCardPool))]
public class SaltyFish() : LexNinja2Card(0, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Food];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        CardModel card = (
            await CardSelectCmd.FromSimpleGrid(
                choiceContext,
                PileType.Discard.GetPile(Owner).Cards,
                Owner,
                new CardSelectorPrefs(SelectionScreenPrompt, 1)
            )
        ).FirstOrDefault<CardModel>();
        if (card == null)
            return;
        NinjaAudio.Play("res://LexNinja2/audio/SaltyFish.mp3");
        card.SetToFreeThisTurn();
        CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, PileType.Hand);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }

    public override string CustomPortraitPath => $"SaltyFish_p.png".BigCardImagePath();
    public override string PortraitPath => $"SaltyFish.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/SaltyFish.png".CardImagePath();
}
