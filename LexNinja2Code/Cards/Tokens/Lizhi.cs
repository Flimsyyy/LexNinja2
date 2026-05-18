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
public class Lizhi() : LexNinja2Card(0, CardType.Skill, CardRarity.Token, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new PowerVar<VulnerablePower>(3), new PowerVar<WeakPower>(3)];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Food];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [CardKeyword.Exhaust, CardKeyword.Retain];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/Lizhi.mp3");
        await PowerCmd.Apply<WeakPower>(
            new ThrowingPlayerChoiceContext(),
            play.Target,
            DynamicVars.Weak.BaseValue,
            Owner.Creature,
            this
        );
        await PowerCmd.Apply<VulnerablePower>(
            new ThrowingPlayerChoiceContext(),
            play.Target,
            DynamicVars.Weak.BaseValue,
            Owner.Creature,
            this
        );
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<WeakPower>().UpgradeValueBy(2);
        DynamicVars.Power<VulnerablePower>().UpgradeValueBy(2);
    }

    public override string CustomPortraitPath => $"Lizhi_p.png".BigCardImagePath();
    public override string PortraitPath => $"Lizhi.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/Lizhi.png".CardImagePath();
}
