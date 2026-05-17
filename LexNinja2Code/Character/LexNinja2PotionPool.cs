using BaseLib.Abstracts;
using Godot;
using LexNinja2.LexNinja2Code.Extensions;

namespace LexNinja2.LexNinja2Code.Character;

public class LexNinja2PotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => LexNinja2.Color;

    public override string BigEnergyIconPath => "charui/NINJAOrb.png".ImagePath();
    public override string TextEnergyIconPath => "charui/energyOrb.png".ImagePath();
}
