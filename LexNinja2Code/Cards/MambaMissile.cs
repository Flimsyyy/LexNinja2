using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
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

public class MambaMissile()
    : LexNinja2Card(3, CardType.Attack, CardRarity.Common, TargetType.RandomEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(3, ValueProp.Move), new RepeatVar(5)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Science];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<Lexkela>()];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Mamba];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/MambaMissile.mp3", 1.5f);
        await Cmd.Wait(0.5f);
        await DamageCmd
            .Attack(DynamicVars.Damage.BaseValue)
            .WithHitCount(DynamicVars.Repeat.IntValue)
            .FromCard(this)
            .TargetingRandomOpponents(CombatState)
            .WithHitFx("vfx/vfx_rock_shatter", tmpSfx: "blunt_attack.mp3")
            .WithHitVfxSpawnedAtBase()
            .Execute(choiceContext);
        foreach (var enemy in CombatState.HittableEnemies)
        {
            if (enemy.GetPower<SoarPower>() != null)
            {
                await PowerCmd.Remove<SoarPower>(enemy);
                NinjaAudio.Play("res://LexNinja2/audio/MambaOut.mp3");
            }
            if (enemy.GetPower<FlutterPower>() != null)
            {
                NinjaAudio.Play("res://LexNinja2/audio/MambaOut.mp3");
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => "MambaMissile_p.png".BigCardImagePath();
    public override string PortraitPath => "MambaMissile.png".CardImagePath();
    public override string BetaPortraitPath => "beta/MambaMissile.png".CardImagePath();

    public override async Task AfterDamageGiven(
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
            return;
        }
        if (cardSource == this)
        {
            NinjaAudio.Play("res://LexNinja2/audio/Man!.mp3", 0.5f);
        }
        else
        {
            return;
        }
    }
}
