using BaseLib.Extensions;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LexNinja2.LexNinja2Code.Cards.Tokens;

[Pool(typeof(TokenCardPool))]
public class SpicyChicken() : LexNinja2Card(0, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<FlexPotionPower>(5)];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Food];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [CardKeyword.Exhaust, CardKeyword.Retain];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/SpicyChicken.mp3");
        await PowerCmd.Apply<FlexPotionPower>(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            DynamicVars.Power<FlexPotionPower>().BaseValue,
            Owner.Creature,
            this
        );
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<FlexPotionPower>().UpgradeValueBy(3);
    }

    public override string CustomPortraitPath => $"SpicyChicken_p.png".BigCardImagePath();
    public override string PortraitPath => $"SpicyChicken.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/SpicyChicken.png".CardImagePath();
}
