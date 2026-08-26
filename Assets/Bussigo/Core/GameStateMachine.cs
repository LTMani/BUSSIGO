using System;
using System.Collections.Generic;

namespace Bussigo.Core
{
    /// <summary>
    /// Lifecycle state machine managing game phase transitions.
    /// </summary>
    public class GameStateMachine
    {
        private readonly Dictionary<GamePhase, IGameState> states = new Dictionary<GamePhase, IGameState>();
        public IGameState CurrentState { get; private set; }
        public GamePhase CurrentPhase => CurrentState != null ? CurrentState.Phase : GamePhase.MainMenu;

        public event Action<GamePhase, GamePhase> OnStateChanged;

        public void RegisterState(IGameState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            states[state.Phase] = state;
        }

        public void ChangeState(GamePhase newPhase)
        {
            if (!states.TryGetValue(newPhase, out IGameState nextState))
            {
                throw new InvalidOperationException($"[GameStateMachine] State for phase '{newPhase}' is not registered.");
            }

            if (CurrentState != null && CurrentState.Phase == newPhase)
            {
                return; // Already in target state
            }

            GamePhase previousPhase = CurrentPhase;

            CurrentState?.OnExit();
            CurrentState = nextState;
            CurrentState.OnEnter();

            OnStateChanged?.Invoke(previousPhase, newPhase);
        }

        public void Update(float deltaTime)
        {
            CurrentState?.OnUpdate(deltaTime);
        }
    }
}
