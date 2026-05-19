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
        get => _wasOwnerPartOfLastPlayerTurn;
        set
        {
            AssertMutable();
            _wasOwnerPartOfLastPlayerTurn = value;
        }
    }

    public override bool ShouldTakeExtraTurn(Player player)
    {
        return WasOwnerPartOfLastPlayerTurn && player == Owner.Player && _isEffective;
    }

    public override Task AfterSideTurnStart(CombatSide side, ICombatState combatState)
    {
        if (side != Owner.Side)
            return Task.CompletedTask;
        if (Owner.Player != null)
            WasOwnerPartOfLastPlayerTurn = CombatManager.Instance.IsPartOfPlayerTurn(Owner.Player);
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
