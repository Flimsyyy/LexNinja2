using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Cards.Curses;
using LexNinja2.LexNinja2Code.Encounter;
using LexNinja2.LexNinja2Code.Relics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace LexNinja2.LexNinja2Code.Event;

[RegisterActEvent(typeof(Hive))]
public class ManYuDian : ModEventTemplate
{
    public override string? CustomInitialPortraitPath =>
        "res://LexNinja2/images/events/ManYuDian.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(25), new GoldVar(220)];

    public override bool IsAllowed(IRunState runState) =>
        runState.Players.All(p => p.Gold >= DynamicVars.Gold.BaseValue);

    public override bool IsShared => true;

    protected override Task BeforeEventStarted(bool isPreFinished)
    {
        NinjaAudio.Play("res://LexNinja2/audio/ManyuDian.mp3");
        return Task.CompletedTask;
    }

    protected override IReadOnlyList<EventOption> GenerateInitialOptions() =>
        [
            new(
                this,
                YuanShen,
                InitialOptionKey("YUAN_SHEN"),
                HoverTipFactory.FromCardWithCardHoverTips<PrimalForce>()
            ),
            new(this, ManYu, InitialOptionKey("MAN_YU"), HoverTipFactory.FromRelic<ToiletWater>()),
            new(
                this,
                Leave,
                InitialOptionKey("LEAVE"),
                HoverTipFactory.FromCardWithCardHoverTips<MosquitoHand>()
            ),
        ];

    private async Task YuanShen()
    {
        NinjaAudio.Play("res://LexNinja2/audio/YuanShen.mp3");
        await PlayerCmd.LoseGold(DynamicVars.Gold.BaseValue, Owner!);
        CardCmd.PreviewCardPileAdd(
            await CardPileCmd.Add(Owner!.RunState.CreateCard<PrimalForce>(Owner), PileType.Deck)
        );
        await CreatureCmd.Heal(Owner!.Creature, DynamicVars.Heal.BaseValue);
        SetEventFinished(PageDescription("YUAN_SHEN"));
    }

    private async Task ManYu()
    {
        NinjaAudio.Play("res://LexNinja2/audio/KillFor500.mp3");
        await RelicCmd.Obtain<ToiletWater>(Owner!);
        FightPage();
    }

    private void FightPage()
    {
        SetEventState(
            PageDescription("FIGHT"),
            [new EventOption(this, Fight, ModOptionKey("FIGHT", "FIGHT"))]
        );
    }

    private Task Fight()
    {
        var list = Owner
            ?.RunState.Players.Select(player => new GoldReward(22, player))
            .Cast<Reward>()
            .ToList();
        if (list != null)
            EnterCombatWithoutExitingEvent<ManYuDianEncounter>(
                list,
                shouldResumeAfterCombat: false
            );
        return Task.CompletedTask;
    }

    private async Task Leave()
    {
        NinjaAudio.Play("res://LexNinja2/audio/MosquitoHand.mp3");
        await Cmd.Wait(0.5f);
        NinjaAudio.Play("res://LexNinja2/audio/Mosquito2.mp3", 0.4f);
        CardCmd.PreviewCardPileAdd(
            await CardPileCmd.Add(Owner!.RunState.CreateCard<MosquitoHand>(Owner), PileType.Deck)
        );
        SetEventFinished(PageDescription("LEAVE"));
    }
}
