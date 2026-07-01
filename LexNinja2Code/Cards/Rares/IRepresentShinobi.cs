using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Cards.Rares;

public class IRepresentShinobi()
    : LexNinja2Card(3, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new EnergyVar(NinjaHelper.GetValueByChallengeMode(2, 1))];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        NinjaHelper.GetValueByChallengeMode(base.CanonicalKeywords, [CardKeyword.Ethereal]);
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [LexKela.HoverTip(), HoverTipFactory.Static(StaticHoverTip.Energy)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/IRepresentShinobi.mp3");
        await CommonActions.ApplySelf<IRepresentShinobiPower>(
            choiceContext,
            this,
            DynamicVars.Energy.BaseValue
        );
    }

    protected override void OnUpgrade()
    {
        if (NinjaConfig.IsChallengeMode())
        {
            DynamicVars.Energy.UpgradeValueBy(1);
        }
        else
        {
            RemoveKeyword(CardKeyword.Ethereal);
        }
    }

    public override string CustomPortraitPath => $"IRepresentShinobi_p.png".BigCardImagePath();
    public override string PortraitPath => $"IRepresentShinobi.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/IRepresentShinobi.png".CardImagePath();
}
