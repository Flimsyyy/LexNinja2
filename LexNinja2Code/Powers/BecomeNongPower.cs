using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace LexNinja2.LexNinja2Code.Powers;

public class BecomeNongPower : CustomPowerModel
{
    private const string CardKey = "Card";
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    protected override object InitInternalData() => new Data();

    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar(CardKey, "滚木")];

    public override string CustomPackedIconPath => "BecomeNongPower.png".PowerImagePath();
    public override string? CustomBigIconPath => "BecomeNongPower.png".BigPowerImagePath();

    // public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    // {
    //     if (side != this.Owner.Side)
    //     {
    //         return;
    //     }
    //     Flash();
    //     if (GetInternalData<Data>().selectedCard.Owner == Owner.Player  && !CardsToSkip.Remove(GetInternalData<Data>().selectedCard))
    //     {
    //         Flash();
    //         CardModel cardModel = base.Owner.Player.RunState.CloneCard(GetInternalData<Data>().selectedCard);
    //         CardsToSkip.Add(cardModel);
    //         CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(cardModel, PileType.Deck, CardPilePosition.Bottom, this));
    //         await PowerCmd.Remove(this);
    //
    //     }
    // }

    public override async Task AfterCombatEnd(CombatRoom room)
    {
        // GD.Print("HELLO WORLD");
        // CardPileCmd.Add(this.GetInternalData<BecomeNongPower.Data>().selectedCard, PileType.Deck, source: (AbstractModel)this);
        // return Task.CompletedTask;
        if (GetInternalData<Data>().SelectedCard == null)
            return;
        Flash();
        NinjaAudio.Play("res://LexNinja2/audio/BecomeNong.mp3", 1);
        await Cmd.Wait(1f);
        NinjaAudio.Play("res://LexNinja2/audio/BingBong.mp3", 0.3f);
        var card = GetInternalData<Data>().SelectedCard;
        if (card == null)
        {
            return;
        }

        for (var i = 0; i < Amount; i++)
        {
            var cardModel = Owner.Player!.RunState.CloneCard(
                card
            );
            CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(cardModel, PileType.Deck));
        }
    }

    public void SetSelectedCard(CardModel card)
    {
        var targetCard = card.CreateClone();
        GetInternalData<Data>().SelectedCard = targetCard;
        ((StringVar)DynamicVars[CardKey]).StringValue =
            targetCard.Title;
    }

    private class Data
    {
        public CardModel? SelectedCard;
    }

    public CardModel? GetNongCard()
    {
        var card = GetInternalData<Data>().SelectedCard;
        return card == null ? null : Owner.Player?.RunState.CloneCard(card);
    }

    // private HashSet<CardModel> CardsToSkip
    // {
    //     get
    //     {
    //         this.AssertMutable();
    //         if (this._cardsToSkip == null)
    //             this._cardsToSkip = new HashSet<CardModel>();
    //         return this._cardsToSkip;
    //     }
    // }
    // private HashSet<CardModel>? _cardsToSkip;
}
