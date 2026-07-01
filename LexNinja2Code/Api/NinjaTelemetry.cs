using System.Linq;
using System.Text.Json.Nodes;
using LexNinja2.LexNinja2Code.Singleton;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib;
using STS2RitsuLib.Settings;
using STS2RitsuLib.Telemetry;

namespace LexNinja2.LexNinja2Code.Api;

public static partial class NinjaTelemetry
{
    private static ITelemetryClient Client = null!;
    private static bool _initialized;

    public static void Register()
    {
        if (_initialized)
        {
            return;
        }
        _initialized = true;

        RitsuLibFramework.RegisterTelemetryContributionProvider(new NinjaContributionProvider());

        TelemetryRegistry.RegisterApplicant(
            new TelemetryApplicant
            {
                ApplicantId = MainFile.ModId,
                OwnerModId = MainFile.ModId,
                DisplayName = MainFile.ModId,
                DisplayNameText = ModSettingsText.LocString(
                    "settings_ui",
                    "LEX_NINJA2_SETTINGS_UI_LEX_NINJA2_MODNAME",
                    "LexNinja2"
                ),
                Adapter = new PostHogTelemetryAdapter(
                    host: "https://ninjamod2-data.ninja-data.workers.dev",
                    projectApiKey: "proxy"
                ),
                Requests =
                [
                    TelemetryRequest.RunHistory(
                        ModSettingsText.LocString(
                            "settings_ui",
                            "LEX_NINJA2_SETTINGS_UI_TELEMETRY.run_history",
                            "Run History"
                        ),
                        sharedContributionSubscriptions: ["lex_ninja2_run_context"],
                        captureFilter: evt =>
                            !evt.IsAbandoned
                            && !DirtyDataSingleton.IsDirtyData
                            && evt.Run.Players.Any(player =>
                                player.CharacterId == ModelDb.Character<Character.LexNinja2>().Id
                            )
                    ),
                ],
            }
        );

        Client = TelemetryApi.GetClient(MainFile.ModId);
    }

    public class NinjaContributionProvider : ITelemetryContributionProvider
    {
        public string ContributorModId => MainFile.ModId;
        public string ContributionId => "lex_ninja2_run_context";
        public TelemetryDataCategory Category => TelemetryDataCategory.RunHistory;
        public TelemetryContributionVisibility Visibility =>
            TelemetryContributionVisibility.PrivateToApplicant;

        public JsonNode? Build(TelemetryContributionContext context)
        {
            return new JsonObject
            {
                ["version"] = MainFile.Version,
                ["is_challenge_mode"] = DirtyDataSingleton.IsChallengeMode,
                ["is_dirty_data"] = DirtyDataSingleton.IsDirtyData,
            };
        }
    }
}
