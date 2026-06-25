using System.Collections.Generic;
using System.Threading.Tasks;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace LexNinja2.LexNinja2Code.Powers;

public class ManTooWeakPower : LexNinja2Power
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [LexKela.HoverTip()];

    public override string CustomIconPath => "ManTooWeakPower.png".PowerImagePath();
    public override string? CustomBigIconPath => "ManTooWeakPower.png".BigPowerImagePath();

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState
    )
    {
        if (side != Owner.Side || Owner.Player == null)
            return;
        if (!await LexKela.Spend(Owner.Player, Amount, null, this))
        {
            return;
        }
        Flash();
        NinjaAudio.Play("res://LexNinja2/audio/ManTooWeak.mp3");
    }
}
