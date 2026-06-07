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

namespace LexNinja2.LexNinja2Code.Cards.Rares;

public class IRepresentShinobi()
    : LexNinja2Card(3, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new PowerVar<IRepresentShinobiPower>(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<Lexkela>(), HoverTipFactory.Static(StaticHoverTip.Energy)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/IRepresentShinobi.mp3");
        await CommonActions.ApplySelf<IRepresentShinobiPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<IRepresentShinobiPower>().UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => $"IRepresentShinobi_p.png".BigCardImagePath();
    public override string PortraitPath => $"IRepresentShinobi.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/IRepresentShinobi.png".CardImagePath();
}
