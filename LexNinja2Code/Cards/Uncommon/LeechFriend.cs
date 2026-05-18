using BaseLib.Utils;
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
        if (play.Target!.HasPower<WeakPower>())
        {
            NinjaAudio.Play("res://LexNinja2/audio/LeechFriend.mp3");
            await CommonActions.ApplySelf<StrengthPower>(choiceContext, this);
            await CommonActions.ApplySelf<DexterityPower>(choiceContext, this);
            await PowerCmd.Apply<StrengthPower>(
                choiceContext,
                play.Target,
                -DynamicVars.Strength.BaseValue,
                Owner.Creature,
                this
            );
            await PowerCmd.Apply<DexterityPower>(
                choiceContext,
                play.Target,
                -DynamicVars.Dexterity.BaseValue,
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
        return CombatState!.HittableEnemies.Any(enemy => enemy.HasPower<WeakPower>());
    }

    public override string CustomPortraitPath => $"LeechFriend_p.png".BigCardImagePath();
    public override string PortraitPath => $"LeechFriend.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/LeechFriend.png".CardImagePath();
}
