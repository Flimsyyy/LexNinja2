using BaseLib.Extensions;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LexNinja2.LexNinja2Code.Cards.Rares;

// [Pool(typeof(TokenCardPool))]
public class HengheShui() : LexNinja2Card(1, CardType.Skill, CardRarity.Rare, TargetType.AnyAlly)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new HealVar(15), new PowerVar<PoisonPower>(3)];

    // protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Infection>()];
    public override CardMultiplayerConstraint MultiplayerConstraint =>
        CardMultiplayerConstraint.MultiplayerOnly;
    public override bool CanBeGeneratedInCombat => false;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/HengheShui.mp3");
        await Cmd.Wait(1.5f);
        NinjaAudio.Play("res://LexNinja2/audio/ASan.mp3");
        var healPoint = Owner.RunState.Rng.CombatEnergyCosts.NextInt(DynamicVars.Heal.IntValue);
        var poisonPoint = Owner.RunState.Rng.CombatEnergyCosts.NextInt(
            DynamicVars.Power<PoisonPower>().IntValue
        );
        if (play.Target == null)
        {
            return;
        }
        await CreatureCmd.Heal(play.Target, healPoint);
        await PowerCmd.Apply<PoisonPower>(
            choiceContext,
            play.Target,
            poisonPoint,
            Owner.Creature,
            this
        );
        // await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Infection>(play.Target.Player), PileType.Discard, Owner);
        // new Rng(this.Seed, StringHelper.SnakeCase(RunRngType.CombatTargets.ToString()));
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Heal.UpgradeValueBy(5);
        DynamicVars.Poison.UpgradeValueBy(2);
    }

    public override string CustomPortraitPath => $"HengheShui_p.png".BigCardImagePath();
    public override string PortraitPath => $"HengheShui.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/HengheShui.png".CardImagePath();
}
