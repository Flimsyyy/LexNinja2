using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards.Uncommons;

public class BuddhaHand()
    : LexNinja2NinjutsuCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<VulnerablePower>(2),
            new DamageVar(10, ValueProp.Move),
            new EnergyVar(2),
            new NinjutsuVar(1),
        ];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [NinjaKeyword.Hand, CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/BuddhaHand.mp3");
        await CommonActions
            .CardAttack(this, play, vfx: "vfx/vfx_attack_blunt")
            .Execute(choiceContext);
        await CommonActions.Apply<VulnerablePower>(choiceContext, this, play);
        if (!Ninjutsu(play))
        {
            return;
        }
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars.Vulnerable.UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => "BuddhaHand_p.png".BigCardImagePath();
    public override string PortraitPath => "BuddhaHand.png".CardImagePath();
    public override string BetaPortraitPath => "beta/BuddhaHand.png".CardImagePath();
}
