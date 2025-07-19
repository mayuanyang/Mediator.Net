using Mediator.Net.Contracts;

namespace Mediator.Net.WebApiSample.Handlers.CommandHandler;

public class CalculateCommand: ICommand {
    public int Left { get; }
    public int Right { get; }

    public CalculateCommand(int left, int right)
    {
        Left = left;
        Right = right;
    }
}