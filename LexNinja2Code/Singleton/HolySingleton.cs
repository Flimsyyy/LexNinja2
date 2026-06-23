using LexNinja2.LexNinja2Code.Api;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models;

namespace LexNinja2.LexNinja2Code.Singleton;

[RegisterSingleton]
public class HolySingleton() : HookedSingletonModel(HookType.Combat)
{
    public override Task AfterDamageGiven(
        PlayerChoiceContext choiceContext,
        Creature? dealer,
        DamageResult result,
        ValueProp props,
        Creature target,
        CardModel? cardSource
    )
    {
        var player = CurrentCombatState!.Players[0];
        if (dealer == null || player.Creature != dealer || cardSource == null)
        {
            return Task.CompletedTask;
        }
        if (cardSource.Tags.Contains(NinjaTags.Holy))
        {
            NinjaAudio.Play("res://LexNinja2/audio/YEEART.mp3", 0.5f);
        }

        return Task.CompletedTask;
    }
}
