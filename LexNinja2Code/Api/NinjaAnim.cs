using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Api;

public static class NinjaAnim
{
    public static async Task TriggerAnim(Creature creature, string animName, float delay)
    {
        await CreatureCmd.TriggerAnim(creature, animName, delay);
    }

    public static async Task TriggerCastAnim(Creature creature, float delay)
    {
        await TriggerAnim(creature, "Cast", delay);
    }

    public static async Task TriggerCastAnim(CardModel card)
    {
        await TriggerCastAnim(card.Owner.Creature, card.Owner.Character.CastAnimDelay);
    }

    public static async Task TriggerAttackAnim(Creature creature, float delay)
    {
        await TriggerAnim(creature, "Attack", delay);
    }

    public static async Task TriggerAttackAnim(CardModel card)
    {
        await TriggerCastAnim(card.Owner.Creature, card.Owner.Character.CastAnimDelay);
    }
}
