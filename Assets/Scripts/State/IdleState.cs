using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KillingJoke.Core
{
    public class IdleState : GameStates
    {
        public override void CheckSwitchStates()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                var newState = new SpeakState(_sm);
                SwitchState(newState);
            }
        }

        public override void EnterState()
        {
            Debug.Log("Entered Idle state.");
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
        }
        public IdleState(StateMachine stateMachine) : base(stateMachine)
        {

        }

    }
}
