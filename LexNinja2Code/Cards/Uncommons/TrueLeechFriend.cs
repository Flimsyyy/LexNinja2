using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LexNinja2.LexNinja2Code.Cards.Uncommons;

public class TrueLeechFriend()
    : LexNinja2Card(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Leech", 3)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Innate];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<WeakPower>(), HoverTipFactory.FromPower<StrengthPower>()];
    protected override bool ShouldGlowGoldInternal => IfWeakened();
    public override CardMultiplayerConstraint MultiplayerConstraint =>
        CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/LeechFriend.mp3");
        await PowerCmd.Apply<StrengthPower>(
            choiceContext,
            Owner.Creature,
            DynamicVars["Leech"].BaseValue,
            Owner.Creature,
            this
        );
        await PowerCmd.Apply<StrengthPower>(
            choiceContext,
            play.Target,
            -DynamicVars["Leech"].BaseValue,
            Owner.Creature,
            this
        );
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Leech"].UpgradeValueBy(2);
    }

    private bool IfWeakened()
    {
        return CombatState!.Allies.Any(ally => ally.HasPower<WeakPower>());
    }

    public override string CustomPortraitPath => $"TrueLeechFriend.png".BigCardImagePath();
    public override string PortraitPath => $"TrueLeechFriend.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/TrueLeechFriend.png".CardImagePath();
}
