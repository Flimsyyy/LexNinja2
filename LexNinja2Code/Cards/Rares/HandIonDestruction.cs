using BaseLib.Utils;
using Godot;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards.Rares;

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
        var xValue = ResolveEnergyXValue();
        if (xValue <= 0)
        {
            return;
        }
        NinjaAudio.Play("res://LexNinja2/audio/HandIonDestruction.mp3");
        if (await Ninjutsu(choiceContext, play))
        {
            await Cmd.Wait(1.2f);
            await PowerCmd.Apply<DoubleDamagePower>(
                choiceContext,
                Owner.Creature,
                1,
                Owner.Creature,
                this
            );
        }

        var sideCenterFloor = VfxCmd.GetSideCenterFloor(CombatSide.Enemy, CombatState!);
        if (sideCenterFloor.HasValue)
        {
            var child = NLargeMagicMissileVfx.Create(sideCenterFloor.Value, new Color("917cf6"));
            if (child != null)
            {
                var instance = NCombatRoom.Instance;
                instance?.CombatVfxContainer.AddChildSafely((Node)child);
                await Cmd.Wait(child.WaitTime);
            }
        }

        foreach (var hittableEnemy in CombatState!.HittableEnemies)
        {
            var instance = NCombatRoom.Instance;
            instance?.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(hittableEnemy)!);
        }

        await CommonActions.CardAttack(this, play, hitCount: xValue).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }

    public override string CustomPortraitPath => $"HandIonDestruction.png".BigCardImagePath();
    public override string PortraitPath => $"HandIonDestruction.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/HandIonDestruction.png".CardImagePath();

    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
