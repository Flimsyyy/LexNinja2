using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace LexNinja2.LexNinja2Code.Powers;

public class BuildSandWallPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<SandWall>()];

    public override string CustomPackedIconPath => "BuildSandWallPower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "BuildSandWallPower84.png".BigPowerImagePath();
}
