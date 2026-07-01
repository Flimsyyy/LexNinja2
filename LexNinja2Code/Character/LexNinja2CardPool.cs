using Godot;
using LexNinja2.LexNinja2Code.Api.Extensions;
using STS2RitsuLib.Scaffolding.Characters;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Utils;

namespace LexNinja2.LexNinja2Code.Character;

public class LexNinja2CardPool : TypeListCardPoolModel, IModColorfulPhilosophersCardPool
{
    public override string Title => LexNinja2.CharacterId; //This is not a display name.
    public override string EnergyColorName => LexNinja2.CharacterId;

    public override string BigEnergyIconPath => "charui/NINJAOrb.png".ImagePath();
    public override string TextEnergyIconPath => "charui/energyOrb.png".ImagePath();

    private static readonly Material? _poolFrameMaterial = MaterialUtils.CreateHsvShaderMaterial(
        0.1f,
        0f,
        0.33f
    );
    public override Material? PoolFrameMaterial => _poolFrameMaterial;

    public override Color DeckEntryCardColor => LexNinja2.Color;

    public override bool IsColorless => false;
}
