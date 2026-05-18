using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace LexNinja2.LexNinja2Code.Cards.Tokens;

[Pool(typeof(TokenCardPool))]
public class ISeeYou() : LexNinja2Card(-1, CardType.Quest, CardRarity.Quest, TargetType.Self)
{
    // private int _cardsInHand;
    private int _combatsSeen;
    public override int MaxUpgradeLevel => 0;
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new("boss", 2), new DynamicVar("Frail", 1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<FrailPower>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];

    // private int CardsInHand
    // {
    //     get { return _cardsInHand; }
    //     set
    //     {
    //         AssertMutable();
    //         _cardsInHand = value;
    //     }
    // }

    public override bool HasTurnEndInHandEffect => true;

    // public override Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    // {
    //     if (side != CombatSide.Player)
    //     {
    //         return Task.CompletedTask;
    //     }
    //     if (base.Pile.Type != PileType.Hand)
    //     {
    //         return Task.CompletedTask;
    //     }
    //     CardsInHand = base.Pile.Cards.Count;
    //     return Task.CompletedTask;
    // }

    [SavedProperty]
    private int CombatsSeen
    {
        get => _combatsSeen;
        set
        {
            AssertMutable();
            _combatsSeen = value;
            DynamicVars["boss"].BaseValue = 2 - CombatsSeen;
        }
    }

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        var alreadyHasFrail = Owner.Creature.HasPower<FrailPower>();
        NinjaAudio.Play("res://LexNinja2/audio/ISeeYou.mp3");
        var powerModel = await PowerCmd.Apply<FrailPower>(
            choiceContext,
            Owner.Creature,
            DynamicVars["Frail"].BaseValue,
            Owner.Creature, //why it's null
            this
        );
        if (powerModel != null && !alreadyHasFrail)
        {
            powerModel.SkipNextDurationTick = true;
        }
    }

    public override async Task AfterCombatEnd(CombatRoom room)
    {
        var pile = Pile;

        if (room.RoomType != RoomType.Boss)
        {
            return;
        }
        CombatsSeen++;

        if (DynamicVars["boss"].BaseValue > 0)
        {
            NinjaAudio.Stop("res://LexNinja2/audio/WhereRUNow.mp3");
            NinjaAudio.Play("res://LexNinja2/audio/WhereRUNow.mp3", 0.15f);
            return;
        }
        if (pile is { Type: PileType.Deck })
        {
            NinjaAudio.Stop("res://LexNinja2/audio/Fadeded.mp3");
            NinjaAudio.Play("res://LexNinja2/audio/Fadeded.mp3");
            CardModel aW = Owner.RunState.CreateCard<AlanWalker>(Owner);
            CardCmd.Upgrade(aW);
            CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(aW, PileType.Deck));
            await CardPileCmd.RemoveFromDeck(this);
        }
    }

    public override string CustomPortraitPath => $"ISeeYou.png".BigCardImagePath();
    public override string PortraitPath => $"ISeeYou.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/ISeeYou.png".CardImagePath();
}
