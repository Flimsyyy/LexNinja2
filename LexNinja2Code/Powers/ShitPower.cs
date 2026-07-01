using System.Collections.Generic;
using System.Threading.Tasks;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace LexNinja2.LexNinja2Code.Powers;

public class ShitPower : LexNinja2Power
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [LexKela.HoverTip()];

    public override string CustomIconPath => "ShitPower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "ShitPower84.png".BigPowerImagePath();

    public override async Task AfterEnergyReset(Player player)
    {
        if (player != Owner.Player)
            return;
        await PlayerCmd.GainEnergy(1, player);
        await LexKela.Gain(player, Amount, null);
        await PowerCmd.Remove(this);
    }
}
