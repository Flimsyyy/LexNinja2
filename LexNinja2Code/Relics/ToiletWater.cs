using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Relics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Relics;

public class ToiletWater() : LexNinja2Relic
{
    public override RelicRarity Rarity => RelicRarity.Event;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];

    public override Task AfterCardDrawn(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw
    )
    {
        if (card.Owner != base.Owner)
        {
            return Task.CompletedTask;
        }
        if (card.Type != CardType.Curse)
        {
            return Task.CompletedTask;
        }
        NinjaAudio.Play("res://LexNinja2/audio/ToiletWater.mp3");
        Flash();
        CardCmd.Exhaust(choiceContext, card);
        CardPileCmd.Draw(choiceContext, Owner);
        return Task.CompletedTask;
    }

    public override string PackedIconPath => "ToiletWater.png".RelicImagePath();
    protected override string PackedIconOutlinePath => "ToiletWater.png".RelicImagePath();
    protected override string BigIconPath => "ToiletWater.png".BigRelicImagePath();
}
