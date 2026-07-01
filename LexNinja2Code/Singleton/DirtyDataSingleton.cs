using System.Threading.Tasks;
using LexNinja2.LexNinja2Code.Api;
using MegaCrit.Sts2.Core.Runs;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models;

namespace LexNinja2.LexNinja2Code.Singleton;

[RegisterSingleton]
public class DirtyDataSingleton() : HookedSingletonModel(HookType.Combat)
{
    public static bool IsChallengeMode { get; private set; }
    public static bool IsDirtyData { get; private set; }

    public override Task BeforeCombatStart()
    {
        if (CurrentRunState is not RunState runState)
        {
            return Task.CompletedTask;
        }
        var dirtyRunData = NinjaRunData.DirtyData!.Get(runState);
        IsChallengeMode = NinjaConfig.IsChallengeMode();
        if (NinjaConfig.IsChallengeMode() == dirtyRunData.IsChallengeMode)
        {
            IsDirtyData = false;
            return Task.CompletedTask;
        }
        MainFile.Logger.Warn(
            "Due to a mismatch between the current difficulty and the Run's difficulty, this Run is marked as Dirty Data."
        );
        NinjaRunData.DirtyData.Modify(
            runState,
            data =>
            {
                data.IsChallengeMode = NinjaConfig.IsChallengeMode();
                data.IsDirtyData = true;
                IsDirtyData = true;
            }
        );
        return Task.CompletedTask;
    }
}
