using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace LexNinja2.LexNinja2Code.Cards.Curses;

[RegisterCard(typeof(CurseCardPool))]
public class HamoodKillAll()
    : LexNinja2BaseCard(1, CardType.Curse, CardRarity.Curse, TargetType.None)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CalculationBaseVar(0),
            new ExtraDamageVar(1),
            new CalculatedDamageVar(ValueProp.Unpowered).WithMultiplier(
                (card, _) =>
                {
                    var baseDamage = 999;
                    if (card.Owner.Creature.CombatState == null)
                        return baseDamage;
                    for (var i = 1; i < card.Owner.Creature.CombatState.RunState.Players.Count; i++)
                    {
                        baseDamage *= 10;
                        baseDamage += 9;
                    }
                    return baseDamage;
                }
            ),
        ];
    public override int MaxUpgradeLevel => 0;

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        NinjaAudio.Play("res://LexNinja2/audio/KillAll.mp3");
        await CreatureCmd.Damage(
            choiceContext,
            Owner.Creature.CombatState!.Creatures.Where(c => !c.IsPet),
            DynamicVars.CalculatedDamage.Calculate(null),
            DynamicVars.CalculatedDamage.Props,
            Owner.Creature // need not null
        );
        NinjaAudio.Play("res://LexNinja2/audio/Kill!@#A%ll.mp3");
    }

    public override bool HasTurnEndInHandEffect => true;

    public override string CustomPortraitPath => "KillAll_p.png".BigCardImagePath();
    public override string PortraitPath => "KillAll.png".CardImagePath();
    public override string BetaPortraitPath => "beta/KillAll.png".CardImagePath();
}
