using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Hooks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace LexNinja2.LexNinja2Code.Powers;

public class IRepresentShinobiPower : CustomPowerModel, IAfterLexKelaSpent
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(StaticHoverTip.Energy), HoverTipFactory.FromPower<Lexkela>()];

    public override string CustomPackedIconPath => "IRepresentShinobiPower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "IRepresentShinobiPower84.png".BigPowerImagePath();

    // public override async Task AfterPowerAmountChanged(
    //     PlayerChoiceContext choiceContext,
    //     PowerModel power,
    //     decimal amount,
    //     Creature? applier,
    //     CardModel? cardSource
    // )
    // {
    //     if (power is Lexkela && amount < 0 && power.Owner == Owner)
    //     {
    //         NinjaAudio.Play("res://LexNinja2/audio/IRepresentShinobi.mp3");
    //         if (Owner.Player != null)
    //             await PlayerCmd.GainEnergy(Amount, Owner.Player);
    //     }
    // }

    public async Task AfterLexKelaSpent(int amount, Player spender)
    {
        if (spender != Owner.Player)
        {
            return;
        }
        int num = CombatManager.Instance.History.CardPlaysStarted.Count(e =>
            e.Actor == Owner && e.HappenedThisTurn(CombatState)
        );
        if (num > 1)
        {
            return;
        }

        NinjaAudio.Play("res://LexNinja2/audio/IRepresentShinobi.mp3");
        await PlayerCmd.GainEnergy(Amount, Owner.Player);
    }
}
