using UnityEngine;
/// <summary>
/// Abstract class for commands.
/// </summary>
public abstract class Command
{
    /// <summary>
    /// Method called to execute command.
    /// </summary>
    public abstract void Execute();
    /// <summary>
    /// Method called to undo command.
    /// </summary>
    public abstract void Undo();
}
