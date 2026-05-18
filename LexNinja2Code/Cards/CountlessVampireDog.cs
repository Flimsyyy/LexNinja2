using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Cards;

public class CountlessVampireDog()
    : LexNinja2Card(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [CardKeyword.Ethereal, CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        CardModel original = (
            await CardSelectCmd.FromHand(
                choiceContext,
                Owner,
                new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 1),
                null,
                this
            )
        ).FirstOrDefault();
        if (original != null)
        {
            int num = 10 - CardPile.GetCards(Owner, PileType.Hand).Count<CardModel>();
            List<CardModel> cards = new List<CardModel>();
            for (int index = 0; index < num; ++index)
                cards.Add((CardModel)original.CreateClone());
            NinjaAudio.Play("res://LexNinja2/audio/CountlessVampireDog.mp3");
            IReadOnlyList<CardPileAddResult> combat = await CardPileCmd.AddGeneratedCardsToCombat(
                (IEnumerable<CardModel>)cards,
                PileType.Hand,
                Owner
            );
        }
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Ethereal);
    }

    public override string CustomPortraitPath => $"CountlessVampireDog.png".BigCardImagePath();
    public override string PortraitPath => $"CountlessVampireDog.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/CountlessVampireDog.png".CardImagePath();
}
