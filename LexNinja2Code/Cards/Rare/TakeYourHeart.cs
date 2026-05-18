using BaseLib.Utils;
using Godot;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards;

public class TakeYourHeart()
    : LexNinja2Card(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    private const string DebuffAmount = "DebuffAmount";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new NinjutsuVar(10), new DamageVar(0, ValueProp.Move), new(DebuffAmount, 2)];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<ArtifactPower>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [CardKeyword.Exhaust, NinjaKeyword.Hand];
    public override CardMultiplayerConstraint MultiplayerConstraint =>
        CardMultiplayerConstraint.SingleplayerOnly;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (!IsEnoughDebuff(play.Target!) || !Ninjutsu(choiceContext))
        {
            await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
            return;
        }
        NinjaAudio.Play("res://LexNinja2/audio/TakeYourHeart.mp3", 0.25f);
        var instance = NCombatRoom.Instance;
        await NinjaAnim.TriggerCastAnim(this);
        for (var i = 0; i < 3; i++)
        {
            instance?.CombatVfxContainer.AddChildSafely(
                NGroundFireVfx.Create(Owner.Creature, VfxColor.Purple)!
            );
        }

        await Cmd.Wait(1F);
        if (LocalContext.IsMe(Owner))
            VfxCmd.PlayFullScreenInCombat("vfx/vfx_adrenaline", Owner.Creature);
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_molten_fist", tmpSfx: "blunt_attack.mp3")
            .Execute(choiceContext);
        await CreatureCmd.Kill(play.Target!, true);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public override string CustomPortraitPath => $"TakeYourHeart_p.png".BigCardImagePath();
    public override string PortraitPath => $"TakeYourHeart.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/TakeYourHeart.png".CardImagePath();

    private bool IsEnoughDebuff(Creature target)
    {
        var debuffAmount = target.Powers.Where(p => p.TypeForCurrentAmount == PowerType.Debuff).Sum(p => p.Amount);
        return debuffAmount >= DynamicVars[DebuffAmount].IntValue;
    }

    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
