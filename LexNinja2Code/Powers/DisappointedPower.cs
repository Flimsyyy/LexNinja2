using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LexNinja2.LexNinja2Code.Powers;

public class DisappointedPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type == CardType.Attack && cardPlay.Card.Owner == Owner.Player)
        {
            await PowerCmd.Apply<WeakPower>(
                new ThrowingPlayerChoiceContext(),
                Owner,
                1,
                null,
                null
            );
            await PowerCmd.Remove(this);
        }
    }

    public override string CustomPackedIconPath => "DisappointedPower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "DisappointedPower84.png".BigPowerImagePath();
}
