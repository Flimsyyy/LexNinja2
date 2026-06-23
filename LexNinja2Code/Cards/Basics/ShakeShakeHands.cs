using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;

namespace LexNinja2.LexNinja2Code.Cards.Basics;

[RegisterCharacterStarterCard(typeof(Character.LexNinja2), Order = 3)]
public class ShakeShakeHands()
    : LexNinja2Card(0, CardType.Skill, CardRarity.Basic, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new PowerVar<WeakPower>(1), new LexKelaVar(1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [CardKeyword.Innate, NinjaKeyword.Hand];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [HoverTipFactory.FromPower<WeakPower>(), HoverTipFactory.FromKeyword(NinjaKeyword.Hand)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/ShakeShakeHand.mp3");
        await CommonActions.ApplySelf<WeakPower>(choiceContext, this);
        await CommonActions.Apply<WeakPower>(choiceContext, this, play);
        await LexKela.Gain(this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.LexKela().UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => $"ShakeShakeHands_p.png".BigCardImagePath();
    public override string PortraitPath => $"ShakeShakeHands.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/ShakeShakeHands.png".CardImagePath();
}
