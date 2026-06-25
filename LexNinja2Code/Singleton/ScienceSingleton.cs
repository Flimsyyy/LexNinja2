using System.Threading.Tasks;
using LexNinja2.LexNinja2Code.Api;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models;

namespace LexNinja2.LexNinja2Code.Singleton;

[RegisterSingleton]
public class ScienceSingleton() : HookedSingletonModel(HookType.Combat)
{
    public override bool TryModifyEnergyCostInCombat(
        CardModel card,
        decimal originalCost,
        out decimal modifiedCost
    )
    {
        var player = card.Owner;
        if (!card.Keywords.Contains(NinjaKeyword.Science))
        {
            modifiedCost = originalCost;
            return false;
        }
        modifiedCost = originalCost - LexKela.Get(player);
        return true;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        var card = cardPlay.Card;
        if (!card.Keywords.Contains(NinjaKeyword.Science))
        {
            return;
        }
        await LexKela.Spend(card.Owner, 1, card);
    }
}
