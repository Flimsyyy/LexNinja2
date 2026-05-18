using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Powers;

public class SciencePower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "SciencePower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "SciencePower84.png".BigPowerImagePath();

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        if (card.Owner.Creature != base.Owner)
        {
            return playCount;
        }
        if (!card.Keywords.Contains(NinjaKeyword.Science))
        {
            return playCount;
        }
        return playCount + 1;
    }

    public override async Task AfterModifyingCardPlayCount(CardModel card)
    {
        await PowerCmd.Decrement((PowerModel)this);
    }
}
