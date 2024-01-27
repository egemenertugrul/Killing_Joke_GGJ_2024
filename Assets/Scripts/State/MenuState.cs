using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KillingJoke.Core
{
    public class MenuState : GameStates
    {
        public override void CheckSwitchStates()
        {
            
        }

        public override void EnterState()
        {
            Debug.Log("Entered Menu state.");
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
            //throw new System.NotImplementedException();
        }
        public MenuState(StateMachine stateMachine) : base(stateMachine)
        {

        }
    }
}
