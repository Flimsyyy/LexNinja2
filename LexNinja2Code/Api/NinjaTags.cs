using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;

namespace LexNinja2.LexNinja2Code.Api;

[RegisterOwnedCardTag(nameof(Ninjutsu))]
[RegisterOwnedCardTag(nameof(Snake))]
[RegisterOwnedCardTag(nameof(Food))]
[RegisterOwnedCardTag(nameof(Holy))]
[RegisterOwnedCardTag(nameof(Mamba))]
public class NinjaTags
{
    public static readonly CardTag Ninjutsu = ModContentRegistry
        .GetQualifiedCardTagId(MainFile.ModId, nameof(Ninjutsu))
        .GetModCardTag();

    public static readonly CardTag Snake = ModContentRegistry
        .GetQualifiedCardTagId(MainFile.ModId, nameof(Snake))
        .GetModCardTag();

    public static readonly CardTag Food = ModContentRegistry
        .GetQualifiedCardTagId(MainFile.ModId, nameof(Food))
        .GetModCardTag();

    public static readonly CardTag Holy = ModContentRegistry
        .GetQualifiedCardTagId(MainFile.ModId, nameof(Holy))
        .GetModCardTag();

    public static readonly CardTag Mamba = ModContentRegistry
        .GetQualifiedCardTagId(MainFile.ModId, nameof(Mamba))
        .GetModCardTag();
}
