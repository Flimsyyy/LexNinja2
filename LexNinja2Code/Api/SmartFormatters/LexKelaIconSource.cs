using SmartFormat.Core.Extensions;
using STS2RitsuLib.Interop.AutoRegistration;

namespace LexNinja2.LexNinja2Code.Api.SmartFormatters;

[RegisterSmartFormatSource]
public class LexKelaIconSource : ISource
{
    public bool TryEvaluateSelector(ISelectorInfo selectorInfo)
    {
        if (selectorInfo.SelectorText != "lexKelaIcon")
        {
            return false;
        }

        selectorInfo.Result = LexKela.Definition;
        return true;
    }
}
