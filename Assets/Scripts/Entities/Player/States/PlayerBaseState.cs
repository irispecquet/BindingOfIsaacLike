namespace Entities.Player.States
{
    public abstract class PlayerBaseState
    {
        protected PlayerController _playerController;

        public PlayerBaseState(PlayerController playerController)
        {
            _playerController = playerController;
        }

        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void FixedUpdateState();
        public abstract void ExitState();
    }
}