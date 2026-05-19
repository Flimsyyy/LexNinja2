using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Character;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace LexNinja2.LexNinja2Code.Relics;

[Pool(typeof(LexNinja2RelicPool))]
public class DimDeadTree() : LexNinja2Relic
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    public override string PackedIconPath => "DimDeadTree.png".RelicImagePath();
    protected override string PackedIconOutlinePath => "/outline/DimDeadTree.png".RelicImagePath();
    protected override string BigIconPath => "DimDeadTree.png".BigRelicImagePath();

    public override async Task AfterSideTurnStart(CombatSide side, ICombatState combatState)
    {
        if (side != Owner.Creature.Side)
            return;
        Flash();
        NinjaAudio.Play("res://LexNinja2/audio/DimDeadTree.mp3");
        await PowerCmd.Apply<FreeNinjutsuPower>(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            1,
            null,
            null
        );
    }
}
