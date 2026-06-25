using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Character;
using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace LexNinja2.LexNinja2Code.Api.Cards;

[RegisterCard(typeof(LexNinja2CardPool), Inherit = true)]
public abstract class LexNinja2Card(int cost, CardType type, CardRarity rarity, TargetType target)
    : LexNinja2BaseCard(cost, type, rarity, target)
{
    public override CardAssetProfile AssetProfile =>
        new(
            PortraitPath: $"{GetType().Name}.png".CardImagePath(),
            BetaPortraitPath: $"beta/{GetType().Name}.png".CardImagePath()
        );
}
