using System.Diagnostics.CodeAnalysis;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Relics;
using LexNinja2.LexNinja2Code.Character;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace LexNinja2.LexNinja2Code.Relics;

[Pool(typeof(LexNinja2RelicPool))]
public class ThreeDuuz() : LexNinja2Relic
{
    public override RelicRarity Rarity => RelicRarity.Shop;

    private bool _wasUsedThisTurn;
    private CardModel? _cardBeingPlayed;

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public bool WasUsedThisTurn
    {
        get => _wasUsedThisTurn;
        set
        {
            AssertMutable();
            _wasUsedThisTurn = value;
        }
    }

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public CardModel? CardBeingPlayed
    {
        get => _cardBeingPlayed;
        set
        {
            AssertMutable();
            _cardBeingPlayed = value;
        }
    }

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (
            CardBeingPlayed != null
            || cardPlay.Card.Owner != base.Owner
            || WasUsedThisTurn
            || !cardPlay.Card.Tags.Contains(NinjaTags.Ninjutsu)
        )
        {
            return Task.CompletedTask;
        }

        CardBeingPlayed = cardPlay.Card;
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card == CardBeingPlayed)
        {
            Flash();
            NinjaAudio.Play("res://LexNinja2/audio/3Duuz.mp3");
            await CardPileCmd.Draw(context, 2, cardPlay.Card.Owner);
            WasUsedThisTurn = true;
            CardBeingPlayed = null;
        }
    }

    public override Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> creatures,
        ICombatState combatState
    )
    {
        if (side != base.Owner.Creature.Side)
        {
            return Task.CompletedTask;
        }
        WasUsedThisTurn = false;
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom _)
    {
        WasUsedThisTurn = false;
        return Task.CompletedTask;
    }

    public override string PackedIconPath => "3Duuz.png".RelicImagePath();
    protected override string PackedIconOutlinePath => "/outline/3Duuz.png".RelicImagePath();
    protected override string BigIconPath => "3Duuz.png".BigRelicImagePath();
}
