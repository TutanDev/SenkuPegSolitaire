using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Command invoker. Collects command into buffer to execute them at once.
/// </summary>
public class CommandInvoker
{
    // Command history
    protected List<Command> commandHistory = new List<Command>();
    protected int executionIndex = 0;

    /// <summary>
    /// Method used to execute current command.
    /// </summary>
    /// <returns>The command index.</returns>
    public virtual int ExecuteCommand()
    {
        if (executionIndex < commandHistory.Count)
        {
            commandHistory[executionIndex].Execute();
            executionIndex++;
        }

        return executionIndex;
    }

    /// <summary>
    /// Method used to add and execute command.
    /// </summary>
    /// <returns>The command index.</returns>
    /// <param name="command">New command.</param>
    public virtual int ExecuteCommand(Command command)
    {
        for (int i = commandHistory.Count - 1; i >= executionIndex; i--)
        {
            commandHistory.RemoveAt(i);
        }

        commandHistory.Add(command);
        return ExecuteCommand();
    }

    /// <summary>
    /// Method used to undo command.
    /// </summary>
    /// <returns>The command index.</returns>
    public virtual int UndoCommand()
    {
        if (executionIndex > 0)
        {
            executionIndex--;
            commandHistory[executionIndex].Undo();
        }

        return executionIndex;
    }

    /// <summary>
    /// Method used to clear command buffer.
    /// </summary>
    public virtual void ClearCommandHistory()
    {
        commandHistory.Clear();
        executionIndex = 0;
    }

    public GameData GetHistoryData()
    {
        List<CommandData> tmp = new List<CommandData>();
        GameData result = new GameData();

        foreach (Command command in commandHistory)
        {
            CommandData data = new CommandData();

            if (command is StartCommand)
            {
                data.type = CommandType.Start;
                data.data = new int[] { ((StartCommand)command).pieceIndex };
            }
            else if (command is MoveCommand)
            {
                var c = (MoveCommand)command;
                data.type = CommandType.Move;
                data.data = new int[] { c.moveData.Old, c.moveData.Deleted, c.moveData.New };
            }

            tmp.Add(data);
        }

        result.Commands = tmp.ToArray();
        return result;
    }

    public void LoadListFromData(GameData data, Board board)
    {
        executionIndex = 0;
        commandHistory.Clear();

        foreach (CommandData c in data.Commands)
        {
            if(c.type == CommandType.Start)
            {
                Command command = new StartCommand(board, c.data[0]);
                commandHistory.Add(command);
            }
            else if(c.type == CommandType.Move)
            {
                Command command = new MoveCommand(board,
                    new Board.MoveData() {
                    Old = c.data[0],
                    Deleted = c.data[1],
                    New = c.data[2]
                });

                commandHistory.Add(command);
            }
        }
    }
}
