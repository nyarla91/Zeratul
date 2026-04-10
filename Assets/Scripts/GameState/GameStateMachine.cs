using System;
using System.Collections.Generic;
using System.Linq;
using GameState.States;
using UnityEngine;

namespace GameState
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IGameState> _states;

        public IGameState CurrentState { get; private set; }

        public GameStateMachine(List<IGameState> states)
        {
            _states = states.ToDictionary(s => s.GetType());
        }
        
        public TState Enter<TState>() where TState : class, IGameState
        {
            CurrentState?.Exit();
            TState state = GetState<TState>();
            state.Enter();
            Debug.Log($"Game transitioned from {CurrentState} to {state}");
            CurrentState = state;
            return state;
        }

        public TState GetState<TState>() where TState : class, IGameState
        {
            if ( ! _states.TryGetValue(typeof(TState), out IGameState state))
                throw new InvalidOperationException($"No state of type {typeof(TState)} found.");
            return (TState) state;
        }
    }
}