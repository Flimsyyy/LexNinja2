using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Cards.Rares;

public class IgnisHealing() : LexNinja2Card(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    private const string Percent = "bili";
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar(Percent, 30), new PowerVar<IgnisHealingPower>(5)];
    public override bool CanBeGeneratedInCombat => false;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/IgnisHealing.mp3");
        var heal = DynamicVars[Percent].BaseValue / 100 * Owner.Creature.MaxHp;
        await CreatureCmd.Heal(Owner.Creature, heal);
        await CommonActions.ApplySelf<IgnisHealingPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars[Percent].UpgradeValueBy(10);
    }

    public override string CustomPortraitPath => $"IgnisHealing_p.png".BigCardImagePath();
    public override string PortraitPath => $"IgnisHealing.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/IgnisHealing.png".CardImagePath();
}
