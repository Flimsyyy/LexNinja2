using LexNinja2.LexNinja2Code.Character;
using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Interop.AutoRegistration;

namespace LexNinja2.LexNinja2Code.Api.Cards;

[RegisterCard(typeof(LexNinja2CardPool), Inherit = true)]
public abstract class LexNinja2NinjutsuCard(
    int cost,
    CardType type,
    CardRarity rarity,
    TargetType target
) : NinjutsuCard(cost, type, rarity, target);
