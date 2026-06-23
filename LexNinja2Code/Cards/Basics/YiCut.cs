using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Cards.Ancients;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace LexNinja2.LexNinja2Code.Cards.Basics;

[RegisterCharacterStarterCard(typeof(Character.LexNinja2), Order = 2)]
[RegisterArchaicToothTranscendence(typeof(ShadeCrossSlash))]
public class YiCut()
    : LexNinja2NinjutsuCard(0, CardType.Attack, CardRarity.Basic, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new NinjutsuVar(2), new PowerVar<VulnerablePower>(1), new DamageVar(10, ValueProp.Move)];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [HoverTipFactory.FromPower<VulnerablePower>(), LexKela.HoverTip()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Blade];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/YiCut.mp3");
        if (Ninjutsu(play))
        {
            await CommonActions
                .CardAttack(
                    this,
                    play,
                    vfx: "vfx/vfx_giant_horizontal_slash",
                    tmpSfx: "slash_attack.mp3"
                )
                .Execute(choiceContext);
        }
        await CommonActions.Apply<VulnerablePower>(choiceContext, this, play);
    }

    protected override void OnUpgrade()
    {
        UpgradeNinjutsuValueBy(-1);
    }

    public override string CustomPortraitPath => "YiCut_p.png".BigCardImagePath();
    public override string PortraitPath => "YiCut.png".CardImagePath();
    public override string BetaPortraitPath => "beta/YiCut.png".CardImagePath();
}
