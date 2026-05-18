using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Relics;

[Pool(typeof(LexNinja2RelicPool))]
public class ReaperHand() : LexNinja2Relic
{
    public override RelicRarity Rarity => RelicRarity.Rare;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new SummonVar(2)];

    public override string PackedIconPath => "ReaperHand.png".RelicImagePath();
    protected override string PackedIconOutlinePath => "ReaperHand.png".RelicImagePath();
    protected override string BigIconPath => "ReaperHand.png".BigRelicImagePath();

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner)
        {
            return;
        }
        if (cardPlay.Card.Keywords.Contains(NinjaKeyword.Hand))
        {
            NinjaAudio.Play("res://LexNinja2/audio/DeathHand.mp3");
            await OstyCmd.Summon(
                new ThrowingPlayerChoiceContext(),
                Owner,
                DynamicVars.Summon.BaseValue,
                this
            );
        }
    }
}
