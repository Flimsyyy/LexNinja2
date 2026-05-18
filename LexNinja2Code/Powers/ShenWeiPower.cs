using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LexNinja2.LexNinja2Code.Powers;

public class ShenWeiPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "ShenWeiPower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "ShenWeiPower84.png".BigPowerImagePath();

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (Owner.GetPower<IntangiblePower>() == null)
        {
            return;
        }

        if (cardPlay.Card.Owner != Owner.Player)
        {
            return;
        }
        if (cardPlay.Card.Type == CardType.Attack)
        {
            await PowerCmd.Apply<IntangiblePower>(
                new ThrowingPlayerChoiceContext(),
                Owner,
                -1,
                null,
                null
            );
        }
    }

    public override async Task AfterSideTurnStart(CombatSide side, ICombatState combatState)
    {
        if (side != Owner.Side)
            return;
        if (Owner.GetPower<Lexkela>() == null)
        {
            return;
        }
        NinjaAudio.Play("res://LexNinja2/audio/ShenWei.mp3");
        Flash();
        await PowerCmd.Apply<IntangiblePower>(
            new ThrowingPlayerChoiceContext(),
            Owner,
            Amount,
            null,
            null
        );
        await PowerCmd.Apply<Lexkela>(new ThrowingPlayerChoiceContext(), Owner, -1, null, null);
    }
}
