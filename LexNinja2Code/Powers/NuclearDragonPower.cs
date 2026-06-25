using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace LexNinja2.LexNinja2Code.Powers;

public class NuclearDragonPower : LexNinja2Power
{
    private const float Scale = 0.8f;
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomIconPath => "NuclearDragonPower.png".PowerImagePath();
    public override string? CustomBigIconPath => "NuclearDragonPower.png".BigPowerImagePath();
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [LexKela.HoverTip()];

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> creatures
    )
    {
        if (side != Owner.Side || Owner.Player == null)
        {
            return;
        }

        if (LexKela.Get(Owner.Player) <= 0)
        {
            return;
        }
        Flash();
        await LexKela.Reset(Owner.Player, this);
        var child = NGroundFireVfx.Create(Owner);
        if (child == null)
            return;
        SfxCmd.Play("event:/sfx/characters/attack_fire");
        child.Scale = Vector2.One * Scale;
        var instance = NCombatRoom.Instance;
        instance?.CombatVfxContainer.AddChildSafely(child);
    }

    public override async Task BeforeHandDraw(
        Player player,
        PlayerChoiceContext choiceContext,
        ICombatState combatState
    )
    {
        if (player != Owner.Player)
        {
            return;
        }
        Flash();
        NinjaAudio.Play("res://LexNinja2/audio/NuclearDragon.mp3");
        await LexKela.Gain(Owner.Player, Amount, this);

        var child = NGroundFireVfx.Create(Owner);
        if (child == null)
            return;
        SfxCmd.Play("event:/sfx/characters/attack_fire");
        child.Scale = Vector2.One * Scale;
        var instance = NCombatRoom.Instance;
        instance?.CombatVfxContainer.AddChildSafely(child);
    }
}
