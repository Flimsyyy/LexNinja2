using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace LexNinja2.LexNinja2Code.Cards.Tokens;

[Pool(typeof(CurseCardPool))]
public class MosquitoHand() : LexNinja2Card(-1, CardType.Curse, CardRarity.Curse, TargetType.Self)
{
    public override int MaxUpgradeLevel => 0;
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [NinjaKeyword.Hand, CardKeyword.Retain, CardKeyword.Unplayable];

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner)
        {
            return;
        }
        NinjaAudio.Play("res://LexNinja2/audio/Mosquito2.mp3");
    }

    public override async Task AfterCardDrawn(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw
    )
    {
        if (card != this)
        {
            return;
        }
        NinjaAudio.Play("res://LexNinja2/audio/MosquitoHand.mp3");
    }

    public override string CustomPortraitPath => $"MosquitoHand_p.png".BigCardImagePath();
    public override string PortraitPath => $"MosquitoHand.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/MosquitoHand.png".CardImagePath();
}
