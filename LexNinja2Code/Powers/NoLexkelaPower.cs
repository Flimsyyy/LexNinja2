using System.Collections.Generic;
using System.Threading.Tasks;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Combat.SecondaryResources;

namespace LexNinja2.LexNinja2Code.Powers;

public class NoLexkelaPower : LexNinja2Power, ISecondaryResourceHookListener
{
    private const float Scale = 0.8f;
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomIconPath => "NoLexkelaPower.png".PowerImagePath();
    public override string? CustomBigIconPath => "NoLexkelaPower.png".BigPowerImagePath();
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [LexKela.HoverTip()];

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> creatures
    )
    {
        if (side != Owner.Side)
        {
            return;
        }
        await PowerCmd.Decrement(this);
    }

    public bool ShouldGainSecondaryResource(SecondaryResourceContext context, decimal amount)
    {
        return context.Definition.Id != LexKela.Id || context.Player != Owner.Player;
    }
}
