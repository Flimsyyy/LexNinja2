using BaseLib.Extensions;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards;

public class ShadowBlade()
    : LexNinja2Card(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(12, ValueProp.Move), new PowerVar<VulnerablePower>(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Blade];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/ShadowBlade.mp3");
        await DamageCmd
            .Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash", tmpSfx: "heavy_attack.mp3")
            .Execute(choiceContext);
        await PowerCmd.Apply<VulnerablePower>(
            new ThrowingPlayerChoiceContext(),
            play.Target,
            DynamicVars.Power<VulnerablePower>().BaseValue,
            Owner.Creature,
            this
        );
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1)
        {
            PretendCardsCanBePlayed = true,
        };
        CardModel card = (
            await CardSelectCmd.FromHand(
                choiceContext,
                Owner,
                prefs,
                (Func<CardModel, bool>)null,
                this
            )
        ).FirstOrDefault();
        if (card == null)
        {
            card = (CardModel)null;
        }
        else
        {
            CardCmd.ApplyKeyword(card, NinjaKeyword.Blade);
            await CardCmd.AutoPlay(choiceContext, card, (Creature)null);
            await CardCmd.Exhaust(choiceContext, card);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars.Power<VulnerablePower>().UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => $"ShadowBlade_p.png".BigCardImagePath();
    public override string PortraitPath => $"ShadowBlade.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/ShadowBlade.png".CardImagePath();
}
