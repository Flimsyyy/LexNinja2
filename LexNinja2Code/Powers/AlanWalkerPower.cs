using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Powers;

public class AlanWalkerPower : LexNinja2Power
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new BlockVar(7, ValueProp.Unpowered)];
    public override string CustomIconPath => "AlanWalkerPower.png".PowerImagePath();
    public override string? CustomBigIconPath => "AlanWalkerPower.png".BigPowerImagePath();

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player)
        {
            return;
        }
        for (var i = 0; i < Amount; i++)
        {
            Flash();
            var enemies = CombatState.HittableEnemies.Where(e => e.IsAlive).ToList();
            var target = enemies.LastOrDefault();
            if (target == null)
            {
                return;
            }

            var nHyperbeamVfx = NHyperbeamVfx.Create(base.Owner, target);
            if (nHyperbeamVfx != null)
            {
                NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(nHyperbeamVfx);
                await Cmd.Wait(0.5f);
            }

            foreach (var enemy in enemies)
            {
                var nHyperbeamImpactVfx = NHyperbeamImpactVfx.Create(Owner, enemy);
                if (nHyperbeamImpactVfx != null)
                {
                    NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(nHyperbeamImpactVfx);
                }

                await CreatureCmd.Damage(context, enemy, 6, ValueProp.Unpowered, null, null);
            }

            await CreatureCmd.GainBlock(Owner, DynamicVars.Block, null);
        }
    }

    public override Task BeforeDeath(Creature creature)
    {
        if (creature != Owner)
            return Task.CompletedTask;
        NinjaAudio.Stop("res://LexNinja2/audio/Faded.mp3", 5f);
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        NinjaAudio.Stop("res://LexNinja2/audio/Faded.mp3", 12f);
        return Task.CompletedTask;
    }
}
