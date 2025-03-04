using LuniLib.SingletonClassBase;
using Player;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [field:SerializeField] public PlayerController PlayerController { get; private set; }
        
    protected override void InternalAwake()
    {
    }
}