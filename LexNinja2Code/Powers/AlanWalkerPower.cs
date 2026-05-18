using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
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

public class AlanWalkerPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    // protected override object InitInternalData() => (object) new Data();

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new BlockVar(7, ValueProp.Unpowered)];
    public override string CustomPackedIconPath => "AlanWalkerPower.png".PowerImagePath();
    public override string? CustomBigIconPath => "AlanWalkerPower.png".BigPowerImagePath();

    // public override Task BeforeCardPlayed(CardPlay cardPlay)
    // {
    //     this.GetInternalData<Data>().amountsForPlayedCards.Add(cardPlay.Card, this.Amount);
    //     return Task.CompletedTask;
    // }

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        // if (GetInternalData<Data>().amountsForPlayedCards.Remove(cardPlay.Card, out var value)&&cardPlay.Card.Owner==Owner.Player)
        if (cardPlay.Card.Owner == Owner.Player)
        {
            Flash();
            NCombatRoom instance = NCombatRoom.Instance;
            List<Creature> enemies = base
                .CombatState.HittableEnemies.Where((Creature e) => e.IsAlive)
                .ToList();
            if (enemies.LastOrDefault() == null)
            {
                return;
            }
            NHyperbeamVfx nHyperbeamVfx = NHyperbeamVfx.Create(base.Owner, enemies.LastOrDefault());
            if (nHyperbeamVfx != null)
            {
                NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(nHyperbeamVfx);
                await MegaCrit.Sts2.Core.Commands.Cmd.Wait(0.5f);
            }
            foreach (Creature item in enemies)
            {
                NHyperbeamImpactVfx nHyperbeamImpactVfx = NHyperbeamImpactVfx.Create(
                    base.Owner,
                    item
                );
                if (nHyperbeamImpactVfx != null)
                {
                    NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(nHyperbeamImpactVfx);
                }
                await CreatureCmd.Damage(context, item, 6, ValueProp.Unpowered, null, null);
            }
            // foreach (Creature enemy in CombatState.HittableEnemies)
            // {
            //
            // }
            await CreatureCmd.GainBlock(Owner, DynamicVars.Block, null);
        }
    }

    public override async Task AfterCombatEnd(CombatRoom room)
    {
        NinjaAudio.Stop("res://LexNinja2/audio/Faded.mp3", 12f);
    }

    // private class Data
    // {
    //     public readonly Dictionary<CardModel, int> amountsForPlayedCards = new Dictionary<CardModel, int>();
    // }
}
