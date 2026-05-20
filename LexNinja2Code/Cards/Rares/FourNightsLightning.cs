using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards.Rares;

public class FourNightsLightning()
    : LexNinja2Card(4, CardType.Attack, CardRarity.Rare, TargetType.RandomEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(4, ValueProp.Move), new NinjutsuVar(4), new RepeatVar(3)];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/FourNightsLightning.mp3", 1.5f);
        await NinjaAnim.TriggerCastAnim(this);
        await Cmd.Wait(0.5f);
        decimal repeatCount = 1;
        if (await Ninjutsu(choiceContext, play))
        {
            repeatCount += DynamicVars.Repeat.BaseValue;
        }
        var hitCount = (int)(4 * repeatCount);
        await CommonActions
            .CardAttack(
                this,
                play,
                hitCount: hitCount,
                vfx: "vfx/vfx_attack_lightning",
                tmpSfx: "lightning_orb_evoke.mp3"
            )
            .Execute(choiceContext);
        await CardPileCmd.Draw(choiceContext, repeatCount, Owner);
        await PlayerCmd.GainEnergy(repeatCount, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => $"FourNightsLightning_p.png".BigCardImagePath();
    public override string PortraitPath => $"FourNightsLightning.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/FourNightsLightning.png".CardImagePath();

    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
