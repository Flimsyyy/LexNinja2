using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace LexNinja2.LexNinja2Code.Powers;

public class FreeNinjutsuPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override string CustomPackedIconPath => "DimDeadTreePower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "DimDeadTreePower84.png".BigPowerImagePath();

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player)
        {
            return;
        }
        // if (cardPlay.Card.BaseReplayCount >1)
        // {
        //     return;
        // }
        if (cardPlay.Card.Keywords.Contains(NinjaKeyword.FreeNinjutsu))
        {
            return;
        }
        if (cardPlay.Card.Tags.Contains(NinjaTags.Ninjutsu))
        {
            await PowerCmd.Decrement(this);
        }
    }
}
