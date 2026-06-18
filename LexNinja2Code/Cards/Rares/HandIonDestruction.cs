using BaseLib.Utils;
using Godot;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards.Rares;

public class HandIonDestruction()
    : LexNinja2Card(3, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(38, ValueProp.Move)];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Hand];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<Lexkela>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/HandIonDestruction.mp3");
        await Cmd.Wait(1.2f);

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

        await CommonActions.CardAttack(this, play).Execute(choiceContext);

        if (!Owner.Creature.HasPower<Lexkela>())
        {
            return;
        }
        await PowerCmd.Apply<Lexkela>(
            choiceContext,
            Owner.Creature,
            -Owner.Creature.GetPowerAmount<Lexkela>(),
            Owner.Creature,
            null
        );
        await PowerCmd.Apply<NoLexkelaPower>(
            choiceContext,
            Owner.Creature,
            1,
            Owner.Creature,
            this
        );
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(8);
    }

    public override string CustomPortraitPath => $"HandIonDestruction.png".BigCardImagePath();
    public override string PortraitPath => $"HandIonDestruction.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/HandIonDestruction.png".CardImagePath();
}
