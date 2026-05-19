using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Cards.Commons;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Powers;

public class InfiniteSnakeBitesPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<AngrySnakeBite>(true)];

    public override string CustomPackedIconPath => "InfiniteSnakeBitesPower.png".PowerImagePath();
    public override string? CustomBigIconPath => "InfiniteSnakeBitesPower.png".BigPowerImagePath();

    public override async Task BeforeHandDraw(
        Player player,
        PlayerChoiceContext choiceContext,
        ICombatState combatState
    )
    {
        if (player != Owner.Player)
            return;
        Flash();
        NinjaAudio.Play("res://LexNinja2/audio/ShootOnThis.mp3");
        for (var i = 0; i < Amount; i++)
        {
            CardModel card = CombatState.CreateCard<AngrySnakeBite>(Owner.Player);
            CardCmd.Upgrade(card);
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, player);
        }
    }
}
