using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards;

public class EclipseMistBlade()
    : LexNinja2Card(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new BlockVar(8, ValueProp.Move), new LexKelaVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Blade];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/EclipseMistBlade.mp3");
        await CommonActions.CardBlock(this, play);
        foreach (var card in GetCards().ToList())
        {
            await CardCmd.Exhaust(choiceContext, card);
            await NinjaHelper.AddLexKela(choiceContext, card);
        }
    }

    public override string CustomPortraitPath => $"EclipseMistBlade_p.png".BigCardImagePath();
    public override string PortraitPath => $"EclipseMistBlade.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/EclipseMistBlade.png".CardImagePath();

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(4);
    }

    private IEnumerable<CardModel> GetCards()
    {
        return PileType
            .Hand.GetPile(Owner)
            .Cards.Where(
                c => !c.Keywords.Contains(NinjaKeyword.Blade) && !c.Tags.Contains(CardTag.Shiv)
            );
    }
}
