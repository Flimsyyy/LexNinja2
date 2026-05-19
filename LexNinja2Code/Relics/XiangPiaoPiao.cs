using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Character;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Rooms;

namespace LexNinja2.LexNinja2Code.Relics;

[Pool(typeof(LexNinja2RelicPool))]
public class XiangPiaoPiao() : LexNinja2Relic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<Lexkela>()];

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is not CombatRoom)
            return;
        Flash();
        await PowerCmd.Apply<Pain>(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            1,
            null,
            null
        );
        await PowerCmd.Apply<Lexkela>(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            2,
            null,
            null
        );
    }

    public override async Task AfterSideTurnStart(CombatSide side, ICombatState combatState)
    {
        if (side != Owner.Creature.Side)
            return;
        Flash();
        NinjaAudio.Play("res://LexNinja2/audio/XiangPiaoPiao.mp3");
        await PowerCmd.Apply<Lexkela>(new ThrowingPlayerChoiceContext(), Owner.Creature, 1, null, null);
    }

    public override string PackedIconPath => "XiangPiaoPiao.png".RelicImagePath();
    protected override string PackedIconOutlinePath => "XiangPiaoPiao.png".RelicImagePath();
    protected override string BigIconPath => "XiangPiaoPiao.png".BigRelicImagePath();

    public override Task BeforeDeath(Creature creature)
    {
        if (creature != this.Owner.Creature)
            return Task.CompletedTask;
        NinjaAudio.Play("res://LexNinja2/audio/Cry.mp3");
        return Task.CompletedTask;
    }
}
