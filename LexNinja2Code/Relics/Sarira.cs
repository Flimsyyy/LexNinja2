using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace LexNinja2.LexNinja2Code.Relics;

[Pool(typeof(EventRelicPool))]
public class Sarira() : LexNinja2Relic
{
    public override RelicRarity Rarity => RelicRarity.Event;

    private bool _wasUsed;
    public override bool IsUsedUp => this._wasUsed;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(100)];

    public override string PackedIconPath => "Sarira.png".RelicImagePath();
    protected override string PackedIconOutlinePath => "/outline/Sarira.png".RelicImagePath();
    protected override string BigIconPath => "Sarira.png".BigRelicImagePath();

    [SavedProperty]
    public bool WasUsed
    {
        get => this._wasUsed;
        set
        {
            this.AssertMutable();
            this._wasUsed = value;
            if (!this.IsUsedUp)
                return;
            this.Status = RelicStatus.Disabled;
        }
    }

    public override bool ShouldDieLate(Creature creature)
    {
        return creature != this.Owner.Creature || this.WasUsed;
    }

    public override async Task AfterPreventingDeath(Creature creature)
    {
        Flash();
        WasUsed = true;
        NinjaAudio.Play("res://LexNinja2/audio/SariraRevive.mp3");
        await CreatureCmd.Heal(
            creature,
            Math.Max(1M, (Decimal)creature.MaxHp * (DynamicVars.Heal.BaseValue / 100M))
        );
        await PlayerCmd.GainGold(-22000000, Owner);
    }
}
