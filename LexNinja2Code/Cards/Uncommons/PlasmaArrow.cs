using BaseLib.Utils;
using Godot;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards.Uncommons;

public class PlasmaArrow()
    : LexNinja2Card(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(12, ValueProp.Move), new PowerVar<PlasmaArrowPower>(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(NinjaKeyword.Renshu)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/PlasmaArrow.mp3");
        await CommonActions
            .CardAttack(this, play)
            .WithHitVfxNode(t => NShivThrowVfx.Create(base.Owner.Creature, t, Colors.Aqua))
            .Execute(choiceContext);
        await CommonActions.ApplySelf<PlasmaArrowPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6);
    }

    public override string CustomPortraitPath => $"PlasmaArrow.png".BigCardImagePath();
    public override string PortraitPath => $"PlasmaArrow.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/PlasmaArrow.png".CardImagePath();
}
