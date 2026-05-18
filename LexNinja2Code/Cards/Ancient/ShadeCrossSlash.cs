using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards;

public class ShadeCrossSlash()
    : LexNinja2Card(0, CardType.Attack, CardRarity.Ancient, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new NinjutsuVar(2), new PowerVar<VulnerablePower>(2), new DamageVar(10, ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<VulnerablePower>(),
            HoverTipFactory.FromKeyword(NinjaKeyword.Blade),
            HoverTipFactory.FromPower<Lexkela>(),
        ];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Blade];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Ninjutsu(choiceContext))
        {
            NinjaAudio.Play("res://LexNinja2/audio/ShadeCrossSlash.mp3");
            var nGrandFinaleVfx = NGrandFinaleVfx.Create(Owner.Creature);
            if (nGrandFinaleVfx != null)
            {
                NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(nGrandFinaleVfx);
                await Cmd.Wait(
                    NGrandFinaleVfx.totalAnticipationDuration
                );
            }
            await CommonActions.CardAttack(this, play, hitCount: 3, tmpSfx: "heavy_attack.mp3").WithHitVfxNode(NGrandFinaleImpactVfx.Create).Execute(choiceContext);
        }
        else
        {
            NinjaAudio.Play("res://LexNinja2/audio/YiCut.mp3");
            await CommonActions
                .CardAttack(this, play, vfx: "vfx/vfx_giant_horizontal_slash", tmpSfx: "slash_attack.mp3")
                .Execute(choiceContext);
        }
        await CommonActionsExtensions.Apply<VulnerablePower>(choiceContext, this, play);
    }

    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();

    protected override void OnUpgrade()
    {
        DynamicVars.Ninjutsu().UpgradeValueBy(-1);
        DynamicVars.Vulnerable.UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => "ShadeCrossSlash_p.png".BigCardImagePath();
    public override string PortraitPath => "ShadeCrossSlash.png".CardImagePath();
    public override string BetaPortraitPath => "beta/ShadeCrossSlash.png".CardImagePath();
}
