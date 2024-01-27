using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
namespace KillingJoke.Core
{

    public class StateMachine : MonoBehaviour
    {
        private GameStates _currentState;
        private GameManager _gameManager;
        [SerializeField] SelectorUnityEventWrapper thumbsUpLeft, thumbsUpRight;
        [SerializeField] SelectorUnityEventWrapper thumbsDownLeft, thumbsDownRight;
        List<SelectorUnityEventWrapper> thumbsUps, thumbsDowns;
        public List<SelectorUnityEventWrapper> ThumbsUps
        {
            get => thumbsUps;
        }
        public List<SelectorUnityEventWrapper> ThumbsDowns
        {
            get => thumbsDowns;
        }
        public GameStates CurrentState
        {
            get => _currentState;
            set => _currentState = value;
        }
        public void Init(GameManager gameManager)
        {
            _currentState = new IdleState(this);
            thumbsUps = new List<SelectorUnityEventWrapper> { thumbsUpLeft, thumbsUpRight };
            thumbsDowns = new List<SelectorUnityEventWrapper> { thumbsDownLeft, thumbsDownRight };
        }

        private void Update()
        {
            _currentState.UpdateState();
        }

        public void Forgive()
        {
            _gameManager.CurrentSession.ForgiveJoker(_gameManager.ActiveJoker);
        }
        public void Kill()
        {
            _gameManager.CurrentSession.KillJoker(_gameManager.ActiveJoker);
        }
    }
}