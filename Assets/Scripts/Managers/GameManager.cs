using System;
using Entities.Player;
using LuniLib.SingletonClassBase;
using LuniLib.UnityUtils;
using Rooms;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [field:SerializeField] public PlayerController PlayerController { get; private set; }
        [field:SerializeField] public RoomManager RoomManager { get; private set; }
        [field:SerializeField] public UIManager UIManager { get; private set; }

        [SerializeField] private SceneField _menuScene;
        
        protected override void InternalAwake()
        {
        }

        public void OnPlayerDied()
        {
            UIManager.FadeIn(() => SceneManager.LoadScene(_menuScene));
        }
    }
}