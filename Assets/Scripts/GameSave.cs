using System;
using UnityEngine;

public class GameSave
{
    public PlayerInfo PlayerInfo = new PlayerInfo();
    public GameOptions GameOptions = new GameOptions();
}

[Serializable]
public class GameOptions
{
    public int GameMusic, GameSound;
    public string PlayerName;
}
[Serializable]
public class PlayerInfo
{
    public string PlayerName;
    public MoneyAccount MoneyAccount = new MoneyAccount();
    public SavableVector3 PlayerPosition = new SavableVector3(Vector3.zero);
}
[Serializable]
public class SavableVector3
{
    public float x;
    public float y;
    public float z;
    public SavableVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }
    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}