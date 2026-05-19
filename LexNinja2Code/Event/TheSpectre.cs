using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Cards.Quests;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.Cards;

namespace LexNinja2.LexNinja2Code.Event;

public sealed class TheSpectre : CustomEventModel
{
    // 背景图位置
    public override string? CustomInitialPortraitPath =>
        "res://LexNinja2/images/events/TheSpectre.png";

    // 设置一些数值
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new StringVar("Card1", ModelDb.Card<ISeeYou>().Title),
            new StringVar("Card2", ModelDb.Card<Impatience>().Title),
        ];

    public override ActModel[] Acts => [ModelDb.Act<Overgrowth>(), ModelDb.Act<Underdocks>()];

    // public override bool IsAllowed(IRunState runState)
    // {
    //     if (runState.Act==ModelDb.Act<Overgrowth>()||runState.Act==ModelDb.Act<Underdocks>())
    //     {
    //         return true;
    //     }
    //     return false;
    // }

    // 事件开始前的逻辑。这里是禁止玩家移除药水
    protected override Task BeforeEventStarted(bool isPreFinished)
    {
        NinjaAudio.Stop("res://LexNinja2/audio/TheSpectre.mp3", 10f);
        NinjaAudio.PlayLooped("res://LexNinja2/audio/TheSpectre.mp3", 0.25f);
        return Task.CompletedTask;
    }

    // 事件结束后的逻辑。这里是允许玩家移除药水
    protected override void OnEventFinished()
    {
        Owner!.CanRemovePotions = true;
        NinjaAudio.Stop("res://LexNinja2/audio/TheSpectre.mp3", 15f);
    }

    // 生成事件初始选项。这里是两个选项：失去生命值或者失去金币，然后进入选择奖励阶段
    protected override IReadOnlyList<EventOption> GenerateInitialOptions() =>
        [
            Option(TakeTheKitten, HoverTipFactory.FromCardWithCardHoverTips<ISeeYou>()),
            Option(StopPostingThis, HoverTipFactory.FromCardWithCardHoverTips<Impatience>()),
        ];

    private async Task TakeTheKitten()
    {
        // NinjaAudio.Stop("res://LexNinja2/audio/TheSpectre.mp3",15f);
        NinjaAudio.Play("res://LexNinja2/audio/ISeeYou.mp3");
        CardCmd.PreviewCardPileAdd(
            await CardPileCmd.Add(Owner!.RunState.CreateCard<ISeeYou>(base.Owner), PileType.Deck)
        );
        SetEventFinished(PageDescription("KITTEN_CHOSEN"));
    }

    private async Task StopPostingThis()
    {
        NinjaAudio.Stop("res://LexNinja2/audio/TheSpectre.mp3", 15f);
        NinjaAudio.Play("res://LexNinja2/audio/ICantImagine.mp3");
        CardCmd.PreviewCardPileAdd(
            await CardPileCmd.Add(Owner!.RunState.CreateCard<Impatience>(base.Owner), PileType.Deck)
        );
        SetEventFinished(PageDescription("STOP_CHOSEN"));
    }

    // public override Task AfterRoomEntered(AbstractRoom room)
    // {
    //     NinjaAudio.Stop("res://LexNinja2/audio/TheSpectre.mp3",10f);
    //     return Task.CompletedTask;
    // }
}
