using BaseLib.Extensions;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LexNinja2.LexNinja2Code.Cards;

public class BlackDragonHand()
    : LexNinja2Card(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("HP", 2), new NinjutsuVar(2), new PowerVar<WeakPower>(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<IntangiblePower>(), HoverTipFactory.FromPower<WeakPower>()];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];
    public override bool CanBeGeneratedInCombat => false;

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [CardKeyword.Ethereal, NinjaKeyword.Hand, CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/BlackDragonHand.mp3");
        await PowerCmd.Apply<WeakPower>(
            new ThrowingPlayerChoiceContext(),
            play.Target,
            DynamicVars.Power<WeakPower>().BaseValue,
            Owner.Creature,
            this
        );
        if (play.Target.HasPower<IntangiblePower>())
        {
            await PowerCmd.Remove<IntangiblePower>(play.Target);
        }

        if (Ninjutsu())
        {
            VfxCmd.PlayOnCreatureCenter(play.Target, "vfx/vfx_attack_lightning");
            SfxCmd.Play("lightning_orb_evoke.mp3");
            await CreatureCmd.LoseMaxHp(
                choiceContext,
                play.Target,
                DynamicVars["HP"].BaseValue,
                true
            );
            await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars["HP"].BaseValue);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["HP"].UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => $"BlackDragonHand_p.png".BigCardImagePath();
    public override string PortraitPath => $"BlackDragonHand.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/BlackDragonHand.png".CardImagePath();

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
