using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KillingJoke.Core
{
    public class ExecuteState : GameStates
    {
        public override void CheckSwitchStates()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _sm.Kill();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                _sm.Forgive();
            } else if(Input.GetMouseButtonDown(2))
            {
                _sm.StartTell();
            }
            //var newState = new IdleState(_sm);
            //SwitchState(newState);
        }

        protected override void ExitState()
        {
            _sm.ThumbsUps.ForEach(u =>
            {
                u.WhenSelected.RemoveListener(_sm.Forgive);
            });
            _sm.ThumbsDowns.ForEach(u =>
            {
                u.WhenSelected.RemoveListener(_sm.Kill);
            });
            _sm.PalmUps.ForEach(u => {
                u.WhenSelected.RemoveListener(_sm.StartTell);
                u.WhenUnselected.RemoveListener(_sm.StopTell);
            });
        }

        public override void EnterState()
        {
            _sm.ThumbsUps.ForEach(u =>
            {
                u.WhenSelected.AddListener(_sm.Forgive);
            });
            _sm.ThumbsDowns.ForEach(u =>
            {
                u.WhenSelected.AddListener(_sm.Kill);
            });
            _sm.PalmUps.ForEach(u => { 
                u.WhenSelected.AddListener(_sm.StartTell);
                u.WhenUnselected.AddListener(_sm.StopTell);
            });
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
        }
        public ExecuteState(StateMachine stateMachine) : base(stateMachine)
        {

        }
    }
}
