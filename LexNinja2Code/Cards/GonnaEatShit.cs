using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards;

public class GonnaEatShit() : LexNinja2Card(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(3, ValueProp.Move | ValueProp.Unblockable),
            new LexKelaVar(2),
            new EnergyVar(1),
        ];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/GonnaEatShit.mp3");
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await CreatureCmd.Damage(
            choiceContext,
            Owner.Creature,
            DynamicVars.Damage.BaseValue,
            ValueProp.Move | ValueProp.Unblockable,
            this
        );
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        await PowerCmd.Apply<Lexkela>(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            DynamicVars["Kela"].BaseValue,
            Owner.Creature,
            this
        );
        await PowerCmd.Apply<ShitPower>(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            DynamicVars["Kela"].BaseValue,
            Owner.Creature,
            this
        );
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Kela"].UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => $"GonnaEatShit_p.png".BigCardImagePath();
    public override string PortraitPath => $"GonnaEatShit.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/GonnaEatShit.png".CardImagePath();
}
