using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Powers;

public class DamageNextTurnPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(Amount, ValueProp.Unpowered)];

    public override string CustomPackedIconPath => "power.png".PowerImagePath();
    public override string? CustomBigIconPath => "power.png".BigPowerImagePath();

    public override async Task AfterPlayerTurnStart(
        PlayerChoiceContext choiceContext,
        Player player
    )
    {
        if (player != Owner.Player)
            return;
        Flash();
        await CreatureCmd.TriggerAnim(Owner, "Attack", 0.5F);
        await Cmd.CustomScaledWait(0.2f, 0.4f);
        foreach (var hittableEnemy in CombatState.HittableEnemies)
        {
            var instance = NCombatRoom.Instance;
            instance?.CombatVfxContainer.AddChildSafely(NBigSlashVfx.Create(hittableEnemy)!);
        }
        await Cmd.CustomScaledWait(0.2f, 0.4f);
        await CreatureCmd.Damage(
            choiceContext,
            CombatState.HittableEnemies,
            DynamicVars.Damage,
            Owner
        );
        await PowerCmd.Remove(this);
    }
}
