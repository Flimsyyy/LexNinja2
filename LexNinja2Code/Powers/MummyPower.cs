using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Combat.SecondaryResources;

namespace LexNinja2.LexNinja2Code.Powers;

public class MummyPower : LexNinja2Power, ISecondaryResourceHookListener
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override bool AllowNegative => true;

    public override string CustomIconPath => "MummyPower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "MummyPower84.png".BigPowerImagePath();
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [LexKela.HoverTip()];

    public async Task AfterSecondaryResourceChanged(SecondaryResourceChangeContext context)
    {
        if (
            context.Definition.Id != LexKela.Id
            || context.Player != Owner.Player
            || context.Delta >= 0
        )
        {
            return;
        }
        await ExecuteMummyEffect();
    }

    private async Task ExecuteMummyEffect()
    {
        Flash();
        NinjaAudio.Play("res://LexNinja2/audio/JRMummy.mp3");
        await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
    }
}
