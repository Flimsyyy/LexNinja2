using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace LexNinja2.LexNinja2Code.Api;

public class NinjaKeyword
{
    [CustomEnum("Renshu")]
    // 放在原版卡牌描述的位置，这里是卡牌描述的前面
    [KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Renshu;

    [CustomEnum("Hand")]
    // 放在原版卡牌描述的位置，这里是卡牌描述的前面
    [KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Hand;

    [CustomEnum("Blade")]
    // 放在原版卡牌描述的位置，这里是卡牌描述的前面
    [KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Blade;

    [CustomEnum("Science")]
    // 放在原版卡牌描述的位置，这里是卡牌描述的前面
    [KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Science;

    [CustomEnum("Sarira")]
    // 放在原版卡牌描述的位置，这里是卡牌描述的前面
    [KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Sarira;
}
