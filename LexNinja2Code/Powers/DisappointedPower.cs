using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LexNinja2.LexNinja2Code.Powers;

public class DisappointedPower : LexNinja2Power
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type == CardType.Attack && cardPlay.Card.Owner == Owner.Player)
        {
            var weakSelf = await PowerCmd.Apply<WeakPower>(context, Owner, 1, null, null);
            weakSelf!.SkipNextDurationTick = false;
            await PowerCmd.Remove(this);
        }
    }

    public override string CustomIconPath => "DisappointedPower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "DisappointedPower84.png".BigPowerImagePath();
}
