using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards.Curses;

[Pool(typeof(TokenCardPool))]
public class HamoodKillAll() : LexNinja2Card(1, CardType.Curse, CardRarity.Curse, TargetType.None)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(999, ValueProp.Unpowered)];
    public override int MaxUpgradeLevel => 0;

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        for (var i = 1; i < Owner.Creature.CombatState!.RunState.Players.Count; i++)
        {
            DynamicVars.Damage.BaseValue *= 10;
            DynamicVars.Damage.BaseValue += 9;
        }

        NinjaAudio.Play("res://LexNinja2/audio/KillAll.mp3");
        await CreatureCmd.Damage(
            choiceContext,
            Owner.Creature.CombatState.Creatures.Where(
                c => !c.IsPet
            ),
            DynamicVars.Damage,
            Owner.Creature // need not null
        );
        NinjaAudio.Play("res://LexNinja2/audio/Kill!@#A%ll.mp3");
    }

    public override bool HasTurnEndInHandEffect => true;

    public override string CustomPortraitPath => "KillAll_p.png".BigCardImagePath();
    public override string PortraitPath => "KillAll.png".CardImagePath();
    public override string BetaPortraitPath => "beta/KillAll.png".CardImagePath();
}
