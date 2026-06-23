using LexNinja2.LexNinja2Code.Api;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace LexNinja2.LexNinja2Code.Event;

[RegisterSharedEvent]
public class Drink : ModEventTemplate
{
    public override string? CustomInitialPortraitPath => "res://LexNinja2/images/events/Drink.png";
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new StringVar("RandomCard"), new HealVar(5), new("Possibility", 15)];

    public override bool IsAllowed(IRunState runState)
    {
        return runState.Players.All(p =>
            PileType.Deck.GetPile(p).Cards.Count(c => c.IsRemovable) > 10
        );
    }

    private decimal _stop;
    private CardModel? _randomCardToLose;
    private CardModel? RandomCardToLose
    {
        get => _randomCardToLose;
        set
        {
            AssertMutable();
            _randomCardToLose = value;
        }
    }

    private void GetNewRandomCard()
    {
        var cards = Owner!.Deck.Cards;
        var list =
            RandomCardToLose == null
                ? cards.Where(c => c.Rarity != CardRarity.Basic).ToList()
                : cards.Where(c => c.GetType() != RandomCardToLose.GetType()).ToList();
        list.RemoveAll(c => !c.IsRemovable);
        if (list.Count == 0)
        {
            list = cards.Where(c => c.IsRemovable).ToList();
        }
        RandomCardToLose = Rng.NextItem(list);
        var stringVar = (StringVar)DynamicVars["RandomCard"];
        if (RandomCardToLose != null)
            stringVar.StringValue = RandomCardToLose.Title;
    }

    protected override Task BeforeEventStarted(bool isPreFinished)
    {
        NinjaAudio.Play("res://LexNinja2/audio/Thirsty.mp3");
        GetNewRandomCard();
        return Task.CompletedTask;
    }

    protected override IReadOnlyList<EventOption> GenerateInitialOptions() =>
        [new(this, Drink0, InitialOptionKey("DRINK0"))];

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
                new EventOption(
                    this,
                    MoreDrink,
                    ModOptionKey("DRINK_OR_LEAVE", "MORE_DRINK"),
                    HoverTipFactory.FromCard(RandomCardToLose!)
                ), // 第二个参数代表该选项所在页面
                new EventOption(this, Leave, ModOptionKey("DRINK_OR_LEAVE", "LEAVE")),
            ]
        );
    }

    private async Task MoreDrink()
    {
        NinjaAudio.Play(
            DynamicVars["Possibility"].BaseValue == 15
                ? "res://LexNinja2/audio/MoreDrink.mp3"
                : "res://LexNinja2/audio/MORE!!!.mp3"
        );
        var player = Owner;
        if (player != null)
            await CreatureCmd.Heal(player.Creature, DynamicVars.Heal.BaseValue);
        if (RandomCardToLose != null)
            await CardPileCmd.RemoveFromDeck(RandomCardToLose);
        GetNewRandomCard();
        _stop = Rng.NextInt(101);
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
            [
                new EventOption(
                    this,
                    MoreDrink,
                    ModOptionKey("MORE_DRINK", "MORE_DRINK"),
                    HoverTipFactory.FromCard(RandomCardToLose!)
                ),
            ]
        );
    }

    private Task Leave()
    {
        SetEventFinished(PageDescription("LEAVE1"));
        return Task.CompletedTask;
    }
}
