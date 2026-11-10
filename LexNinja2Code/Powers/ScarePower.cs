using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Powers;

public class ScarePower : LexNinja2Power
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomIconPath => "ScarePower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "ScarePower84.png".BigPowerImagePath();

    public override async Task AfterPlayerTurnStart(
        PlayerChoiceContext choiceContext,
        Player player
    )
    {
        if (player != Owner.Player)
            return;
        NinjaAudio.Play("res://LexNinja2/audio/吓我一跳释放忍术.mp3");
        var cards = GetNinjutsuCards(Owner.Player);
        for (var i = 0; i < Amount; i++)
        {
            var card = CardFactory
                .GetDistinctForCombat(
                    Owner.Player,
                    cards,
                    1,
                    Owner.Player.RunState.Rng.CombatCardGeneration
                )
                .FirstOrDefault();
            if (card == null)
            {
                return;
            }
            // if (!(Owner.HasPower<WePeacePower>()&&card.Type==CardType.Attack))
            // {
            await CardCmd.AutoPlay(choiceContext, card.CreateDupe(player), null);
            // }
        }
    }

    private static List<CardModel> GetNinjutsuFromPool(CardPoolModel pool, Player player) =>
        pool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint)
            .Where(c => c.Tags.Contains(NinjaTags.Ninjutsu))
            .ToList();

    private static List<CardModel> GetNinjutsuCards(Player player)
    {
        var cards = GetNinjutsuFromPool(player.Character.CardPool, player);
        return cards.Count > 0
            ? cards
            : GetNinjutsuFromPool(ModelDb.Character<Character.LexNinja2>().CardPool, player);
    }
}
