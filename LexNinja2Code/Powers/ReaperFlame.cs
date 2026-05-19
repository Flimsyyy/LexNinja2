using BaseLib.Abstracts;
using Godot;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Powers;

public class ReaperFlame : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override object InitInternalData() => new Data();

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(Amount, ValueProp.Unblockable | ValueProp.Unpowered)];

    public override string CustomPackedIconPath => "DeathGodFlame32.png".PowerImagePath();
    public override string? CustomBigIconPath => "DeathGodFlame84.png".BigPowerImagePath();

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        GetInternalData<Data>().AmountsForPlayedCards.Add(cardPlay.Card, Amount);
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (
            GetInternalData<Data>().AmountsForPlayedCards.Remove(cardPlay.Card, out _)
            && cardPlay.Card.Owner == Owner.Player
        )
        {
            Flash();
            var instance = NCombatRoom.Instance;
            foreach (var enemy in CombatState.HittableEnemies)
            {
                instance?.CombatVfxContainer.AddChildSafely(
                    NGroundFireVfx.Create(enemy, VfxColor.Purple)!
                );
                SfxCmd.Play("event:/sfx/characters/attack_fire");
                await CreatureCmd.Damage(
                    context,
                    enemy,
                    Amount,
                    ValueProp.Unblockable | ValueProp.Unpowered,
                    null,
                    null
                );
            }
        }
    }

    private class Data
    {
        public readonly Dictionary<CardModel, int> AmountsForPlayedCards = new();
    }
}
