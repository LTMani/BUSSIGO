using System;
using System.Collections.Generic;

namespace Bussigo.Game.Core
{
    public interface IState
    {
        void OnEnter();
        void OnUpdate(float deltaTime);
        void OnExit();
    }

    public class StateMachine
    {
        private readonly Dictionary<Type, IState> _states = new Dictionary<Type, IState>();
        private IState _currentState;
        public IState CurrentState => _currentState;
        public Type CurrentStateType => _currentState?.GetType();

        public event Action<IState, IState> OnStateChanged;

        public void RegisterState<T>(T state) where T : IState
        {
            _states[typeof(T)] = state;
        }

        public void ChangeState<T>() where T : IState
        {
            Type stateType = typeof(T);
            if (!_states.TryGetValue(stateType, out IState newState))
            {
                throw new KeyNotFoundException($"State {stateType.Name} not registered in StateMachine.");
            }

            IState previousState = _currentState;
            _currentState?.OnExit();
            _currentState = newState;
            _currentState.OnEnter();
            OnStateChanged?.Invoke(previousState, _currentState);
        }

        public void Update(float deltaTime)
        {
            _currentState?.OnUpdate(deltaTime);
        }
    }
}
