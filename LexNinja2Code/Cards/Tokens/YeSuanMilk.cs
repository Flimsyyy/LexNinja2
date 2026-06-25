using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.Interop.AutoRegistration;

namespace LexNinja2.LexNinja2Code.Cards.Tokens;

[RegisterCard(typeof(TokenCardPool))]
public class YeSuanMilk() : LexNinja2BaseCard(0, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new LexKelaVar(3)];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Food];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/YeSuanMilk.mp3");
        await LexKela.Gain(this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.LexKela().UpgradeValueBy(2);
    }

    public override string CustomPortraitPath => $"YeSuanMilk_p.png".BigCardImagePath();
    public override string PortraitPath => $"YeSuanMilk.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/YeSuanMilk.png".CardImagePath();
}
