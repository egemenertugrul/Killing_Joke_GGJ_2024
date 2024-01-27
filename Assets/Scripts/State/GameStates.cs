using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KillingJoke.Core
{

    public abstract class GameStates : MonoBehaviour
    {
        protected StateMachine _sm;
        public GameStates(StateMachine stateMachine)
        {
            _sm = stateMachine;
        }
        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void CheckSwitchStates();
        public virtual void SwitchState(GameStates newState)
        {
            newState.EnterState();
            _sm.CurrentState = newState;
        }

    }
}
