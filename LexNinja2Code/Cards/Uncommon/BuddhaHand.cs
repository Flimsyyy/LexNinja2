using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards;

public class BuddhaHand()
    : LexNinja2Card(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    // private decimal _extraDamage;
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<VulnerablePower>(2),
            new DamageVar(10, ValueProp.Move),
            new EnergyVar(2),
            new NinjutsuVar(1),
        ];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [NinjaKeyword.Hand, CardKeyword.Exhaust];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];

    // private Decimal ExtraDamage
    // {
    //     get => this._extraDamage;
    //     set
    //     {
    //         this.AssertMutable();
    //         this._extraDamage = value;
    //     }
    // }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/BuddhaHand.mp3");
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_blunt").Execute(choiceContext);
        await CommonActionsExtensions.Apply<VulnerablePower>(choiceContext, this, play);
        if (Ninjutsu(choiceContext))
        {
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        }
    }

    // public override Task AfterCardDrawn(
    //     PlayerChoiceContext choiceContext,
    //     CardModel card,
    //     bool fromHandDraw)
    // {
    //     if (card != this)
    //         return Task.CompletedTask;
    //     Decimal baseValue = this.DynamicVars["Increase"].BaseValue;
    //     DamageVar damage = this.DynamicVars.Damage;
    //     damage.BaseValue = damage.BaseValue + baseValue;
    //     this.ExtraDamage += baseValue;
    //     return Task.CompletedTask;
    // }
    //
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars.Vulnerable.UpgradeValueBy(1);
    }

    // protected override void AfterDowngraded()
    // {
    //     base.AfterDowngraded();
    //     DamageVar damage = this.DynamicVars.Damage;
    //     damage.BaseValue = damage.BaseValue + this.ExtraDamage;
    // }
    //
    // private void BuffFromCardPlay(Decimal extraDamage)
    // {
    //     DamageVar damage = this.DynamicVars.Damage;
    //     damage.BaseValue = damage.BaseValue - extraDamage;
    //     this.ExtraDamage -= extraDamage;
    // }

    public override string CustomPortraitPath => "BuddhaHand_p.png".BigCardImagePath();
    public override string PortraitPath => "BuddhaHand.png".CardImagePath();
    public override string BetaPortraitPath => "beta/BuddhaHand.png".CardImagePath();
    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
