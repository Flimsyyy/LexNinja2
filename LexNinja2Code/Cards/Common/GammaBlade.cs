using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards;

public class GammaBlade()
    : LexNinja2Card(5, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(7, ValueProp.Move),
            new PowerVar<VulnerablePower>(2),
            new PowerVar<WeakPower>(2),
        ];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [NinjaKeyword.Blade, NinjaKeyword.Science];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<Lexkela>(),
            HoverTipFactory.FromPower<WeakPower>(),
            HoverTipFactory.FromPower<VulnerablePower>(),
        ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/GammaBlade.mp3");
        VfxCmd.PlayOnCreature(Owner.Creature, "vfx/vfx_sweeping_beam");
        await Cmd.Wait(0.1f);
        await CommonActions.CardAttack(this, play, hitCount: 2, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
        await CommonActionsExtensions.Apply<WeakPower>(choiceContext, this, play);
        await CommonActionsExtensions.Apply<VulnerablePower>(choiceContext, this, play);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }

    public override string CustomPortraitPath => "GammaBlade.png".BigCardImagePath();
    public override string PortraitPath => "GammaBlade.png".CardImagePath();
    public override string BetaPortraitPath => "beta/GammaBlade.png".CardImagePath();
}
