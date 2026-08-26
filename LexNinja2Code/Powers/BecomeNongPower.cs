using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace LexNinja2.LexNinja2Code.Powers;

public class BecomeNongPower : LexNinja2Power
{
    private const string CardKey = "Card";
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    protected override object InitInternalData() => new Data();

    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar(CardKey, "滚木")];

    public override string CustomIconPath => "BecomeNongPower.png".PowerImagePath();
    public override string CustomBigIconPath => "BecomeNongPower.png".BigPowerImagePath();

    public override async Task AfterCombatEnd(CombatRoom room)
    {
        if (GetInternalData<Data>().SelectedCard == null)
            return;
        Flash();
        NinjaAudio.Play("res://LexNinja2/audio/BecomeNong.mp3", 1);
        await Cmd.Wait(1f);
        NinjaAudio.Play("res://LexNinja2/audio/BingBong.mp3", 0.3f);
        var card = GetInternalData<Data>().SelectedCard;
        if (card == null)
            return;

        if (Owner.CombatState!.RunState.Players.Count > 1)
        {
            //清除卡牌上多余的信息
            var cardToClone = CardModel.FromSerializable(card.ToSerializable());
            for (var i = 0; i < Amount; i++)
            {
                var cardModel = Owner.Player!.RunState.CloneCard(cardToClone);
                cardModel.Owner = Owner.Player!;
                CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(cardModel, PileType.Deck));
            }
            return;
        }

        for (var i = 0; i < Amount; i++)
        {
            var cardModel = Owner.Player!.RunState.CloneCard(card);
            CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(cardModel, PileType.Deck));
        }
    }

    public void SetSelectedCard(CardModel card)
    {
        GetInternalData<Data>().SelectedCard = card;
        ((StringVar)DynamicVars[CardKey]).StringValue = card.Title;
    }

    public bool IsEqual(CardModel card)
    {
        var selectedCard = GetInternalData<Data>().SelectedCard;
        if (selectedCard == null)
        {
            return false;
        }
        return selectedCard == card;
    }

    private class Data
    {
        public CardModel? SelectedCard;
    }

    public async Task Apply(
        PlayerChoiceContext choiceContext,
        decimal amount,
        CardModel? cardSource,
        bool silent = false
    )
    {
        if (CombatManager.Instance.IsEnding || amount == 0M || !Owner.CanReceivePowers)
        {
            return;
        }
        var combatState = Owner.CombatState;
        if (combatState == null)
        {
            return;
        }
        await PowerCmd.ModifyAmount(choiceContext, this, amount, Owner, cardSource);
    }

    public static async Task Apply(
        PlayerChoiceContext choiceContext,
        Player owner,
        decimal amount,
        CardModel cardSelected,
        CardModel? cardSource,
        bool silent = false
    )
    {
        var self = owner.Creature;
        if (CombatManager.Instance.IsEnding || amount == 0M || !self.CanReceivePowers)
        {
            return;
        }
        var combatState = self.CombatState;
        if (combatState == null)
        {
            return;
        }
        var basePower = ModelDb.Power<BecomeNongPower>();
        var instanceForStacking = self.GetPowerInstances(basePower.Id)
            .FirstOrDefault(p =>
                p is BecomeNongPower becomeNongPower && becomeNongPower.IsEqual(cardSelected)
            );
        if (instanceForStacking != null)
        {
            await PowerCmd.ModifyAmount(
                choiceContext,
                instanceForStacking,
                amount,
                self,
                cardSource
            );
            return;
        }
        var power = (BecomeNongPower)basePower.ToMutable();
        power.SetSelectedCard(cardSelected);
        power.AssertMutable();
        power.Applier = self;
        await Hook.BeforePowerAmountChanged(combatState, power, amount, self, self, cardSource);
        var modifiedAmount = amount;
        IEnumerable<AbstractModel>? givenModifiers = null;
        if (combatState.ContainsCreature(self))
            modifiedAmount = Hook.ModifyPowerAmountGiven(
                combatState,
                power,
                self,
                modifiedAmount,
                self,
                cardSource,
                out givenModifiers
            );
        modifiedAmount = Hook.ModifyPowerAmountReceived(
            combatState,
            power,
            self,
            modifiedAmount,
            self,
            out var receivedModifiers
        );
        await power.BeforeApplied(self, modifiedAmount, self, cardSource);
        if (!self.CanReceivePowers)
        {
            return;
        }
        power.ApplyInternal(self, modifiedAmount, silent);
        if (modifiedAmount != 0M)
            CombatManager.Instance.History.PowerReceived(combatState, power, modifiedAmount, self);
        if (CombatManager.Instance.IsInProgress)
            await Cmd.CustomScaledWait(0.1f, 0.25f);
        if (givenModifiers != null)
            await Hook.AfterModifyingPowerAmountGiven(combatState, givenModifiers, power);
        await Hook.AfterModifyingPowerAmountReceived(combatState, receivedModifiers, power);
        if (modifiedAmount != 0M)
        {
            await power.AfterApplied(self, cardSource);
            await Hook.AfterPowerAmountChanged(
                combatState,
                choiceContext,
                power,
                modifiedAmount,
                self,
                cardSource
            );
        }
    }
}
