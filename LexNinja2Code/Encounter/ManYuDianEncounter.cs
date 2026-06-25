using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace LexNinja2.LexNinja2Code.Encounter;

[RegisterGlobalEncounter]
public class ManYuDianEncounter : ModEncounterTemplate
{
    public override bool IsValidForAct(ActModel act) => false;

    public override IEnumerable<MonsterModel> AllPossibleMonsters =>
        [ModelDb.Monster<TerrorEel>(), ModelDb.Monster<PhantasmalGardener>()];

    public override string? CustomEncounterScenePath =>
        "res://LexNinja2/scenes/man_yu_dian_encounter.tscn";

    public override IReadOnlyList<string> Slots => ["first", "second", "third"];

    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters() =>
        [
            (ModelDb.Monster<TerrorEel>().ToMutable(), "first"),
            (ModelDb.Monster<PhantasmalGardener>().ToMutable(), "second"),
            (ModelDb.Monster<PhantasmalGardener>().ToMutable(), "third"),
        ];

    public override RoomType RoomType => RoomType.Monster;
}
