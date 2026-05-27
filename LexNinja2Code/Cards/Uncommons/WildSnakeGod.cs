using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace LexNinja2.LexNinja2Code.Cards.Uncommons;

public class WildSnakeGod() : LexNinja2Card(0, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new NinjutsuVar(1), new PowerVar<WildSnakeGodPower>(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/WildSnakeGod.mp3");
        await CommonActions.ApplySelf<WildSnakeGodPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }

    public override string CustomPortraitPath => $"WildSnakeGod.png".BigCardImagePath();
    public override string PortraitPath => $"WildSnakeGod.png".CardImagePath();

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> creatures,
        ICombatState combatState
    )
    {
        if (side != Owner.Creature.Side)
            return;
        EnergyCost.SetThisCombat(Owner!.RunState.Rng.CombatEnergyCosts.NextInt(4));
        NCard.FindOnTable(this)?.PlayRandomizeCostAnim();
        return;
    }
}
