using Unity.AI.Navigation;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player Player;
    public NavMeshSurface NavMeshSurface;

    public IsometricCameraController Camera;
}
