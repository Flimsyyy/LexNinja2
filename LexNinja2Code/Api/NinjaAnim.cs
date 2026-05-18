using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Api;

public static class NinjaAnim
{
    public static async Task TriggerCastAnim(CardModel card)
    {
        await CreatureCmd.TriggerAnim(
            card.Owner.Creature,
            "Cast",
            card.Owner.Character.CastAnimDelay
        );
    }
}
