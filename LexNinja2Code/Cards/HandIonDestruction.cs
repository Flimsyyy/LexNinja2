using Godot;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards;

public class HandIonDestruction()
    : LexNinja2Card(0, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(6, ValueProp.Move), new NinjutsuVar(3)];
    protected override bool HasEnergyCostX => true;
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Hand];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        HandIonDestruction card = this;
        int num1 = card.ResolveEnergyXValue();
        if (num1 > 0)
        {
            NinjaAudio.Play("res://LexNinja2/audio/HandIonDestruction.mp3");
            if (Ninjutsu())
            {
                await MegaCrit.Sts2.Core.Commands.Cmd.Wait(1.2f);
                await PowerCmd.Apply<DoubleDamagePower>(
                    new ThrowingPlayerChoiceContext(),
                    Owner.Creature,
                    1,
                    Owner.Creature,
                    this
                );
            }
            Vector2? sideCenterFloor = VfxCmd.GetSideCenterFloor(
                CombatSide.Enemy,
                card.CombatState
            );
            if (sideCenterFloor.HasValue)
            {
                NLargeMagicMissileVfx child = NLargeMagicMissileVfx.Create(
                    sideCenterFloor.Value,
                    new Color("917cf6")
                );
                if (child != null)
                {
                    NCombatRoom instance = NCombatRoom.Instance;
                    if (instance != null)
                        instance.CombatVfxContainer.AddChildSafely((Node)child);
                    await MegaCrit.Sts2.Core.Commands.Cmd.Wait(child.WaitTime);
                }
            }
            foreach (
                Creature hittableEnemy in (IEnumerable<Creature>)card.CombatState.HittableEnemies
            )
            {
                NCombatRoom instance = NCombatRoom.Instance;
                if (instance != null)
                    instance.CombatVfxContainer.AddChildSafely(
                        (Node)NGroundFireVfx.Create(hittableEnemy)
                    );
            }
        }
        await DamageCmd
            .Attack(card.DynamicVars.Damage.BaseValue)
            .WithHitCount(num1)
            .FromCard((CardModel)card)
            .TargetingAllOpponents(card.CombatState)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }

    public override string CustomPortraitPath => $"HandIonDestruction.png".BigCardImagePath();
    public override string PortraitPath => $"HandIonDestruction.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/HandIonDestruction.png".CardImagePath();

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
