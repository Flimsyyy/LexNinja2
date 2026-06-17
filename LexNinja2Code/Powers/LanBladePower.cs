using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Cards.Tokens;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace LexNinja2.LexNinja2Code.Powers;

public class LanBladePower : CustomPowerModel
{
    protected override object InitInternalData() => new Data();

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(NinjaKeyword.Blade), HoverTipFactory.FromCard<LanBlade>()];

    public override string CustomPackedIconPath => "LanBladePower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "LanBladePower84.png".BigPowerImagePath();

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        var cardPlayed = cardPlay.Card;
        if (cardPlayed.Owner != Owner.Player)
        {
            return;
        }
        if (!NinjaHelper.IsBladeRenShu(cardPlayed))
        {
            return;
        }
        if (cardPlayed is LanBlade)
        {
            return;
        }
        if (!GetInternalData<Data>().IsActive)
        {
            GetInternalData<Data>().IsActive = true;
            return;
        }
        NinjaAudio.Play("res://LexNinja2/audio/LanBlade.mp3");
        for (var i = 0; i < Amount; i++)
        {
            var card = CombatState.CreateCard<LanBlade>(Owner.Player);
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner.Player);
        }
    }

    private class Data
    {
        public bool IsActive;
    }
}
