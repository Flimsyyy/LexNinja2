using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Powers;

public class Pain : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    private bool _shouldIgnoreNextInstance;

    public override string CustomPackedIconPath => "PainPower.png".PowerImagePath();
    public override string? CustomBigIconPath => "PainPower.png".BigPowerImagePath();

    private int flag = 0;

    public override async Task BeforeApplied(
        Creature target,
        Decimal amount,
        Creature? applier,
        CardModel? cardSource
    )
    {
        if (this._shouldIgnoreNextInstance)
        {
            this._shouldIgnoreNextInstance = false;
        }
        else
        {
            NinjaAudio.Play("res://LexNinja2/audio/Painful.mp3");
        }
    }

    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        Decimal amount,
        Creature? applier,
        CardModel? cardSource
    )
    {
        Lexkela lexkela = power as Lexkela;
        if (
            Owner.GetPower<Lexkela>() != null
            && power == lexkela
            && amount < 0
            && power.Owner == Owner
        )
        {
            flag = 1;
        }
    }

    public override async Task AfterPlayerTurnStart(
        PlayerChoiceContext choiceContext,
        Player player
    )
    {
        if (player != Owner.Player)
            return;
        flag = 0;
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != this.Owner.Side)
            return;
        if (flag == 1)
            return;
        await PowerCmd.Apply<Lexkela>(new ThrowingPlayerChoiceContext(), Owner, 1, null, null);
    }

    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource
    )
    {
        if (target == base.Owner && result.UnblockedDamage > 0)
        {
            Flash();
            NinjaAudio.Play("res://LexNinja2/audio/Pain.mp3");
        }
        return;
    }
}
