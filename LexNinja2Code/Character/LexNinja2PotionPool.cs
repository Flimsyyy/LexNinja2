using Godot;
using LexNinja2.LexNinja2Code.Api.Extensions;
using STS2RitsuLib.Scaffolding.Content;

namespace LexNinja2.LexNinja2Code.Character;

public class LexNinja2PotionPool : TypeListPotionPoolModel
{
    public override string EnergyColorName => LexNinja2.CharacterId;
    public override Color LabOutlineColor => LexNinja2.Color;

    public override string BigEnergyIconPath => "charui/NINJAOrb.png".ImagePath();
    public override string TextEnergyIconPath => "charui/energyOrb.png".ImagePath();
}
