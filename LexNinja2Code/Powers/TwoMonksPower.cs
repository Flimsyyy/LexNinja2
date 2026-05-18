using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Powers;

public class TwoMonksPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "TwoMonksPower.png".PowerImagePath();
    public override string? CustomBigIconPath => "TwoMonksPower.png".BigPowerImagePath();

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        if (card.Owner.Creature != base.Owner)
        {
            return playCount;
        }
        if (!card.Tags.Contains(NinjaTags.Ninjutsu))
        {
            return playCount;
        }
        if (card is LexNinja2Card lexNinjaCard)
        {
            lexNinjaCard.SetLexkelaToFreeUntilPlayed();
        }
        return playCount + 1;
    }

    public override async Task AfterModifyingCardPlayCount(CardModel card)
    {
        await PowerCmd.Decrement((PowerModel)this);
    }
}
