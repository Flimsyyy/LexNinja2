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
public class PenglaiCan() : LexNinja2Card(0, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Food];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [CardKeyword.Exhaust, CardKeyword.Retain];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/PenglaiCan.mp3");
        CardSelectorPrefs prefs = new CardSelectorPrefs(
            SelectionScreenPrompt,
            0,
            DynamicVars.Cards.IntValue
        );
        foreach (
            CardModel card in await CardSelectCmd.FromHand(
                choiceContext,
                Owner,
                prefs,
                (Func<CardModel, bool>)null,
                (AbstractModel)this
            )
        )
            await CardCmd.Exhaust(choiceContext, card);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(2);
    }

    public override string CustomPortraitPath => $"PenglaiCan_p.png".BigCardImagePath();
    public override string PortraitPath => $"PenglaiCan.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/PenglaiCan.png".CardImagePath();
}
