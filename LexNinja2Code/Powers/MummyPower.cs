using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Powers;

public class MummyPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override bool AllowNegative => true;

    public override string CustomPackedIconPath => "MummyPower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "MummyPower84.png".BigPowerImagePath();
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<Lexkela>()];

    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource
    )
    {
        if (
            power.Owner == Owner
            && Owner.HasPower<Lexkela>()
            && power is Lexkela
            && (amount < 0m || -amount == Owner.GetPowerAmount<Lexkela>())
        )
        {
            await ExecuteMummyEffect();
        }

        // if (Owner.GetPower<Lexkela>()!=null&&(amount < 0m) && applier == base.Owner && power is Lexkela)
        // {
        //     Flash();
        //     NinjaAudio.Play("res://LexNinja2/audio/JRMummy.mp3");
        //     await CreatureCmd.GainBlock(Owner,this.Amount,ValueProp.Unpowered,null);
        // }
    }

    private async Task ExecuteMummyEffect()
    {
        Flash();
        NinjaAudio.Play("res://LexNinja2/audio/JRMummy.mp3");
        await CreatureCmd.GainBlock(base.Owner, (decimal)this.Amount, ValueProp.Unpowered, null);
    }
}
