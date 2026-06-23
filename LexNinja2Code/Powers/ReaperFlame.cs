using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Powers;

public class ReaperFlame : LexNinja2Power
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override object InitInternalData() => new Data();

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(Amount, ValueProp.Unblockable | ValueProp.Unpowered)];

    public override string CustomIconPath => "DeathGodFlame32.png".PowerImagePath();
    public override string? CustomBigIconPath => "DeathGodFlame84.png".BigPowerImagePath();

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player)
        {
            return Task.CompletedTask;
        }
        GetInternalData<Data>().AmountsForPlayedCards.Add(cardPlay.Card, Amount);
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (
            cardPlay.Card.Owner != Owner.Player
            || !GetInternalData<Data>().AmountsForPlayedCards.Remove(cardPlay.Card, out var amount)
        )
        {
            return;
        }
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
                amount,
                ValueProp.Unblockable | ValueProp.Unpowered,
                null,
                null
            );
        }
    }

    private class Data
    {
        public readonly Dictionary<CardModel, int> AmountsForPlayedCards = new();
    }
}
