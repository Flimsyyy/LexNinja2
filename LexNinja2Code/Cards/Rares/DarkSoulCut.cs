using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
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
        if (!Ninjutsu(choiceContext))
        {
            return;
        }
        var instance = NCombatRoom.Instance;
        instance?.CombatVfxContainer.AddChildSafely(
            NGroundFireVfx.Create(Owner.Creature, VfxColor.Purple)!
        );
        SfxCmd.Play("event:/sfx/characters/attack_fire");
        NinjaAudio.Play("res://LexNinja2/audio/DarkSoulCut.mp3");
        var shouldTriggerFatal = play.Target!.Powers.All(p => p.ShouldOwnerDeathTriggerFatal());
        var hitPoint = play.Target.CurrentHp;

        await Cmd.Wait(1f);
        instance?.CombatVfxContainer.AddChildSafely(
            NGroundFireVfx.Create(play.Target, VfxColor.Purple)!
        );
        SfxCmd.Play("event:/sfx/characters/attack_fire");
        var attackCommand = CommonActions.CardAttack(this, play, hitCount: 3);
        await attackCommand.Execute(choiceContext);
        if (
            shouldTriggerFatal
            && attackCommand.Results.SelectMany(r => r).Any(r => r.WasTargetKilled)
        )
        {
            await CreatureCmd.GainMaxHp(Owner.Creature, hitPoint);
        }
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }

    public override string CustomPortraitPath => $"DarkSoulCut_p.png".BigCardImagePath();
    public override string PortraitPath => $"DarkSoulCut.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/DarkSoulCut.png".CardImagePath();
    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
