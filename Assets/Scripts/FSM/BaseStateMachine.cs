using System;
using System.Collections.Generic;
using LearnGame.Exceptions;
using UnityEngine;

namespace LearnGame.FSM
{
    public class BaseStateMachine
    {
        private BaseState _currentState;
        private List<BaseState> _states;
        private Dictionary<BaseState, List<Transition>> _transitions;
        public float _hp { get; private set; }

        public float _maxhp { get; private set; }

        public BaseStateMachine()
        {
            _states = new List<BaseState>();
            _transitions = new Dictionary<BaseState, List<Transition>>();
        }

        public void SetInitialState(BaseState state)
        {
            _currentState = state;
            _currentState.StateID = "idle";
        }

        public void AddState(BaseState state, List<Transition> transitions)
        {
            if(!_states.Contains(state))
            {
                _states.Add(state);
                _transitions.Add(state, transitions);
            } else
            {
                throw new AlreadyExistsException($"State {state.GetType()} already exists in state machine!");
            }
        }

        public void Update(float HP, float maxHP)
        {
            _hp = HP;
            _maxhp = maxHP;
            foreach (var transition in _transitions[_currentState])
            {
                if (transition.Condition())
                {
                    _currentState = transition.ToState;
                    break;
                }
            }

            _currentState.Execute();
        }

        public string returnCurrentStateID()
        {
            return _currentState.GetID();
        }
    }
}
