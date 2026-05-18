using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Cards;
using LexNinja2.LexNinja2Code.Cards.Curses;
using LexNinja2.LexNinja2Code.Cards.Tokens;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Event;

public class TheGreatSeal : CustomEventModel
{
    public override string? CustomInitialPortraitPath =>
        "res://LexNinja2/images/events/TheGreatSeal.png";
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(10, ValueProp.Unblockable | ValueProp.Unpowered)];

    public override ActModel[] Acts => [ModelDb.Act<Glory>()];

    // public override bool IsAllowed(IRunState runState)
    // {
    //     if (runState.Act!=ModelDb.Act<Glory>())
    //     {
    //         return false;
    //     }
    //     foreach (Player player in (IEnumerable<Player>) runState.Players)
    //     {
    //         if ((Decimal) player.Creature.CurrentHp <= this.DynamicVars.Damage.BaseValue)
    //             return false;
    //     }
    //     // if (runState.Players.Count == 1)
    //     //     return true;
    //     return true;
    // }

    protected override Task BeforeEventStarted(bool isPreFinished)
    {
        // NinjaAudio.Stop("res://LexNinja2/audio/TheGreatSeal.mp3",10f);
        NinjaAudio.Stop("res://LexNinja2/audio/TheGreatSeal2.mp3", 10f);
        NinjaAudio.Play("res://LexNinja2/audio/TheGreatSeal0.mp3");
        MegaCrit.Sts2.Core.Commands.Cmd.Wait(2f, true);
        NinjaAudio.Play("res://LexNinja2/audio/Hamood.mp3", 0.25f);
        // NinjaAudio.PlayLooped("res://LexNinja2/audio/TheGreatSeal.mp3",0.25f);
        NinjaAudio.PlayLooped("res://LexNinja2/audio/TheGreatSeal2.mp3", 0.25f);
        return Task.CompletedTask;
    }

    protected override void OnEventFinished()
    {
        // NinjaAudio.Stop("res://LexNinja2/audio/TheGreatSeal.mp3",15f);
        NinjaAudio.Stop("res://LexNinja2/audio/TheGreatSeal2.mp3", 15f);
    }

    protected override IReadOnlyList<EventOption> GenerateInitialOptions() =>
        [
            Option(SaveHamood, HoverTipFactory.FromCardWithCardHoverTips<Normality>()),
            Option(KillHamood),
            Option(HamoodKillAll, HoverTipFactory.FromCardWithCardHoverTips<HamoodKillAll>()),
        ];

    private async Task SaveHamood()
    {
        CardCmd.PreviewCardPileAdd(
            await CardPileCmd.Add(Owner.RunState.CreateCard<Normality>(base.Owner), PileType.Deck)
        );
        for (int i = 0; i < 2; i++)
        {
            await RelicCmd.Obtain(
                RelicFactory.PullNextRelicFromFront(base.Owner, RelicRarity.Rare).ToMutable(),
                base.Owner
            );
        }
        SetEventFinished(PageDescription("OPTION_1"));
    }

    private async Task KillHamood()
    {
        await CreatureCmd.Damage(
            new ThrowingPlayerChoiceContext(),
            Owner!.Creature,
            DynamicVars.Damage,
            null,
            null
        );
        await CardPileCmd.RemoveFromDeck(
            (
                await CardSelectCmd.FromDeckForRemoval(
                    base.Owner,
                    new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, 2)
                )
            ).ToList()
        );
        SetEventFinished(PageDescription("OPTION_2"));
        NinjaAudio.Play("res://LexNinja2/audio/KillYourGrandpa.mp3");
        await MegaCrit.Sts2.Core.Commands.Cmd.Wait(2.5f);
        NinjaAudio.Play("res://LexNinja2/audio/HamoodKick.mp3");
        NDebugAudioManager.Instance.Play("blunt_attack.mp3", 0.8f, PitchVariance.Medium);
        await MegaCrit.Sts2.Core.Commands.Cmd.Wait(1.5f);
        NDebugAudioManager.Instance.Play("heavy_attack.mp3", 0.8f, PitchVariance.Medium);
        NinjaAudio.Play("res://LexNinja2/audio/KillHamood.mp3");
    }

    private async Task HamoodKillAll()
    {
        NinjaAudio.Play("res://LexNinja2/audio/Hamood.mp3");
        NinjaAudio.Play("res://LexNinja2/audio/KillAll.mp3");
        CardCmd.PreviewCardPileAdd(
            await CardPileCmd.Add(
                Owner.RunState.CreateCard<HamoodKillAll>(base.Owner),
                PileType.Deck
            )
        );
        SetEventFinished(PageDescription("OPTION_3"));
    }
}
