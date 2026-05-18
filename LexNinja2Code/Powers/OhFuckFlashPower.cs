using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace LexNinja2.LexNinja2Code.Powers;

public class OhFuckFlashPower : CustomPowerModel
{
    private bool _wasOwnerPartOfLastPlayerTurn = true;
    private bool _isEffective = true;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override string CustomPackedIconPath => "OhFuckFlash.png".PowerImagePath();
    public override string? CustomBigIconPath => "OhFuckFlash.png".BigPowerImagePath();

    private bool WasOwnerPartOfLastPlayerTurn
    {
        get => this._wasOwnerPartOfLastPlayerTurn;
        set
        {
            this.AssertMutable();
            this._wasOwnerPartOfLastPlayerTurn = value;
        }
    }

    public override bool ShouldTakeExtraTurn(Player player)
    {
        return this.WasOwnerPartOfLastPlayerTurn && player == this.Owner.Player && _isEffective;
    }

    public override Task AfterSideTurnStart(CombatSide side, ICombatState combatState)
    {
        if (side != this.Owner.Side)
            return Task.CompletedTask;
        this.WasOwnerPartOfLastPlayerTurn = CombatManager.Instance.IsPartOfPlayerTurn(
            this.Owner.Player
        );
        return Task.CompletedTask;
    }

    public override async Task AfterPlayerTurnStart(
        PlayerChoiceContext choiceContext,
        Player player
    )
    {
        if (player != Owner.Player)
            return;
        _isEffective = false;
        await PowerCmd.Remove(this);
    }
}
