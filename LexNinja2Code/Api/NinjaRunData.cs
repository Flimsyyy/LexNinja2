using MegaCrit.Sts2.Core.Modding;
using STS2RitsuLib;
using STS2RitsuLib.RunData;

namespace LexNinja2.LexNinja2Code.Api;

[ModInitializer(nameof(Initialize))]
public class NinjaRunData
{
    public static RunSavedData<DirtyDataRunState>? DirtyData;

    public static void Initialize()
    {
        using (RitsuLibFramework.BeginModDataRegistration(MainFile.ModId))
        {
            var store = RitsuLibFramework.GetRunSavedDataStore(MainFile.ModId);
            DirtyData = store.Register(
                "dirty_data",
                () => new DirtyDataRunState(),
                new RunSavedDataOptions
                {
                    WritePolicy = RunSavedDataWritePolicy.AlwaysWhenRegistered,
                    SyncLobbyOnChange = true,
                }
            );
        }
    }

    public sealed class DirtyDataRunState
    {
        public bool IsChallengeMode { get; set; } = NinjaConfig.IsChallengeMode();
        public bool IsDirtyData { get; set; } = false;
    }
}
