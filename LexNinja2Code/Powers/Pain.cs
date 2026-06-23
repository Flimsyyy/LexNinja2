using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Powers;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Powers;

public class Pain : LexNinja2Power
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override string CustomIconPath => "PainPower.png".PowerImagePath();
    public override string? CustomBigIconPath => "PainPower.png".BigPowerImagePath();

    public override async Task BeforeApplied(
        Creature target,
        decimal amount,
        Creature? applier,
        CardModel? cardSource
    )
    {
        NinjaAudio.Play("res://LexNinja2/audio/Painful.mp3");
    }

    public override Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource
    )
    {
        if (target != Owner || result.UnblockedDamage <= 0)
            return Task.CompletedTask;
        Flash();
        NinjaAudio.Play("res://LexNinja2/audio/Pain.mp3");
        return Task.CompletedTask;
    }
}
