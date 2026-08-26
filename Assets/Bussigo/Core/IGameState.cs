namespace Bussigo.Core
{
    /// <summary>
    /// Contract for all modular game states in the lifecycle state machine.
    /// </summary>
    public interface IGameState
    {
        GamePhase Phase { get; }
        void OnEnter();
        void OnUpdate(float deltaTime);
        void OnExit();
    }
}
