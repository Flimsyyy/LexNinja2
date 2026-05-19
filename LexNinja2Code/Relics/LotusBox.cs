using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Character;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace LexNinja2.LexNinja2Code.Relics;

[Pool(typeof(LexNinja2RelicPool))]
public class LotusBox() : LexNinja2Relic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<Lexkela>()];

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is not CombatRoom)
            return;
        Flash();
        NinjaAudio.Play("res://LexNinja2/audio/LotusBox.mp3");
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

    public override string PackedIconPath => "LotusBox.png".RelicImagePath();
    protected override string PackedIconOutlinePath => "/outline/LotusBox.png".RelicImagePath();
    protected override string BigIconPath => "LotusBox.png".BigRelicImagePath();

    public override RelicModel? GetUpgradeReplacement() => ModelDb.Relic<XiangPiaoPiao>();

    public override Task BeforeDeath(Creature creature)
    {
        if (creature != Owner.Creature)
            return Task.CompletedTask;
        NinjaAudio.Play("res://LexNinja2/audio/Cry.mp3");
        return Task.CompletedTask;
    }
}
