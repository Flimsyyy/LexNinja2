using SmartFormat.Core.Extensions;
using STS2RitsuLib.Interop.AutoRegistration;

namespace LexNinja2.LexNinja2Code.Api.SmartFormatters;

[RegisterSmartFormatSource]
public class IfChallengeModeSource : ISource
{
    public bool TryEvaluateSelector(ISelectorInfo selectorInfo)
    {
        if (selectorInfo.SelectorText != "IfChallengeMode")
        {
            return false;
        }

        selectorInfo.Result = NinjaConfig.IsChallengeMode();
        return true;
    }
}
