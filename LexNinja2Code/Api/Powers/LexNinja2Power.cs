using LexNinja2.LexNinja2Code.Api.Extensions;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace LexNinja2.LexNinja2Code.Api.Powers;

[RegisterPower(Inherit = true)]
public abstract class LexNinja2Power : ModPowerTemplate
{
    public override PowerAssetProfile AssetProfile =>
        new(
            IconPath: $"{GetType().Name}.png".PowerImagePath(),
            BigIconPath: $"{GetType().Name}.png".BigPowerImagePath()
        );
}
