using System;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace LexNinja2.LexNinja2Code.Api.Extensions;

public static class CardCostColorExtensions
{
    public static string? GetColorName(this CardCostColor cardCostColor)
    {
        return cardCostColor switch
        {
            CardCostColor.Unmodified => null,
            CardCostColor.Increased => "blue",
            CardCostColor.Decreased => "green",
            CardCostColor.InsufficientResources => "red",
            _ => throw new ArgumentOutOfRangeException(nameof(cardCostColor), cardCostColor, null),
        };
    }
}
