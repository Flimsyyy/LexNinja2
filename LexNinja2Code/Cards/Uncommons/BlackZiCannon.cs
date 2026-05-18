using BaseLib.Utils;
using Godot;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards.Uncommons;

public class BlackZiCannon()
    : LexNinja2Card(4, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(26, ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<Lexkela>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Science];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/BlackZiCannon.mp3");
        var creatureNode = NCombatRoom.Instance?.GetCreatureNode(play.Target);
        if (creatureNode != null)
        {
            var child = NLargeMagicMissileVfx.Create(
                creatureNode.GetBottomOfHitbox(),
                new Color("50b598")
            );
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child!);
            await Cmd.Wait(child!.WaitTime);
        }
        await CommonActions
            .CardAttack(this, play, tmpSfx: "blunt_attack.mp3")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(8);
    }

    public override string CustomPortraitPath => $"BlackZiCannon_p.png".BigCardImagePath();
    public override string PortraitPath => $"BlackZiCannon.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/BlackZiCannon.png".CardImagePath();
}
