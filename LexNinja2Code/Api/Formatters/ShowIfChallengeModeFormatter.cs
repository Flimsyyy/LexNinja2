using MegaCrit.Sts2.Core.Localization;
using SmartFormat.Core.Extensions;
using STS2RitsuLib.Interop.AutoRegistration;

namespace LexNinja2.LexNinja2Code.Api.Formatters;

[RegisterSmartFormatter]
public class ShowIfChallengeModeFormatter : IFormatter
{
    public string Name
    {
        get => "IfChallengeMode";
        set => throw new NotSupportedException("Setting the 'Names' property is not supported.");
    }
    public bool CanAutoDetect { get; set; }

    public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
    {
        if (formattingInfo.CurrentValue is not "LexNinja2")
        {
            return false;
        }
        var formatList = formattingInfo.Format?.Split('|');
        if (formatList == null)
            throw new LocException(
                $"Format expression must contain at least 1 option. format={formattingInfo.Format}."
            );
        var left =
            formatList.Count <= 2
                ? formatList[0]
                : throw new LocException(
                    $"Format expression cannot contain more than 2 options. num_of_options={formatList.Count} format={formattingInfo.Format}."
                );
        var right = formatList.Count > 1 ? formatList[1] : null;
        if (NinjaConfig.ChallengeMode)
        {
            formattingInfo.FormatAsChild(left, formattingInfo.CurrentValue);
        }
        else if (right != null)
        {
            formattingInfo.FormatAsChild(right, formattingInfo.CurrentValue);
        }
        return true;
    }
}
