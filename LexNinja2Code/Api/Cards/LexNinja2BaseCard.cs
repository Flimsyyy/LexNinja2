using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Scaffolding.Content;

namespace LexNinja2.LexNinja2Code.Api.Cards;

public abstract class LexNinja2BaseCard(
    int cost,
    CardType type,
    CardRarity rarity,
    TargetType target
) : ModCardTemplate(cost, type, rarity, target)
{
    public override CardAssetProfile AssetProfile =>
        new(
            PortraitPath: $"{GetType().Name}.png".CardImagePath(),
            BetaPortraitPath: $"beta/{GetType().Name}.png".CardImagePath()
        );
}
