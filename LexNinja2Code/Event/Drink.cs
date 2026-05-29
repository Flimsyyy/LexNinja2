using BaseLib.Abstracts;
using Godot;
using LexNinja2.LexNinja2Code.Api;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace LexNinja2.LexNinja2Code.Event;

public class Drink : CustomEventModel
{
    public override string? CustomInitialPortraitPath => "res://LexNinja2/images/events/Drink.png";
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new StringVar("RandomCard"), new HealVar(5), new("Possibility", 15)];

    public override bool IsAllowed(IRunState runState)
    {
        if (
            runState.Players.All(p => PileType.Deck.GetPile(p).Cards.Count(c => c.IsRemovable) > 10)
        )
        {
            return true;
        }
        return false;
    }

    private decimal _stop;
    private CardModel? _randomCardToLose;
    private CardModel? RandomCardToLose
    {
        get { return _randomCardToLose; }
        set
        {
            AssertMutable();
            _randomCardToLose = value;
        }
    }

    private void GetNewRandomCard()
    {
        List<CardModel> list;
        if (RandomCardToLose == null)
        {
            list = Owner.Deck.Cards.Where((CardModel c) => c.Rarity != CardRarity.Basic).ToList();
        }
        else
        {
            list = Owner
                .Deck.Cards.Where((CardModel c) => c.GetType() != RandomCardToLose.GetType())
                .ToList();
        }
        list.RemoveAll((CardModel c) => !c.IsRemovable);
        if (list.Count == 0)
        {
            list = base.Owner.Deck.Cards.Where((CardModel c) => c.IsRemovable).ToList();
        }
        RandomCardToLose = base.Rng.NextItem(list);
        StringVar stringVar = (StringVar)base.DynamicVars["RandomCard"];
        if (RandomCardToLose != null)
            stringVar.StringValue = RandomCardToLose.Title;
    }

    protected override Task BeforeEventStarted(bool isPreFinished)
    {
        NinjaAudio.Play("res://LexNinja2/audio/Thirsty.mp3");
        GetNewRandomCard();
        return Task.CompletedTask;
    }

    protected override IReadOnlyList<EventOption> GenerateInitialOptions() => [Option(Drink0)];

    private async Task Drink0()
    {
        NinjaAudio.Play("res://LexNinja2/audio/Drink.mp3");
        var player = Owner;
        if (player != null)
            await CreatureCmd.Heal(player.Creature, DynamicVars.Heal.BaseValue);
        DrinkOrLeavePage();
    }

    private void DrinkOrLeavePage()
    {
        SetEventState(
            PageDescription("DRINK_OR_LEAVE"),
            [
                Option(MoreDrink, "DRINK_OR_LEAVE", HoverTipFactory.FromCard(RandomCardToLose)), // 第二个参数代表该选项所在页面
                Option(Leave, "DRINK_OR_LEAVE"),
            ]
        );
    }

    private async Task MoreDrink()
    {
        if (DynamicVars["Possibility"].BaseValue == 15)
        {
            NinjaAudio.Play("res://LexNinja2/audio/MoreDrink.mp3");
        }
        else
        {
            NinjaAudio.Play("res://LexNinja2/audio/MORE!!!.mp3");
        }
        var player = Owner;
        if (player != null)
            await CreatureCmd.Heal(player.Creature, DynamicVars.Heal.BaseValue);
        if (RandomCardToLose != null)
            await CardPileCmd.RemoveFromDeck(RandomCardToLose);
        GetNewRandomCard();
        _stop = Owner.RunState.Rng.CombatEnergyCosts.NextInt(101);
        GD.Print(_stop);
        if (
            _stop >= DynamicVars["Possibility"].BaseValue
            || DynamicVars["Possibility"].BaseValue == 15
        )
        {
            DynamicVars["Possibility"].BaseValue += 10;
            MoreDrinkPage();
        }
        else
        {
            SetEventFinished(PageDescription("LEAVE2"));
        }
    }

    private void MoreDrinkPage()
    {
        SetEventState(
            PageDescription("MORE_DRINK"),
            [Option(MoreDrink, "MORE_DRINK", HoverTipFactory.FromCard(RandomCardToLose))]
        );
    }

    private async Task Leave()
    {
        SetEventFinished(PageDescription("LEAVE1"));
    }

    private async Task Leave2()
    {
        SetEventFinished(PageDescription("LEAVE2"));
    }
}
