using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace LexNinja2.LexNinja2Code.Powers;

public class ManTooWeakPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<Lexkela>()];

    public override string CustomPackedIconPath => "ManTooWeakPower.png".PowerImagePath();
    public override string? CustomBigIconPath => "ManTooWeakPower.png".BigPowerImagePath();

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState
    )
    {
        if (side != Owner.Side)
            return;
        if (Owner.GetPower<Lexkela>() == null)
        {
            return;
        }
        Flash();
        NinjaAudio.Play("res://LexNinja2/audio/ManTooWeak.mp3");
        await PowerCmd.Apply<Lexkela>(
            new ThrowingPlayerChoiceContext(),
            Owner,
            -Amount,
            Owner,
            null
        );
        // await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Owner, Amount, Owner, null);
        // await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), Owner, Amount, Owner, null);
    }
}
