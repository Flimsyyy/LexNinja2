using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace LexNinja2.LexNinja2Code.Api;

[RegisterOwnedCardKeyword(
    nameof(RenShu),
    CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.None
)]
[RegisterOwnedCardKeyword(
    nameof(Hand),
    CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.AfterCardDescription
)]
[RegisterOwnedCardKeyword(
    nameof(Blade),
    CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.AfterCardDescription
)]
[RegisterOwnedCardKeyword(
    nameof(Science),
    CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.AfterCardDescription
)]
public class NinjaKeyword
{
    public static readonly CardKeyword RenShu = ModContentRegistry
        .GetQualifiedKeywordId(MainFile.ModId, nameof(RenShu))
        .GetModCardKeyword();

    public static readonly CardKeyword Hand = ModContentRegistry
        .GetQualifiedKeywordId(MainFile.ModId, nameof(Hand))
        .GetModCardKeyword();

    public static readonly CardKeyword Blade = ModContentRegistry
        .GetQualifiedKeywordId(MainFile.ModId, nameof(Blade))
        .GetModCardKeyword();

    public static readonly CardKeyword Science = ModContentRegistry
        .GetQualifiedKeywordId(MainFile.ModId, nameof(Science))
        .GetModCardKeyword();
}
