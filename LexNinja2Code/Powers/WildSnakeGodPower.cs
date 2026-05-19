using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.TestSupport;

namespace LexNinja2.LexNinja2Code.Powers;

public class WildSnakeGodPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "WildSnakeGodPower.png".PowerImagePath();
    public override string? CustomBigIconPath => "WildSnakeGodPower.png".BigPowerImagePath();

    public override async Task AfterSideTurnStart(CombatSide side, ICombatState combatState)
    {
        if (side != Owner.Side)
            return;
        NinjaAudio.Play("res://LexNinja2/audio/WildSnakeGod.mp3");
        Flash();
        foreach (
            var card in PileType.Hand.GetPile(Owner.Player!).Cards.Where(c => !c.EnergyCost.CostsX)
        )
        {
            if (card.EnergyCost.GetWithModifiers(CostModifiers.None) < 0)
                continue;
            card.EnergyCost.SetThisCombat(NextEnergyCost());
            NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
            // IReadOnlyList<CardModel> cards = PileType.Hand.GetPile(Owner.Player).Cards;
            // if (cards.Count == 0)
            //     return;
            // int amount = (int) ((Decimal) cards.Count * Amount);
            // await CreatureCmd.GainBlock(Owner, amount, ValueProp.Unpowered, null);
        }
    }

    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        return player != Owner.Player ? count : count + Amount;
    }

    private int _testEnergyCostOverride = -1;

    public int TestEnergyCostOverride
    {
        get => _testEnergyCostOverride;
        set
        {
            TestMode.AssertOn();
            AssertMutable();
            _testEnergyCostOverride = value;
        }
    }

    private int NextEnergyCost()
    {
        return TestEnergyCostOverride >= 0
            ? TestEnergyCostOverride
            : Owner.Player!.RunState.Rng.CombatEnergyCosts.NextInt(4);
    }
}
