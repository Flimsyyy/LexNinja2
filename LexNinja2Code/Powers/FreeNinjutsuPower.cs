using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Hooks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Powers;

public class FreeNinjutsuPower : CustomPowerModel, ITryModifyLexKelaCost, IAfterLexKelaSpent
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "DimDeadTreePower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "DimDeadTreePower84.png".BigPowerImagePath();

    public bool TryModifyLexKeLaCost(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        if (card.Owner.Creature != Owner)
        {
            modifiedCost = originalCost;
            return false;
        }
        modifiedCost = 0;
        return true;
    }

    public async Task AfterLexKelaSpent(
        PlayerChoiceContext choiceContext,
        int amount,
        Player spender
    )
    {
        if (spender.Creature != Owner)
        {
            return;
        }
        if (amount > 0)
        {
            return;
        }
        await PowerCmd.Decrement(this);
    }
}
