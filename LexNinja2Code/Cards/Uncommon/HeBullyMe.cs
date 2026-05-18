using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LexNinja2.LexNinja2Code.Cards;

public class HeBullyMe()
    : LexNinja2Card(3, CardType.Skill, CardRarity.Uncommon, TargetType.AllAllies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new EnergyVar(3), new PowerVar<StrengthPower>(5)];
    public override CardMultiplayerConstraint MultiplayerConstraint =>
        CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        IEnumerable<Creature> players =
            from c in CombatState?.GetTeammatesOf(Owner.Creature)
            where c is { IsAlive: true, IsPlayer: true }
            select c;
        foreach (var player in players)
        {
            if (player == Owner.Creature) continue;
            await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, player.Player!);
            await PowerCmd.Apply<FlexPotionPower>(
                choiceContext,
                player.Player!.Creature,
                DynamicVars.Strength.BaseValue,
                Owner.Creature,
                this
            );
        }
        NinjaAudio.Play("res://LexNinja2/audio/HeBullyMe.mp3");
        PlayerCmd.EndTurn(Owner, false);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public override string CustomPortraitPath => $"HeBullyMe.png".BigCardImagePath();
    public override string PortraitPath => $"HeBullyMe.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/HeBullyMe.png".CardImagePath();
}
