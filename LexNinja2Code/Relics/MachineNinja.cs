using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Character;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace LexNinja2.LexNinja2Code.Relics;

[Pool(typeof(LexNinja2RelicPool))]
public class MachineNinja() : LexNinja2Relic
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    public override string PackedIconPath => "MachineNinja.png".RelicImagePath();
    protected override string PackedIconOutlinePath => "/outline/MachineNinja.png".RelicImagePath();
    protected override string BigIconPath => "MachineNinja.png".BigRelicImagePath();

    // public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    // {
    //     if (side != Owner.Creature.Side)
    //         return;
    //     Flash();
    //     NinjaAudio.Play("res://LexNinja2/audio/machine.mp3");
    //     PowerCmd.Apply<SciencePower>(new ThrowingPlayerChoiceContext(), Owner.Creature, 1, null, null);
    // }
    private bool _wasUsedThisTurn;
    private CardModel? _cardBeingPlayed;
    private bool WasUsedThisTurn
    {
        get { return _wasUsedThisTurn; }
        set
        {
            AssertMutable();
            _wasUsedThisTurn = value;
        }
    }

    private CardModel? CardBeingPlayed
    {
        get { return _cardBeingPlayed; }
        set
        {
            AssertMutable();
            _cardBeingPlayed = value;
        }
    }

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (CardBeingPlayed != null)
        {
            return Task.CompletedTask;
        }
        if (cardPlay.Card.Owner != base.Owner)
        {
            return Task.CompletedTask;
        }
        if (WasUsedThisTurn)
        {
            return Task.CompletedTask;
        }
        if (!cardPlay.Card.Keywords.Contains(NinjaKeyword.Science))
        {
            return Task.CompletedTask;
        }
        CardBeingPlayed = cardPlay.Card;
        return Task.CompletedTask;
    }

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        if (card.Owner != base.Owner)
        {
            return playCount;
        }
        if (WasUsedThisTurn)
        {
            return playCount;
        }

        if (!card.Keywords.Contains(NinjaKeyword.Science))
        {
            return playCount;
        }
        Flash();
        NinjaAudio.Play("res://LexNinja2/audio/machine.mp3", 0.7f);
        WasUsedThisTurn = true;
        CardBeingPlayed = (CardModel)null;
        this.Status = RelicStatus.Normal;
        return playCount + 1;
    }

    // public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    // {
    //     if (cardPlay.Card != CardBeingPlayed)
    //         return;
    //     Flash();
    //
    //
    // }

    public override Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        ICombatState combatState
    )
    {
        if (side != base.Owner.Creature.Side)
        {
            return Task.CompletedTask;
        }
        WasUsedThisTurn = false;
        this.Status = RelicStatus.Active;
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom _)
    {
        WasUsedThisTurn = false;
        return Task.CompletedTask;
    }
}
