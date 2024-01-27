using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KillingJoke.Core
{
    public class SpeakState : GameStates
    {
        public override void CheckSwitchStates()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                //var newState = new ListenState(_sm);
                var newState = new ExecuteState(_sm);
                SwitchState(newState);
            }
        }

        public override void EnterState()
        {
            Debug.Log("Entered Speak state.");
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
        }
        public SpeakState(StateMachine stateMachine) : base(stateMachine)
        {

        }
    }
}