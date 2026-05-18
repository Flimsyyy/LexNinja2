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

public class FourNightsLightning()
    : LexNinja2Card(4, CardType.Attack, CardRarity.Rare, TargetType.RandomEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(4, ValueProp.Move), new NinjutsuVar(4), new RepeatVar(3)];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/FourNightsLightning.mp3", 1.5f);
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await MegaCrit.Sts2.Core.Commands.Cmd.Wait(0.5f);
        if (Ninjutsu())
        {
            int repeatDamageCount = 4 + 4 * DynamicVars.Repeat.IntValue;
            await DamageCmd
                .Attack(DynamicVars.Damage.BaseValue)
                .WithHitCount(repeatDamageCount)
                .FromCard(this)
                .TargetingRandomOpponents(CombatState)
                .WithHitFx("vfx/vfx_attack_lightning", tmpSfx: "lightning_orb_evoke.mp3")
                .Execute(choiceContext);
            for (int i = 0; i < DynamicVars.Repeat.BaseValue + 1; i++)
            {
                await CardPileCmd.Draw(choiceContext, 1, Owner);
                await PlayerCmd.GainEnergy(1, Owner);
            }
            return;
        }
        await DamageCmd
            .Attack(DynamicVars.Damage.BaseValue)
            .WithHitCount(4)
            .FromCard(this)
            .TargetingRandomOpponents(CombatState)
            .WithHitFx("vfx/vfx_attack_lightning", tmpSfx: "lightning_orb_evoke.mp3")
            .Execute(choiceContext);
        await CardPileCmd.Draw(choiceContext, 1, Owner);
        await PlayerCmd.GainEnergy(1, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => $"FourNightsLightning_p.png".BigCardImagePath();
    public override string PortraitPath => $"FourNightsLightning.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/FourNightsLightning.png".CardImagePath();

    private Boolean Ninjutsu()
    {
        if (Owner.Creature.GetPower<FreeNinjutsuPower>() != null)
        {
            return true;
        }
        if (Owner.Creature.GetPower<Lexkela>() != null)
        {
            if (Owner.Creature.GetPower<Lexkela>().Amount >= DynamicVars["Renshu"].BaseValue)
            {
                PowerCmd.Apply<Lexkela>(
                    new ThrowingPlayerChoiceContext(),
                    Owner.Creature,
                    -DynamicVars["Renshu"].BaseValue,
                    Owner.Creature,
                    this
                );
                return true;
            }
        }
        return false;
    }

    private Boolean CanCastNinjutsu()
    {
        if (Owner.Creature.GetPower<FreeNinjutsuPower>() != null)
        {
            return true;
        }

        if (Owner.Creature.GetPower<Lexkela>() != null)
        {
            if (Owner.Creature.GetPower<Lexkela>().Amount >= DynamicVars["Renshu"].BaseValue)
            {
                return true;
            }
        }

        return false;
    }

    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
