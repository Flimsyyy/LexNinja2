using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Cards;

public class RadiationAnnihilation()
    : LexNinja2Card(6, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<Lexkela>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Science];
    public override CardMultiplayerConstraint MultiplayerConstraint =>
        CardMultiplayerConstraint.SingleplayerOnly;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/RadiationAnnihilation.mp3", 1f);
        await PowerCmd.Apply<Radiation>(
            new ThrowingPlayerChoiceContext(),
            CombatState.HittableEnemies,
            25,
            Owner.Creature,
            this
        );
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public override string CustomPortraitPath => $"RadiationAnnihilation_p.png".BigCardImagePath();
    public override string PortraitPath => $"RadiationAnnihilation.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/RadiationAnnihilation.png".CardImagePath();
}
