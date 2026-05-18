using BaseLib.Extensions;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards.Uncommons;

public class SharpenToKill()
    : LexNinja2Card(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(9, ValueProp.Move), new PowerVar<BladePowerUp>(2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Blade];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/SharpenToKill.mp3");
        await CommonActions
            .CardAttack(this, play, vfx: "vfx/vfx_attack_slash")
            .Execute(choiceContext);
        await CommonActions.ApplySelf<BladePowerUp>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
        DynamicVars.Power<BladePowerUp>().UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => $"SharpenToKill_p.png".BigCardImagePath();
    public override string PortraitPath => $"SharpenToKill.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/SharpenToKill.png".CardImagePath();
}
