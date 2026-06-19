using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LexNinja2.LexNinja2Code.Cards.Uncommons;

public class TrueLeechFriend()
    : LexNinja2Card(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Leech", 3)];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [CardKeyword.Innate, CardKeyword.Retain];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<StrengthPower>()];
    protected override bool ShouldGlowGoldInternal =>
        CombatState != null && CombatState.Allies.Any(CanLeechFriend);
    public override CardMultiplayerConstraint MultiplayerConstraint =>
        CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target != null && !CanLeechFriend(play.Target))
        {
            return;
        }
        NinjaAudio.Play("res://LexNinja2/audio/LeechFriend.mp3");
        await PowerCmd.Apply<FlexPotionPower>(
            choiceContext,
            Owner.Creature,
            DynamicVars["Leech"].BaseValue,
            Owner.Creature,
            this
        );
        if (play.Target != null)
            await PowerCmd.Apply<DarkShacklesPower>(
                choiceContext,
                play.Target,
                DynamicVars["Leech"].BaseValue,
                Owner.Creature,
                this
            );
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Leech"].UpgradeValueBy(2);
    }

    private bool CanLeechFriend(Creature target)
    {
        if (
            CombatManager.Instance.History.CardPlaysFinished.Count(
                (CardPlayFinishedEntry e) =>
                    e.HappenedThisTurn(base.CombatState) && e.CardPlay.Card.Owner == target.Player
            )
            < CombatManager.Instance.History.CardPlaysFinished.Count(
                (CardPlayFinishedEntry e) =>
                    e.HappenedThisTurn(base.CombatState) && e.CardPlay.Card.Owner == Owner
            )
        )
        {
            return true;
        }
        return false;
    }

    public override string CustomPortraitPath => $"TrueLeechFriend.png".BigCardImagePath();
    public override string PortraitPath => $"TrueLeechFriend.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/TrueLeechFriend.png".CardImagePath();
}
