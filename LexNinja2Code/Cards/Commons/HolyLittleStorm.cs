using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards.Commons;

public class HolyLittleStorm()
    : LexNinja2NinjutsuCard(1, CardType.Attack, CardRarity.Common, TargetType.RandomEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(NinjaHelper.GetValueByChallengeMode(6, 7), ValueProp.Move),
            new NinjutsuVar(0) { HasLexKelaCostX = true },
        ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [LexKela.HoverTip()];

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [NinjaKeyword.Hand, NinjaKeyword.Blade];

    protected override HashSet<CardTag> AdditionalCanonicalTags => [NinjaTags.Holy];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/HolyLittleStorm.mp3");
        await Cmd.Wait(1f);
        var hitCount = ResolveLexkelaXValue(play) + 1;
        await CommonActions
            .CardAttack(
                this,
                play,
                hitCount: hitCount,
                vfx: "vfx/vfx_attack_blunt",
                tmpSfx: "blunt_attack.mp3"
            )
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public override string CustomPortraitPath => $"HolyLittleStorm_p.png".BigCardImagePath();
    public override string PortraitPath => $"HolyLittleStorm.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/HolyLittleStorm.png".CardImagePath();

    public override Task AfterDamageGiven(
        PlayerChoiceContext choiceContext,
        Creature? dealer,
        DamageResult result,
        ValueProp props,
        Creature target,
        CardModel? cardSource
    )
    {
        if (dealer != Owner.Creature || cardSource == null)
        {
            return Task.CompletedTask;
        }
        if (cardSource == this)
        {
            NinjaAudio.Play("res://LexNinja2/audio/YEEART.mp3", 0.5f);
        }
        return Task.CompletedTask;
    }
}
