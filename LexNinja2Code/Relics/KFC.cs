using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Character;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
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

    public const int turnsThreshold = 3;
    private const string _turnsKey = "Turns";
    private bool _isActivating;
    private int _turnsSeen;
    public override bool ShowCounter => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Turns", 3M)];

    public override int DisplayAmount
    {
        get
        {
            if (!IsActivating)
            {
                return TurnsSeen;
            }
            return base.DynamicVars["Turns"].IntValue;
        }
    }
    private bool IsActivating
    {
        get => this._isActivating;
        set
        {
            this.AssertMutable();
            this._isActivating = value;
            this.InvokeDisplayAmountChanged();
        }
    }

    [SavedProperty]
    public int TurnsSeen
    {
        get => this._turnsSeen;
        set
        {
            this.AssertMutable();
            this._turnsSeen = value;
            this.InvokeDisplayAmountChanged();
        }
    }

    public override async Task AfterSideTurnStart(CombatSide side, ICombatState combatState)
    {
        if (side == base.Owner.Creature.Side)
        {
            TurnsSeen = (TurnsSeen + 1) % base.DynamicVars["Turns"].IntValue;
            base.Status = (
                (TurnsSeen == base.DynamicVars["Turns"].IntValue - 1)
                    ? RelicStatus.Active
                    : RelicStatus.Normal
            );
            if (TurnsSeen == 0)
            {
                TaskHelper.RunSafely(DoActivateVisuals());
                NinjaAudio.Play("res://LexNinja2/audio/KFC.mp3");
                IEnumerable<CardModel> distinctForCombat = CardFactory.GetDistinctForCombat(
                    base.Owner,
                    from c in ModelDb
                        .CardPool<TokenCardPool>()
                        .GetUnlockedCards(
                            base.Owner.UnlockState,
                            base.Owner.RunState.CardMultiplayerConstraint
                        )
                    where (c.Tags.Contains(NinjaTags.Food))
                    select c,
                    1,
                    base.Owner.RunState.Rng.CombatCardGeneration
                );
                foreach (CardModel item in distinctForCombat.ToList())
                {
                    await CardPileCmd.AddGeneratedCardToCombat(item, PileType.Hand, Owner);
                }
            }
        }
    }

    private async Task DoActivateVisuals()
    {
        IsActivating = true;
        Flash();
        await MegaCrit.Sts2.Core.Commands.Cmd.Wait(1f);
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
