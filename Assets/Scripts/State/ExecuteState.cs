using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KillingJoke.Core
{
    public class ExecuteState : GameStates
    {
        private float _time_to_forgive = 1f;
        private Coroutine _coroutineForgive;
        private Coroutine _coroutineKill;
        private float _time_to_kill = 1f;
        private bool _canPlay = false;

        //public override void CheckSwitchStates()
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        _sm.Kill();
        //    }
        //    else if (Input.GetMouseButtonDown(1))
        //    {
        //        _sm.Forgive();
        //    }
        //    else if (Input.GetMouseButtonDown(2))
        //    {
        //        _sm.StartListen();
        //    }
        //    else if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        _sm.StopTellDefault();
        //    }

        //}
        public override void CheckSwitchStates()
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartKill();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                StartForgive();
            }
            else if (Input.GetMouseButtonDown(2))
            {
                _sm.StartListen();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                _sm.StopTellDefault();
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                StopKillingAction();
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                StopForgivingAction();
            }

        }

        protected override void ExitState()
        {
            _sm.ThumbsUps.ForEach(u =>
            {
                u.WhenSelected.RemoveListener(StartForgive);
                u.WhenUnselected.AddListener(StopForgivingAction);

            });
            _sm.ThumbsDowns.ForEach(u =>
            {
                u.WhenSelected.RemoveListener(StartKill);
                u.WhenSelected.RemoveListener(StopKillingAction);
            });
            //_sm.PalmUps.ForEach(u => {
            //    u.WhenSelected.RemoveListener(_sm.StartTell);
            //    u.WhenUnselected.RemoveListener(_sm.StopTell);
            //});
            _sm.PalmUpLeft.WhenSelected.RemoveListener(_sm.StartTell);
            _sm.PalmUpLeft.WhenUnselected.RemoveListener(_sm.StopTell);

            _sm.PalmUpRight.WhenSelected.RemoveListener(_sm.StartListen);
        }


        public void StartForgive()
        {
            _coroutineForgive = StartCoroutine(ForgiveTimer());
        }
        public void StartKill()
        {
            _coroutineKill = StartCoroutine(KillTimer());
        }
        IEnumerator ForgiveTimer()
        {
            yield return new WaitForSeconds(_time_to_forgive);
            _sm.Forgive();
        }
        IEnumerator KillTimer()
        {
            yield return new WaitForSeconds(_time_to_kill);
            _sm.Kill();

        }
        public void StopForgivingAction()
        {
            if (_coroutineForgive != null)
            {
                StopCoroutine(_coroutineForgive);
            }
        }
        public void StopKillingAction()
        {
            if (_coroutineKill != null)
            {
                StopCoroutine(_coroutineKill);
            }

        }


        public override void EnterState()
        {
            _sm.ThumbsUps.ForEach(u =>
            {
                u.WhenSelected.AddListener(StartForgive);
                u.WhenUnselected.AddListener(StopForgivingAction);
            });
            _sm.ThumbsDowns.ForEach(u =>
            {
                u.WhenSelected.AddListener(StartKill);
                u.WhenUnselected.AddListener(StopKillingAction);

            });
            //_sm.PalmUps.ForEach(u => { 
            //    u.WhenSelected.AddListener(_sm.StartTell);
            //    u.WhenUnselected.AddListener(_sm.StopTell);
            //});
            _sm.PalmUpLeft.WhenSelected.AddListener(_sm.StartTell);
            _sm.PalmUpLeft.WhenUnselected.AddListener(_sm.StopTell);
            _sm.PalmUpRight.WhenSelected.AddListener(_sm.StartListen);
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
