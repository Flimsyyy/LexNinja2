using System.Collections.Generic;
using System.Threading.Tasks;
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
            new PowerVar<StrengthPower>(NinjaHelper.GetValueByChallengeMode(3, 5)),
            new PowerVar<DexterityPower>(NinjaHelper.GetValueByChallengeMode(3, 5)),
            new PowerVar<ManTooWeakPower>(NinjaHelper.GetValueByChallengeMode(1, 2)),
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
        if (NinjaConfigsPage.IsChallengeMode())
        {
            DynamicVars.Power<StrengthPower>().UpgradeValueBy(1);
            DynamicVars.Power<DexterityPower>().UpgradeValueBy(1);
        }
        else
        {
            DynamicVars.Power<ManTooWeakPower>().UpgradeValueBy(-1);
        }
    }

    public override string CustomPortraitPath => $"ManTooWeak_p.png".BigCardImagePath();
    public override string PortraitPath => $"ManTooWeak.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/ManTooWeak.png".CardImagePath();
}
