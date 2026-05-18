using BaseLib.Extensions;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Cards;

public class BladeDefence() : LexNinja2Card(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new PowerVar<BladeDefencePower>(2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Blade];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/BladeDefence.mp3");
        await CommonActions.ApplySelf<BladeDefencePower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<BladeDefencePower>().UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => $"BladeDefence_p.png".BigCardImagePath();
    public override string PortraitPath => $"BladeDefence.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/BladeDefence.png".CardImagePath();
}
