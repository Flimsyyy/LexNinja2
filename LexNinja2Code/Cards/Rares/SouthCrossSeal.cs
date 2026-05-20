using BaseLib.Extensions;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards.Rares;

public class SouthCrossSeal()
    : LexNinja2Card(3, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new PowerVar<SealPower>(2), new NinjutsuVar(3)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/SouthCrossSeal.wav");
        await CreatureCmd.Stun(play.Target!);
        await CreatureCmd.GainBlock(play.Target!, 50, ValueProp.Unpowered, play);
        if (!await Ninjutsu(choiceContext, play))
        {
            return;
        }
        await CommonActions.Apply<SealPower>(choiceContext, this, play);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<SealPower>().UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => $"SouthCrossSeal_p.png".BigCardImagePath();
    public override string PortraitPath => $"SouthCrossSeal.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/SouthCrossSeal.png".CardImagePath();

    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
