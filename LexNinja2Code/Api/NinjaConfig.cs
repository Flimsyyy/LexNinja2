using STS2RitsuLib.Settings;

namespace LexNinja2.LexNinja2Code.Api;

[ModSettingsPage(MainFile.ModId)]
[ModSettingsSection("general", TitleLocKey = "LEX_NINJA2_SETTINGS_UI_SECTION_GENERAL")]
public sealed class NinjaConfig
{
    [ModSettingsToggle(
        "challengeMode",
        "general",
        LabelLocKey = "LEX_NINJA2_SETTINGS_UI_TOGGLE_CHALLENGE_MODE.title",
        DescriptionLocKey = "LEX_NINJA2_SETTINGS_UI_TOGGLE_CHALLENGE_MODE.description"
    )]
    [ModSettingsBinding(Source = ModSettingsReflectionBindingSource.Global)]
    public static bool ChallengeMode { get; set; } = false;
}
