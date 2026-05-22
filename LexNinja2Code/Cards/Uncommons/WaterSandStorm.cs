using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards.Uncommons;

public class WaterSandStorm()
    : LexNinja2Card(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new NinjutsuVar(2),
            new PowerVar<SandWall>(10),
            new CalculationBaseVar(0),
            new ExtraDamageVar(1),
            new CalculatedDamageVar(ValueProp.Move).WithMultiplier(
                (card, _) =>
                {
                    var sandWall = card.Owner.Creature.GetPowerAmount<SandWall>();
                    if (card is not LexNinja2Card ninjaCard || !ninjaCard.CanCastNinjutsu())
                        return sandWall;
                    sandWall += card.DynamicVars.Power<SandWall>().IntValue;
                    return sandWall;
                }
            ),
        ];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/WaterSandStorm.mp3");
        if (await Ninjutsu(choiceContext, play))
        {
            await CommonActions.ApplySelf<SandWall>(choiceContext, this);
        }
        if (!Owner.HasPower<SandWall>())
        {
            return;
        }
        var nRollingBoulderVfx = NRollingBoulderVfx.Create(
            CombatState!.HittableEnemies,
            Owner.Creature.GetPower<SandWall>()!.Amount
        );
        if (nRollingBoulderVfx == null)
        {
            return;
        }
        // nRollingBoulderVfx.Connect(
        //     NRollingBoulderVfx.SignalName.HitCreature,
        //     Callable.From(
        //         async delegate(NCreature _)
        //         {
        //             await CommonActions
        //                 .CardAttack(this, play, tmpSfx: "blunt_attack.mp3")
        //                 .Execute(choiceContext);
        //         }
        //     )
        // );
        await CommonActions
            .CardAttack(
                this,
                play.Target,
                damage: Owner.Creature.GetPowerAmount<SandWall>(),
                tmpSfx: "blunt_attack.mp3"
            )
            .Execute(choiceContext);
        var signalAwaiter = nRollingBoulderVfx.ToSignal(
            nRollingBoulderVfx,
            NRollingBoulderVfx.SignalName.Finished
        );
        NCombatRoom.Instance?.CombatVfxContainer.CallDeferred(
            Node.MethodName.AddChild,
            nRollingBoulderVfx
        );
        await signalAwaiter;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<SandWall>().UpgradeValueBy(3);
    }

    public override string CustomPortraitPath => $"WaterSandStorm_p.png".BigCardImagePath();
    public override string PortraitPath => $"WaterSandStorm.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/WaterSandStorm.png".CardImagePath();

    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
