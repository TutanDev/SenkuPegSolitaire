using System;
using UnityEngine;

[Serializable]
public struct GameData
{
    [SerializeField] public CommandData[] Commands;
}

[Serializable]
public struct CommandData 
{
    [SerializeField] public CommandType type;
    [SerializeField] public int[] data;
}

[System.Serializable]
public enum CommandType
{
    Start = 0,
    Move = 1,
}
