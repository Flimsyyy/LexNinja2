using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Cards.Curses;
using LexNinja2.LexNinja2Code.Encounter;
using LexNinja2.LexNinja2Code.Relics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace LexNinja2.LexNinja2Code.Event;

public class ManYuDian : CustomEventModel
{
    public override string? CustomInitialPortraitPath =>
        "res://LexNinja2/images/events/ManYuDian.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(25), new GoldVar(220)];

    public override bool IsAllowed(IRunState runState) =>
        runState.Players.All(p => p.Gold >= DynamicVars.Gold.BaseValue);

    public override bool IsShared => true;

    public override ActModel[] Acts => [ModelDb.Act<Hive>()];

    protected override Task BeforeEventStarted(bool isPreFinished)
    {
        NinjaAudio.Play("res://LexNinja2/audio/ManyuDian.mp3");
        return Task.CompletedTask;
    }

    protected override IReadOnlyList<EventOption> GenerateInitialOptions() =>
        [
            Option(YuanShen, HoverTipFactory.FromCardWithCardHoverTips<PrimalForce>()),
            Option(ManYu, HoverTipFactory.FromRelic<ToiletWater>()),
            Option(Leave, HoverTipFactory.FromCardWithCardHoverTips<MosquitoHand>()),
        ];

    private async Task YuanShen()
    {
        await PlayerCmd.LoseGold(DynamicVars.Gold.BaseValue, Owner!);
        CardCmd.PreviewCardPileAdd(
            await CardPileCmd.Add(
                Owner!.RunState.CreateCard<PrimalForce>(base.Owner),
                PileType.Deck
            )
        );
        await CreatureCmd.Heal(Owner!.Creature, DynamicVars.Heal.BaseValue);
        SetEventFinished(PageDescription("YUAN_SHEN"));
    }

    private async Task ManYu()
    {
        await RelicCmd.Obtain<ToiletWater>(Owner!);
        FightPage();
    }

    private void FightPage()
    {
        SetEventState(PageDescription("FIGHT"), [Option(Fight, "FIGHT")]);
    }

    private async Task Fight()
    {
        List<Reward> list = new List<Reward>();
        foreach (Player player in base.Owner.RunState.Players)
        {
            list.Add(new GoldReward(22, player));
        }
        EnterCombatWithoutExitingEvent<ManYuDianEncounter>(list, shouldResumeAfterCombat: false);
    }

    private async Task Leave()
    {
        NinjaAudio.Play("res://LexNinja2/audio/MosquitoHand.mp3");
        await Cmd.Wait(0.5f);
        NinjaAudio.Play("res://LexNinja2/audio/Mosquito2.mp3", 0.4f);
        CardCmd.PreviewCardPileAdd(
            await CardPileCmd.Add(
                Owner!.RunState.CreateCard<MosquitoHand>(base.Owner),
                PileType.Deck
            )
        );
        SetEventFinished(PageDescription("LEAVE"));
    }
}
