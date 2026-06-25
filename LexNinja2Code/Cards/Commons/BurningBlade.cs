using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
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

namespace LexNinja2.LexNinja2Code.Cards.Commons;

public class BurningBlade()
    : LexNinja2NinjutsuCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(6, ValueProp.Move), new NinjutsuVar(1)];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [HoverTipFactory.Static(StaticHoverTip.ReplayStatic)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Blade];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        // bool shouldTriggerFatal = play.Target.Powers.All<PowerModel>((Func<PowerModel, bool>) (p => p.ShouldOwnerDeathTriggerFatal()));
        var instance = NCombatRoom.Instance;
        instance?.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(play.Target!)!);

        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        // if (!shouldTriggerFatal || !attackCommand.Results.Any<DamageResult>((Func<DamageResult, bool>) (r => r.WasTargetKilled)))

        if (!Ninjutsu(play))
        {
            return;
        }
        foreach (var card in Owner.PlayerCombatState!.AllCards.OfType<BurningBlade>())
        {
            ++card.BaseReplayCount;
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }

    public override string CustomPortraitPath => "OverBurningBlade_p.png".BigCardImagePath();
    public override string PortraitPath => "OverBurningBlade.png".CardImagePath();
    public override string BetaPortraitPath => "beta/OverBurningBlade.png".CardImagePath();

    public override Task AfterDamageGiven(
        PlayerChoiceContext choiceContext,
        Creature? dealer,
        DamageResult result,
        ValueProp props,
        Creature target,
        CardModel? cardSource
    )
    {
        if (dealer != Owner.Creature || cardSource == null || cardSource != this)
        {
            return Task.CompletedTask;
        }
        NinjaAudio.Play("res://LexNinja2/audio/OverBurningBlade.mp3");
        return Task.CompletedTask;
    }
}
