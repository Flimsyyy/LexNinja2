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
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace LexNinja2.LexNinja2Code.Relics;

[Pool(typeof(LexNinja2RelicPool))]
public class KFC() : LexNinja2Relic
{
    public override RelicRarity Rarity => RelicRarity.Common;

    public const int TurnsThreshold = 3;
    private const string TurnsKey = "Turns";
    private bool _isActivating;
    private int _turnsSeen;
    public override bool ShowCounter => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new(TurnsKey, 3M)];

    public override int DisplayAmount => !IsActivating ? TurnsSeen : DynamicVars[TurnsKey].IntValue;

    private bool IsActivating
    {
        get => _isActivating;
        set
        {
            AssertMutable();
            _isActivating = value;
            InvokeDisplayAmountChanged();
        }
    }

    // BaseLib requires it to be public so that it can be scanned
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SavedProperty]
    public int TurnsSeen
    {
        get => _turnsSeen;
        set
        {
            AssertMutable();
            _turnsSeen = value;
            InvokeDisplayAmountChanged();
        }
    }

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> creatures,
        ICombatState combatState
    )
    {
        if (side != Owner.Creature.Side)
        {
            return;
        }
        TurnsSeen = (TurnsSeen + 1) % DynamicVars[TurnsKey].IntValue;
        Status =
            TurnsSeen == DynamicVars[TurnsKey].IntValue - 1
                ? RelicStatus.Active
                : RelicStatus.Normal;
        if (TurnsSeen != 0)
        {
            return;
        }
        await TaskHelper.RunSafely(DoActivateVisuals());
        NinjaAudio.Play("res://LexNinja2/audio/KFC.mp3");
        var distinctForCombat = CardFactory.GetDistinctForCombat(
            Owner,
            from c in ModelDb
                .CardPool<TokenCardPool>()
                .GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
            where (c.Tags.Contains(NinjaTags.Food))
            select c,
            1,
            Owner.RunState.Rng.CombatCardGeneration
        );
        foreach (var item in distinctForCombat.ToList())
        {
            await CardPileCmd.AddGeneratedCardToCombat(item, PileType.Hand, Owner);
        }
    }

    private async Task DoActivateVisuals()
    {
        IsActivating = true;
        Flash();
        await Cmd.Wait(1f);
        IsActivating = false;
    }

    public override Task AfterCombatEnd(CombatRoom _)
    {
        base.Status = RelicStatus.Normal;
        return Task.CompletedTask;
    }

    public override string PackedIconPath => "KFC.png".RelicImagePath();
    protected override string PackedIconOutlinePath => "/outline/KFC.png".RelicImagePath();
    protected override string BigIconPath => "KFC.png".BigRelicImagePath();
}
