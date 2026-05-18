using BaseLib.Abstracts;
using Godot;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace LexNinja2.LexNinja2Code.Powers;

public class NuclearDragonPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "NuclearDragonPower.png".PowerImagePath();
    public override string? CustomBigIconPath => "NuclearDragonPower.png".BigPowerImagePath();
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<Lexkela>()];

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != this.Owner.Side)
        {
            return;
        }

        if (Owner.GetPower<Lexkela>() == null)
        {
            return;
        }
        Flash();
        await PowerCmd.Apply<Lexkela>(
            new ThrowingPlayerChoiceContext(),
            Owner,
            -Owner.GetPower<Lexkela>().Amount,
            null,
            null
        );

        float scale = 0.8f;
        NGroundFireVfx child = NGroundFireVfx.Create(Owner);
        if (child == null)
            return;
        SfxCmd.Play("event:/sfx/characters/attack_fire");
        child.Scale = Vector2.One * scale;
        NCombatRoom instance = NCombatRoom.Instance;
        if (instance != null)
            instance.CombatVfxContainer.AddChildSafely((Node)child);
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
        await PowerCmd.Apply<Lexkela>(new ThrowingPlayerChoiceContext(), Owner, Amount, null, null);

        float scale = 0.8f;
        NGroundFireVfx child = NGroundFireVfx.Create(Owner);
        if (child == null)
            return;
        SfxCmd.Play("event:/sfx/characters/attack_fire");
        child.Scale = Vector2.One * scale;
        NCombatRoom instance = NCombatRoom.Instance;
        if (instance != null)
            instance.CombatVfxContainer.AddChildSafely((Node)child);
    }

    /*public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Side)
            return;
        Flash();
        await PowerCmd.Apply<Lexkela>(new ThrowingPlayerChoiceContext(), Owner, Amount, null, null);
    }*/
}
