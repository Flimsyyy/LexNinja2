using BaseLib.Extensions;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Cards.Rares;

public class InfernoDragon() : LexNinja2Card(5, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new PowerVar<InfernoDragonPower>(8)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<Lexkela>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Science];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/InfernoDragon.mp3");
        await CommonActions.ApplySelf<InfernoDragonPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<InfernoDragonPower>().UpgradeValueBy(4);
    }

    public override string CustomPortraitPath => $"InfernoDragon.png".BigCardImagePath();
    public override string PortraitPath => $"InfernoDragon.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/InfernoDragon.png".CardImagePath();
}
