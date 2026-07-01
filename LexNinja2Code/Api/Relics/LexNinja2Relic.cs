using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace LexNinja2.LexNinja2Code.Api.Relics;

[RegisterRelic(typeof(LexNinja2RelicPool), Inherit = true)]
public abstract class LexNinja2Relic : ModRelicTemplate
{
    public override RelicAssetProfile AssetProfile =>
        new(
            IconPath: $"{GetType().Name}.png".RelicImagePath(),
            IconOutlinePath: $"/outline/{GetType().Name}.png".RelicImagePath(),
            BigIconPath: $"{GetType().Name}.png".BigRelicImagePath()
        );
}
