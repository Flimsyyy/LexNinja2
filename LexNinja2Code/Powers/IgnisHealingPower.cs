using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Powers;

public class IgnisHealingPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "IgnisHealingPower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "IgnisHealingPower84.png".BigPowerImagePath();

    public override async Task AfterPlayerTurnStart(
        PlayerChoiceContext choiceContext,
        Player player
    )
    {
        if (player != Owner.Player)
            return;
        NinjaAudio.Play("res://LexNinja2/audio/Die!Worm.ogg");
        await CreatureCmd.Damage(
            choiceContext,
            Owner,
            Amount,
            ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move,
            Owner
        );
    }
}
