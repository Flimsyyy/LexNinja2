using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Rooms;

namespace LexNinja2.LexNinja2Code.Cards.Curses;

[Pool(typeof(CurseCardPool))]
public class MosquitoHand() : LexNinja2Card(-1, CardType.Curse, CardRarity.Curse, TargetType.Self)
{
    public override int MaxUpgradeLevel => 0;
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [NinjaKeyword.Hand, CardKeyword.Retain, CardKeyword.Unplayable];

    public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (Pile!.Type == PileType.Exhaust || cardPlay.Card.Owner != Owner)
        {
            return Task.CompletedTask;
        }
        NinjaAudio.Play("res://LexNinja2/audio/Mosquito2.mp3", 0.4f);
        return Task.CompletedTask;
    }

    // public override Task AfterCardDrawn(
    //     PlayerChoiceContext choiceContext,
    //     CardModel card,
    //     bool fromHandDraw
    // )
    // {
    //     if (card != this)
    //     {
    //         return Task.CompletedTask;
    //     }
    //     NinjaAudio.Play("res://LexNinja2/audio/MosquitoHand.mp3");
    //     return Task.CompletedTask;
    // }

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is not CombatRoom)
            return;
        NinjaAudio.Play("res://LexNinja2/audio/MosquitoHand.mp3");
        await Cmd.Wait(0.5f);
        NinjaAudio.Play("res://LexNinja2/audio/Mosquito2.mp3", 0.4f);
    }

    public override string CustomPortraitPath => $"MosquitoHand_p.png".BigCardImagePath();
    public override string PortraitPath => $"MosquitoHand.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/MosquitoHand.png".CardImagePath();
}
