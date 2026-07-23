using MegaCrit.Sts2.Core.DevConsole;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Entities.Players;

namespace LexNinja2.LexNinja2Code.Api.Commands;

public class LexKelaConsoleCmd : AbstractConsoleCmd
{
    public override string CmdName => "lexkela";
    public override string Args => "<amount:int>";
    public override string Description => "Adds or Loses lex kela to player";
    public override bool IsNetworked => true;

    public override CmdResult Process(Player? issuingPlayer, string[] args)
    {
        if (args.Length == 0 || !int.TryParse(args[0], out var result))
            return new CmdResult(false, "The first argument must be an int.");
        if (issuingPlayer == null)
            return new CmdResult(false, "This command only works during a run.");
        if (issuingPlayer.PlayerCombatState == null)
            return new CmdResult(false, "This command only works in combat.");
        if (result < 0)
            return new CmdResult(
                LexKela.Lose(issuingPlayer, -result),
                true,
                $"Lost '{-result}' lex kela."
            );
        return new CmdResult(
            LexKela.Gain(issuingPlayer, result),
            true,
            $"Added '{result}' lex kela."
        );
    }
}
