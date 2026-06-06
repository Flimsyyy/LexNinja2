using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;

namespace LexNinja2.LexNinja2Code.Encounter;

public class ManYuDianEncounter : CustomEncounterModel
{
    public override bool IsValidForAct(ActModel act) => false;

    public override IEnumerable<MonsterModel> AllPossibleMonsters =>
        [ModelDb.Monster<TerrorEel>(), ModelDb.Monster<PhantasmalGardener>()];

    public override string? CustomScenePath => "res://LexNinja2/scenes/man_yu_dian_encounter.tscn";

    public override IReadOnlyList<string> Slots => ["first", "second", "third"];

    public ManYuDianEncounter()
        : base(RoomType.Monster) // 这个遭遇的房间类型，这里是普通怪物
    { }

    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters() =>
        [
            (ModelDb.Monster<TerrorEel>().ToMutable(), "first"),
            (ModelDb.Monster<PhantasmalGardener>().ToMutable(), "second"),
            (ModelDb.Monster<PhantasmalGardener>().ToMutable(), "third"),
        ];
}
