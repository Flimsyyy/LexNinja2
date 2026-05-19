using BaseLib.Extensions;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.TextEffects;
using SmartFormat.Core.Extensions;

namespace LexNinja2.LexNinja2Code.Api.Formatters;

public class RenShuFormatter : IFormatter
{
    public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
    {
        if (formattingInfo.CurrentValue is not NinjutsuVar renShu)
        {
            return false;
        }
        var owner = renShu.GetOwner();
        if (
            owner is CardModel card
            && (
                card.Keywords.Contains(NinjaKeyword.FreeNinjutsu)
                || card.Owner.HasPower<FreeNinjutsuPower>()
            )
        )
        {
            formattingInfo.Write(StsTextUtilities.HighlightChangeText(0.ToString(), 1));
            return true;
        }
        formattingInfo.Write(renShu.ToHighlightedString(true));
        return true;
    }

    public string Name
    {
        get => "renShu";
        set => throw new NotImplementedException();
    }

    public bool CanAutoDetect { get; set; }
}
