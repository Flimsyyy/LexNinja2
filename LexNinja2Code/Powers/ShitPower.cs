using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace LexNinja2.LexNinja2Code.Powers;

public class ShitPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<Lexkela>()];

    public override string CustomPackedIconPath => "ShitPower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "ShitPower84.png".BigPowerImagePath();

    public override async Task AfterEnergyReset(Player player)
    {
        if (player != Owner.Player)
            return;
        await PlayerCmd.GainEnergy(1, player);
        await PowerCmd.Apply<Lexkela>(new ThrowingPlayerChoiceContext(), Owner, Amount, null, null);
        await PowerCmd.Remove(this);
    }
}
