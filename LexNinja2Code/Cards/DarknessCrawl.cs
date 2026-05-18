using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards;

public class DarknessCrawl()
    : LexNinja2Card(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override bool HasEnergyCostX => true;
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new BlockVar(5, ValueProp.Move), new LexKelaVar(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/DarknessCrawl.mp3");
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        int evokeCount = ResolveEnergyXValue();
        if (IsUpgraded)
            evokeCount = evokeCount + 1;
        for (int i = 0; i < evokeCount; ++i)
        {
            NinjaAudio.Play("res://LexNinja2/audio/Crawl.mp3", 1);
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
            await PowerCmd.Apply<Lexkela>(
                new ThrowingPlayerChoiceContext(),
                Owner.Creature,
                DynamicVars["Kela"].BaseValue,
                Owner.Creature,
                this
            );
        }
    }

    protected override void OnUpgrade() { }

    public override string CustomPortraitPath => $"DarknessCrawl_p.png".BigCardImagePath();
    public override string PortraitPath => $"DarknessCrawl.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/DarknessCrawl.png".CardImagePath();
}
