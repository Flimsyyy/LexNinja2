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
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards;

public class HolyLittleStorm()
    : LexNinja2Card(1, CardType.Attack, CardRarity.Common, TargetType.RandomEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7, ValueProp.Move)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<Lexkela>()];

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [NinjaKeyword.Hand, NinjaKeyword.Blade];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu, NinjaTags.Holy];
    protected override bool ShouldGlowGoldInternal => IsGlowed();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/HolyLittleStorm.mp3");
        await MegaCrit.Sts2.Core.Commands.Cmd.Wait(1f);
        int amount = 0;
        if (Owner.GetRelic<ChemicalX>() != null)
        {
            amount += 2;
            Owner.GetRelic<ChemicalX>().Flash();
        }
        if (Owner.Creature.GetPower<Lexkela>() != null)
        {
            amount += Owner.Creature.GetPower<Lexkela>().Amount;
            await DamageCmd
                .Attack(DynamicVars.Damage.BaseValue)
                .WithHitCount(amount + 1)
                .FromCard(this)
                .TargetingRandomOpponents(this.CombatState)
                .WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3")
                .Execute(choiceContext);
            if (BaseReplayCount > 1)
            {
                return;
            }
            if (Keywords.Contains(NinjaKeyword.FreeNinjutsu))
            {
                RemoveKeyword(NinjaKeyword.FreeNinjutsu);
                return;
            }
            if (Owner.Creature.GetPower<FreeNinjutsuPower>() != null)
            {
                return;
            }
            await PowerCmd.Apply<Lexkela>(
                new ThrowingPlayerChoiceContext(),
                Owner.Creature,
                -Owner.Creature.GetPower<Lexkela>().Amount,
                Owner.Creature,
                this
            );
        }
        else
        {
            await DamageCmd
                .Attack(DynamicVars.Damage.BaseValue)
                .WithHitCount(1)
                .FromCard(this)
                .TargetingRandomOpponents(this.CombatState)
                .WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3")
                .Execute(choiceContext);
        }
    }

    private Boolean Ninjutsu()
    {
        if (Owner.Creature.GetPower<FreeNinjutsuPower>() != null)
        {
            return true;
        }
        if (Owner.Creature.GetPower<Lexkela>() != null)
        {
            return true;
        }
        return false;
    }

    private Boolean IsGlowed()
    {
        if (this.Keywords.Contains(NinjaKeyword.FreeNinjutsu))
        {
            return true;
        }
        if (Owner.Creature.GetPower<FreeNinjutsuPower>() != null)
        {
            return true;
        }
        if (Owner.Creature.GetPower<Lexkela>() != null)
        {
            return true;
        }
        return false;
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public override string CustomPortraitPath => $"HolyLittleStorm_p.png".BigCardImagePath();
    public override string PortraitPath => $"HolyLittleStorm.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/HolyLittleStorm.png".CardImagePath();

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
            NinjaAudio.Play("res://LexNinja2/audio/YEEART.mp3", 0.5f);
        }
        else
        {
            return;
        }
    }
}
