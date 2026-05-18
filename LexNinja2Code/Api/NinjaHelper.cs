using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Api;

public static class NinjaHelper
{
    public static async Task AddLexKela(PlayerChoiceContext ctx, CardModel card)
    {
        await PowerCmd.Apply<Lexkela>(
            ctx,
            card.Owner.Creature,
            card.DynamicVars.LexKela().BaseValue,
            card.Owner.Creature,
            card
        );
    }
}