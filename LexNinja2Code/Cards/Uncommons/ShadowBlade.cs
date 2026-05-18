using BaseLib.Extensions;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards.Uncommons;

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
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_slash", tmpSfx: "heavy_attack.mp3")
            .Execute(choiceContext);
        await CommonActionsExtensions.Apply<VulnerablePower>(choiceContext, this, play);
        var card = CommonActions.SelectSingleCard(this, SelectionScreenPrompt, choiceContext, PileType.Hand).Result;
        if (card == null)
        {
            return;
        }
        CardCmd.ApplyKeyword(card, NinjaKeyword.Blade);
        await CardCmd.AutoPlay(choiceContext, card, null);
        await CardCmd.Exhaust(choiceContext, card);
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
