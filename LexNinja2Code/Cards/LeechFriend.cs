using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LexNinja2.LexNinja2Code.Cards;

public class LeechFriend()
    : LexNinja2Card(2, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new PowerVar<StrengthPower>(2), new PowerVar<DexterityPower>(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<WeakPower>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Innate];
    protected override bool ShouldGlowGoldInternal => IfWeakened();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target.GetPower<WeakPower>() != null)
        {
            NinjaAudio.Play("res://LexNinja2/audio/LeechFriend.mp3");
            await PowerCmd.Apply<StrengthPower>(
                new ThrowingPlayerChoiceContext(),
                Owner.Creature,
                DynamicVars.Strength.BaseValue,
                Owner.Creature,
                this
            );
            await PowerCmd.Apply<DexterityPower>(
                new ThrowingPlayerChoiceContext(),
                Owner.Creature,
                DynamicVars.Strength.BaseValue,
                Owner.Creature,
                this
            );
            await PowerCmd.Apply<StrengthPower>(
                new ThrowingPlayerChoiceContext(),
                play.Target,
                -DynamicVars.Strength.BaseValue,
                Owner.Creature,
                this
            );
            await PowerCmd.Apply<DexterityPower>(
                new ThrowingPlayerChoiceContext(),
                play.Target,
                -DynamicVars.Strength.BaseValue,
                Owner.Creature,
                this
            );
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Strength.UpgradeValueBy(1);
        DynamicVars.Dexterity.UpgradeValueBy(1);
    }

    private bool IfWeakened()
    {
        foreach (Creature enemy in CombatState.HittableEnemies)
        {
            if (enemy.HasPower<WeakPower>())
            {
                return true;
            }
        }
        return false;
    }

    public override string CustomPortraitPath => $"LeechFriend_p.png".BigCardImagePath();
    public override string PortraitPath => $"LeechFriend.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/LeechFriend.png".CardImagePath();
}
