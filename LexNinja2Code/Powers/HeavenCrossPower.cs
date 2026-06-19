using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Hooks;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace LexNinja2.LexNinja2Code.Powers;

public class HeavenCrossPower : CustomPowerModel, IAfterLexKelaSpent
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<Lexkela>(), HoverTipFactory.FromKeyword(NinjaKeyword.Renshu)];

    public override string CustomPackedIconPath => "HeavenCrossPower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "HeavenCrossPower84.png".BigPowerImagePath();

    public async Task AfterLexKelaSpent(
        PlayerChoiceContext choiceContext,
        int amount,
        Player spender
    )
    {
        if (spender != Owner.Player)
        {
            return;
        }

        await NinjaHelper.AddLexKela(choiceContext, spender, Amount, null);
    }
}
