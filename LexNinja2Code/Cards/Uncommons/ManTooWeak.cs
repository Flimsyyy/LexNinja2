using BaseLib.Extensions;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LexNinja2.LexNinja2Code.Cards.Uncommons;

public class ManTooWeak() : LexNinja2Card(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<StrengthPower>(5),
            new PowerVar<DexterityPower>(5),
            new PowerVar<ManTooWeakPower>(2),
        ];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [LexKela.HoverTip()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Hand];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/ManTooWeak.mp3");
        await CommonActions.ApplySelf<StrengthPower>(choiceContext, this);
        await CommonActions.ApplySelf<DexterityPower>(choiceContext, this);
        await CommonActions.ApplySelf<ManTooWeakPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<ManTooWeakPower>().UpgradeValueBy(-1);
    }

    public override string CustomPortraitPath => $"ManTooWeak_p.png".BigCardImagePath();
    public override string PortraitPath => $"ManTooWeak.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/ManTooWeak.png".CardImagePath();
}
