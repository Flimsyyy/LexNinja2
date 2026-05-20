using BaseLib.Utils;
using Godot;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards.Uncommons;

public class HamoodKick()
    : LexNinja2Card(0, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(13, ValueProp.Move), new NinjutsuVar(1)];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/HamoodKick.mp3");
        await CommonActions
            .CardAttack(this, play, tmpSfx: "blunt_attack.mp3")
            .WithHitVfxNode((Func<Creature, Node2D>)(t => NBigSlashImpactVfx.Create(t)!))
            .Execute(choiceContext);
        if (!await Ninjutsu(choiceContext, play))
        {
            return;
        }
        EnergyCost.AddThisCombat(-1);
    }

    public override Task AfterCardDrawn(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw
    )
    {
        if (card != this)
            return Task.CompletedTask;
        EnergyCost.AddThisCombat(1);
        return Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5);
    }

    public override string CustomPortraitPath => "HamoodKick2.png".BigCardImagePath();
    public override string PortraitPath => "HamoodKick2.png".CardImagePath();
    public override string BetaPortraitPath => "beta/HamoodKick2.png".CardImagePath();

    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
