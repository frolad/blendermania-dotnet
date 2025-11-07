public interface IBMCommand
{
    public const string COMMAND_NAME = "base-command";

    public static Task Execute(string payload) => Task.CompletedTask;

}