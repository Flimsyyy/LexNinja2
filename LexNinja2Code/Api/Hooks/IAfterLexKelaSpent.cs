using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace LexNinja2.LexNinja2Code.Api.Hooks;

public interface IAfterLexKelaSpent
{
    Task AfterLexKelaSpent(PlayerChoiceContext choiceContext, int amount, Player spender);
}
