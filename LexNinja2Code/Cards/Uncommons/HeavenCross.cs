using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Cards.Uncommons;

public class HeavenCross() : LexNinja2Card(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<HeavenCrossPower>(1)];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [LexKela.HoverTip(), HoverTipFactory.FromKeyword(NinjaKeyword.RenShu)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/HeavenCross.mp3");
        await CommonActions.ApplySelf<HeavenCrossPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }

    public override string CustomPortraitPath => $"HeavenCross_p.png".BigCardImagePath();
    public override string PortraitPath => $"HeavenCross.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/HeavenCross.png".CardImagePath();
}
