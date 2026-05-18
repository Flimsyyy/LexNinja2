using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards;

public class YiCut()
    : LexNinja2Card(0, CardType.Attack, CardRarity.Basic, TargetType.AllEnemies),
        ITranscendenceCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new NinjutsuVar(2), new PowerVar<VulnerablePower>(1), new DamageVar(10, ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<VulnerablePower>(), HoverTipFactory.FromPower<Lexkela>()];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Blade];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/YiCut.mp3");
        decimal Renshu = DynamicVars["Renshu"].BaseValue;
        Lexkela kela = Owner.Creature.GetPower<Lexkela>();
        if (Ninjutsu())
        {
            await DamageCmd
                .Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .TargetingAllOpponents(CombatState)
                .WithHitFx("vfx/vfx_giant_horizontal_slash", tmpSfx: "slash_attack.mp3")
                .Execute(choiceContext);
        }
        await PowerCmd.Apply<VulnerablePower>(
            new ThrowingPlayerChoiceContext(),
            CombatState.HittableEnemies,
            DynamicVars.Vulnerable.BaseValue,
            Owner.Creature,
            this
        );
    }

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

    protected override void OnUpgrade()
    {
        DynamicVars["Renshu"].UpgradeValueBy(-1);
    }

    public override string CustomPortraitPath => "YiCut_p.png".BigCardImagePath();
    public override string PortraitPath => "YiCut.png".CardImagePath();
    public override string BetaPortraitPath => "beta/YiCut.png".CardImagePath();

    public CardModel GetTranscendenceTransformedCard() => ModelDb.Card<ShadeCrossSlash>();
}
