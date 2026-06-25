using System.Collections.Generic;
using System.Threading.Tasks;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LexNinja2.LexNinja2Code.Powers;

public class ShenWeiPower : LexNinja2Power
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomIconPath => "ShenWeiPower32.png".PowerImagePath();
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
            await PowerCmd.Apply<IntangiblePower>(context, Owner, -1, Owner, null);
        }
    }

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> creatures,
        ICombatState combatState
    )
    {
        if (side != Owner.Side || Owner.Player == null)
            return;
        if (!await LexKela.Spend(Owner.Player, 1, null, this))
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
    }
}
