using Godot;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards;

public class DarkSoulCut() : LexNinja2Card(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(0, ValueProp.Move), new NinjutsuVar(3)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(StaticHoverTip.Fatal)];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [NinjaKeyword.Blade, CardKeyword.Exhaust];
    public override bool CanBeGeneratedInCombat => false;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Ninjutsu())
        {
            NCombatRoom instance = NCombatRoom.Instance;
            if (instance != null)
                instance.CombatVfxContainer.AddChildSafely(
                    (Node)NGroundFireVfx.Create(Owner.Creature, VfxColor.Purple)
                );
            SfxCmd.Play("event:/sfx/characters/attack_fire");
            NinjaAudio.Play("res://LexNinja2/audio/DarkSoulCut.mp3");
            bool shouldTriggerFatal = play.Target.Powers.All<PowerModel>(
                (Func<PowerModel, bool>)(p => p.ShouldOwnerDeathTriggerFatal())
            );
            Vector2? monsterPos = new Vector2?();
            int hitPoint = play.Target.CurrentHp;

            await MegaCrit.Sts2.Core.Commands.Cmd.Wait(1f);
            if (instance != null)
                instance.CombatVfxContainer.AddChildSafely(
                    (Node)NGroundFireVfx.Create(play.Target, VfxColor.Purple)
                );
            SfxCmd.Play("event:/sfx/characters/attack_fire");
            AttackCommand attackCommand = await DamageCmd
                .Attack(DynamicVars.Damage.BaseValue)
                .WithHitCount(3)
                .FromCard((CardModel)this)
                .Targeting(play.Target)
                .Execute(choiceContext);
            if (
                shouldTriggerFatal
                && attackCommand
                    .Results.SelectMany((List<DamageResult> r) => r)
                    .Any((DamageResult r) => r.WasTargetKilled)
            )
            {
                await CreatureCmd.GainMaxHp(Owner.Creature, hitPoint);
            }
        }
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }

    public override string CustomPortraitPath => $"DarkSoulCut_p.png".BigCardImagePath();
    public override string PortraitPath => $"DarkSoulCut.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/DarkSoulCut.png".CardImagePath();

    private Boolean Ninjutsu()
    {
        if (Owner.Creature.GetPower<FreeNinjutsuPower>() != null)
        {
            return true;
        }
        if (Owner.Creature.GetPower<Lexkela>() != null)
        {
            if (Owner.Creature.GetPower<Lexkela>().Amount >= DynamicVars["Renshu"].BaseValue)
            {
                PowerCmd.Apply<Lexkela>(
                    new ThrowingPlayerChoiceContext(),
                    Owner.Creature,
                    -DynamicVars["Renshu"].BaseValue,
                    Owner.Creature,
                    this
                );
                return true;
            }
        }
        return false;
    }

    private Boolean CanCastNinjutsu()
    {
        if (Owner.Creature.GetPower<FreeNinjutsuPower>() != null)
        {
            return true;
        }

        if (Owner.Creature.GetPower<Lexkela>() != null)
        {
            if (Owner.Creature.GetPower<Lexkela>().Amount >= DynamicVars["Renshu"].BaseValue)
            {
                return true;
            }
        }

        return false;
    }

    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
