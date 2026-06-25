using System;
using System.Linq;
using System.Text;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using SmartFormat.Core.Extensions;
using STS2RitsuLib.Cards.FreePlay;
using STS2RitsuLib.Combat.SecondaryResources;
using STS2RitsuLib.Interop.AutoRegistration;

namespace LexNinja2.LexNinja2Code.Api.Formatters;

[RegisterSmartFormatter]
public class RenShuFormatter : IFormatter
{
    public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
    {
        if (formattingInfo.CurrentValue is not NinjutsuVar renShu)
        {
            return false;
        }
        var owner = renShu._owner;
        if (owner is not NinjutsuCard card)
        {
            return false;
        }
        if (card.HasLexKelaCostX)
        {
            formattingInfo.Write("X");
            return true;
        }
        if (owner is not { IsMutable: true })
        {
            formattingInfo.Write(renShu.ToHighlightedString(true));
            return true;
        }

        var requestCost = card.DynamicVars.Ninjutsu().BaseValue;
        if (requestCost == 0)
        {
            formattingInfo.Write(0.ToString());
            return true;
        }
        var plan = SecondaryResourcePaymentResolver.Plan(
            card,
            FreePlayBindingRegistry.IsCardFreeForUpcomingPlay(card)
        );
        var line = plan.Lines.FirstOrDefault(line => line.Definition.Id == LexKela.Id);
        if (line == null)
        {
            return false;
        }

        formattingInfo.Write(GetLexKelaText(card, line.Cost));
        return true;
    }

    public string Name
    {
        get => "renShu";
        set => throw new NotImplementedException();
    }

    public bool CanAutoDetect { get; set; }

    private static string GetLexKelaText(NinjutsuCard card, int cost)
    {
        var sb = new StringBuilder();
        var color = NinjaColor.GetLexKelaCostColor(card).GetColorName();
        var hasColor = color != null;
        if (hasColor)
        {
            sb.Append('[');
            sb.Append(color);
            sb.Append(']');
        }
        sb.Append(cost);
        if (!hasColor)
            return sb.ToString();
        sb.Append("[/");
        sb.Append(color);
        sb.Append(']');

        return sb.ToString();
    }
}
