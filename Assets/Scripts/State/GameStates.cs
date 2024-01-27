using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        protected virtual void ExitState() { }
        public abstract void UpdateState();
        public abstract void CheckSwitchStates();
        public virtual void SwitchState(GameStates newState)
        {
            ExitState();
            newState.EnterState();
            _sm.CurrentState = newState;
        }

    }
}
