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
            }
            var newState = new IdleState(_sm);
            SwitchState(newState);
        }

        private void OnExecute()
        {
            _sm.ThumbsUps.ForEach(u =>
            {
                u.WhenSelected.RemoveListener(_sm.Forgive);
                u.WhenSelected.RemoveListener(OnExecute);
            });
            _sm.ThumbsDowns.ForEach(u =>
            {
                u.WhenSelected.RemoveListener(_sm.Kill);
                u.WhenSelected.RemoveListener(OnExecute);
            });
            var newState = new IdleState(_sm);
            SwitchState(newState);
        }
        public override void EnterState()
        {
            _sm.ThumbsUps.ForEach(u =>
            {
                u.WhenSelected.AddListener(_sm.Forgive);
                u.WhenSelected.AddListener(OnExecute);
            });
            _sm.ThumbsDowns.ForEach(u =>
            {
                u.WhenSelected.AddListener(_sm.Kill);
                u.WhenSelected.AddListener(OnExecute);
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
