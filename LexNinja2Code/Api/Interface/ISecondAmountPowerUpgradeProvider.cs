using LexNinja2.LexNinja2Code.Api.Powers;

namespace LexNinja2.LexNinja2Code.Api.Interface;

public interface ISecondAmountPowerUpgradeProvider
{
    bool TryUpgradeSecondAmount(SecondAmountPower power);
}
