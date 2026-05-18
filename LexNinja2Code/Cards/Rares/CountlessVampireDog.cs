using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Cards.Rares;

public class CountlessVampireDog()
    : LexNinja2Card(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [CardKeyword.Ethereal, CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var original = CommonActions
            .SelectSingleCard(
                this,
                CardSelectorPrefs.TransformSelectionPrompt,
                choiceContext,
                PileType.Hand
            )
            .Result;
        if (original == null)
        {
            return;
        }
        NinjaAudio.Play("res://LexNinja2/audio/CountlessVampireDog.mp3");
        var num = 10 - CardPile.GetCards(Owner, PileType.Hand).Count();
        for (var index = 0; index < num; ++index)
            await CardPileCmd.AddGeneratedCardToCombat(
                original.CreateClone(),
                PileType.Hand,
                Owner
            );
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Ethereal);
    }

    public override string CustomPortraitPath => $"CountlessVampireDog.png".BigCardImagePath();
    public override string PortraitPath => $"CountlessVampireDog.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/CountlessVampireDog.png".CardImagePath();
}
