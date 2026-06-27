using STS2RitsuLib;
using STS2RitsuLib.Data;
using STS2RitsuLib.Settings;
using STS2RitsuLib.Utils.Persistence;

namespace LexNinja2.LexNinja2Code.Api;

public sealed class NinjaConfigs
{
    public bool ChallengeMode { get; set; }
}

public static class NinjaConfigsPage
{
    private const string DataKey = "settings";

    private static readonly ModSettingsValueBinding<NinjaConfigs, bool> ChallengeModeBinding = new(
        MainFile.ModId,
        DataKey,
        SaveScope.Profile,
        static s => s.ChallengeMode,
        static (s, v) => s.ChallengeMode = v
    );

    public static void Register()
    {
        ModDataStore
            .For(MainFile.ModId)
            .Register(
                key: DataKey,
                fileName: "settings.json",
                scope: SaveScope.Global,
                defaultFactory: () => new NinjaConfigs(),
                autoCreateIfMissing: true
            );

        RitsuLibFramework.RegisterModSettings(
            MainFile.ModId,
            page =>
                page.WithTitle(
                        ModSettingsText.LocString(
                            "settings_ui",
                            "LEX_NINJA2_SETTINGS_UI_LEX_NINJA2_MODNAME",
                            "LexNinja2"
                        )
                    )
                    .WithModDisplayName(
                        ModSettingsText.LocString(
                            "settings_ui",
                            "LEX_NINJA2_SETTINGS_UI_LEX_NINJA2_MODNAME",
                            "LexNinja2"
                        )
                    )
                    .WithVisibleOnHostSurfaces(ModSettingsHostSurface.MainMenu)
                    .AddSection(
                        "general",
                        section =>
                            section
                                .WithTitle(
                                    ModSettingsText.LocString(
                                        "settings_ui",
                                        "LEX_NINJA2_SETTINGS_UI_SECTION_GENERAL",
                                        "General"
                                    )
                                )
                                .AddToggle(
                                    "challengeMode",
                                    ModSettingsText.LocString(
                                        "settings_ui",
                                        "LEX_NINJA2_SETTINGS_UI_TOGGLE_CHALLENGE_MODE.title",
                                        "Challenge Mode"
                                    ),
                                    ChallengeModeBinding,
                                    ModSettingsText.LocString(
                                        "settings_ui",
                                        "LEX_NINJA2_SETTINGS_UI_TOGGLE_CHALLENGE_MODE.description",
                                        "Challenge Mode"
                                    )
                                )
                    )
        );
    }

    public static bool IsChallengeMode()
    {
        return ChallengeModeBinding.Read();
    }
}
