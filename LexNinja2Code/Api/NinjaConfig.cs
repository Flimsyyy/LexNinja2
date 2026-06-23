using BaseLib.Config;

namespace LexNinja2.LexNinja2Code.Api;

public sealed class NinjaConfig : SimpleModConfig
{
    public static bool ChallengeMode { get; set; } = false;
}
