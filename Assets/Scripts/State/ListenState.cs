using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KillingJoke.Core
{
    public class ListenState : GameStates
    {

        public override void CheckSwitchStates()
        {
            var newState = new ExecuteState(_sm);
            SwitchState(newState);
        }

        public override void EnterState()
        {
            Debug.Log("Entered Listen state.");
        }

        public override void UpdateState()
        {
            return;
        }
        public ListenState(StateMachine stateMachine) : base(stateMachine)
        {

        }
    }
}
