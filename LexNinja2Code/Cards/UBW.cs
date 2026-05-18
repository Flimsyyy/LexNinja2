using Godot;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards;

public class UBW() : LexNinja2Card(2, CardType.Attack, CardRarity.Rare, TargetType.RandomEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new NinjutsuVar(1),
            new DamageVar(8, ValueProp.Move),
            new CalculationBaseVar(0),
            new CalculationExtraVar(1),
            new CalculatedVar("HitCounts").WithMultiplier(
                (Func<CardModel, Creature, Decimal>)(
                    (card, _) =>
                        (Decimal)
                            CombatManager.Instance.History.CardPlaysFinished.Count<CardPlayFinishedEntry>(
                                (Func<CardPlayFinishedEntry, bool>)(
                                    e =>
                                        e.CardPlay.Card.Owner == card.Owner
                                        && (
                                            e.CardPlay.Card.Keywords.Contains(NinjaKeyword.Blade)
                                            || e.CardPlay.Card.Tags.Contains(CardTag.Shiv)
                                        )
                                )
                            )
                )
            ),
        ];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Blade];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Ninjutsu())
        {
            NinjaAudio.Play("res://LexNinja2/audio/UBW.mp3");
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
            NCombatRoom instance = NCombatRoom.Instance;
            if (instance != null)
                instance.CombatVfxContainer.AddChildSafely(
                    (Node)NHellraiserVfx.Create(Owner.Creature)
                );
            await MegaCrit.Sts2.Core.Commands.Cmd.Wait(1);
            await DamageCmd
                .Attack(DynamicVars.Damage.BaseValue)
                .WithHitCount(
                    (int)((CalculatedVar)this.DynamicVars["HitCounts"]).Calculate(play.Target)
                )
                .FromCard(this)
                .TargetingRandomOpponents(CombatState)
                .WithHitFx("vfx/hellraiser_attack_vfx", tmpSfx: "heavy_attack.mp3")
                .SpawningHitVfxOnEachCreature()
                .WithHitVfxSpawnedAtBase()
                .Execute(choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
    }

    public override string CustomPortraitPath => $"UBW_p.png".BigCardImagePath();
    public override string PortraitPath => $"UBW.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/UBW.png".CardImagePath();

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
