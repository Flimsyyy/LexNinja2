using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Relics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace LexNinja2.LexNinja2Code.Relics;

public class ToiletWater : LexNinja2Relic
{
    public override RelicRarity Rarity => RelicRarity.Event;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new("Count", 2)];
    private int _count;
    public override bool ShowCounter => true;
    public override int DisplayAmount => Count;
    public int Count
    {
        get => _count;
        set
        {
            AssertMutable();
            _count = value;
            UpdateDisplay();
        }
    }

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is not CombatRoom)
            return;
        Count = DynamicVars["Count"].IntValue;
    }

    public override async Task AfterCardDrawn(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw
    )
    {
        if (card.Owner != base.Owner)
        {
            return;
        }
        if (card.Type != CardType.Curse)
        {
            return;
        }
        if (Count == 0)
        {
            return;
        }
        NinjaAudio.Play("res://LexNinja2/audio/ToiletWater.mp3");
        Flash();
        await CardCmd.Exhaust(choiceContext, card);
        await CardPileCmd.Draw(choiceContext, Owner);
        Count--;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (Count == 0)
        {
            Status = RelicStatus.Disabled;
        }
        else
        {
            Status = RelicStatus.Active;
        }
    }

    public override string PackedIconPath => "ToiletWater.png".RelicImagePath();
    protected override string PackedIconOutlinePath => "ToiletWater.png".RelicImagePath();
    protected override string BigIconPath => "ToiletWater.png".BigRelicImagePath();
}
