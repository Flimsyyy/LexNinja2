using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards.Uncommons;

public class BuildSandWall()
    : LexNinja2NinjutsuCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [HoverTipFactory.FromPower<SandWall>()];
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new NinjutsuVar(1),
            new PowerVar<BuildSandWallPower>(1),
            new CalculationBaseVar(0),
            new CalculationExtraVar(1),
            new CalculatedBlockVar(ValueProp.Move).WithMultiplier(
                (card, _) => card.Owner.Creature.GetPowerAmount<SandWall>()
            ),
        ];

    public override bool GainsBlock => true;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/BuildSandWall.mp3");
        await CommonActions.ApplySelf<BuildSandWallPower>(choiceContext, this);
        if (!Ninjutsu(play))
        {
            return;
        }
        await CommonActions.CardBlock(this, play);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public override string CustomPortraitPath => $"BuildSandWall_p.png".BigCardImagePath();
    public override string PortraitPath => $"BuildSandWall.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/BuildSandWall.png".CardImagePath();
}
