using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Powers;

public class SealPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "SealPower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "SealPower84.png".BigPowerImagePath();

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != this.Owner.Side)
        {
            return;
        }
        Flash();
        NinjaAudio.Play("res://LexNinja2/audio/SouthCrossSeal.wav");
        await CreatureCmd.Stun(Owner);
        await CreatureCmd.GainBlock(Owner, 50, ValueProp.Unpowered, null);
        await PowerCmd.Decrement(this);
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
        if (target == base.Owner && result.UnblockedDamage != 0)
        {
            // Creature creature = dealer;
            // if (dealer.Monster is Osty)
            // {
            //     creature = dealer.PetOwner.Creature;
            // }
            // if (creature.Player != null||props.IsPoweredAttack())
            // {
            Flash();
            await PowerCmd.Remove(this);
            // }
        }
    }
}
