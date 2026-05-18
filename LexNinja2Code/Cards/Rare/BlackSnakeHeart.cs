using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LexNinja2.LexNinja2Code.Cards;

public class BlackSnakeHeart() : LexNinja2Card(4, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new PowerVar<BlackSnakeHeartPower>(1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<IntangiblePower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/BlackSnakeHeart.mp3");
        await CommonActions.ApplySelf<BlackSnakeHeartPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public override string CustomPortraitPath => $"BlackSnakeHeart.png".BigCardImagePath();
    public override string PortraitPath => $"BlackSnakeHeart.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/BlackSnakeHeart.png".CardImagePath();
}
