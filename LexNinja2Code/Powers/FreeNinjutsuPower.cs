using System.Linq;
using System.Threading.Tasks;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Combat.SecondaryResources;

namespace LexNinja2.LexNinja2Code.Powers;

public class FreeNinjutsuPower : LexNinja2Power, ISecondaryResourceHookListener
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomIconPath => "DimDeadTreePower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "DimDeadTreePower84.png".BigPowerImagePath();

    public decimal ModifySecondaryResourceCost(SecondaryResourceCostContext context, decimal cost)
    {
        if (context.Definition.Id != LexKela.Id)
        {
            return cost;
        }
        var card = context.Card;
        if (card.Owner.Creature != Owner || !card.Tags.Contains(NinjaTags.Ninjutsu))
        {
            return cost;
        }
        return 0;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var card = cardPlay.Card;
        if (card.Owner.Creature != Owner || !card.Tags.Contains(NinjaTags.Ninjutsu))
        {
            return;
        }
        await PowerCmd.Decrement(this);
    }
}
