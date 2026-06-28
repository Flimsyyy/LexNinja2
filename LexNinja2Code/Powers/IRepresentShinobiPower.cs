using System.Collections.Generic;
using System.Threading.Tasks;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Combat.SecondaryResources;

namespace LexNinja2.LexNinja2Code.Powers;

public class IRepresentShinobiPower : LexNinja2Power, ISecondaryResourceHookListener
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [HoverTipFactory.Static(StaticHoverTip.Energy), LexKela.HoverTip()];

    public bool IsUsedThisTurn { get; set; }

    public override string CustomIconPath => "IRepresentShinobiPower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "IRepresentShinobiPower84.png".BigPowerImagePath();

    public async Task AfterSecondaryResourceChanged(SecondaryResourceChangeContext context)
    {
        if (context.Definition.Id != LexKela.Id)
        {
            return;
        }

        if (context.Delta >= 0)
        {
            return;
        }

        var player = context.Player;

        if (player != Owner.Player)
        {
            return;
        }

        if (NinjaConfigsPage.IsChallengeMode())
        {
            if (IsUsedThisTurn)
            {
                return;
            }
            IsUsedThisTurn = true;
        }

        NinjaAudio.Play("res://LexNinja2/audio/IRepresentShinobi.mp3");
        await PlayerCmd.GainEnergy(Amount, Owner.Player);
    }

    public override Task BeforeHandDraw(
        Player player,
        PlayerChoiceContext choiceContext,
        ICombatState combatState
    )
    {
        if (!NinjaConfigsPage.IsChallengeMode() || player != Owner.Player)
        {
            return Task.CompletedTask;
        }
        IsUsedThisTurn = false;
        return Task.CompletedTask;
    }
}
