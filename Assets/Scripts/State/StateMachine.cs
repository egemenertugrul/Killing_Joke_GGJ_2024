using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using System;
namespace KillingJoke.Core
{

    public class StateMachine : MonoBehaviour
    {
        private GameStates _currentState;
        private GameManager _gameManager;
        [SerializeField] SelectorUnityEventWrapper thumbsUpLeft, thumbsUpRight;
        [SerializeField] SelectorUnityEventWrapper thumbsDownLeft, thumbsDownRight;
        [SerializeField] SelectorUnityEventWrapper palmUpLeft, palmUpRight;
        List<SelectorUnityEventWrapper> thumbsUps, thumbsDowns, palmUps;
        public List<SelectorUnityEventWrapper> ThumbsUps
        {
            get => thumbsUps;
        }
        public List<SelectorUnityEventWrapper> ThumbsDowns
        {
            get => thumbsDowns;
        }
        public List<SelectorUnityEventWrapper> PalmUps
        {
            get => palmUps;
        }
        public GameStates CurrentState
        {
            get => _currentState;
            set => _currentState = value;
        }
        public SelectorUnityEventWrapper PalmUpRight { get => palmUpRight; set => palmUpRight = value; }
        public SelectorUnityEventWrapper PalmUpLeft { get => palmUpLeft; set => palmUpLeft = value; }

        public void Init(GameManager gameManager)
        {
            _gameManager = gameManager;
            _currentState = new IdleState(this);

            thumbsUps = new List<SelectorUnityEventWrapper> { thumbsUpLeft, thumbsUpRight };
            thumbsDowns = new List<SelectorUnityEventWrapper> { thumbsDownLeft, thumbsDownRight };
            palmUps = new List<SelectorUnityEventWrapper> { palmUpLeft, palmUpRight };
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

        public void StartTell()
        {
            _gameManager.StartListenPlayer();
        }

        public void StopTell()
        {
            _gameManager.StopListenPlayer((voiceInput) =>
            {
                //_gameManager.CurrentSession.TellAndListenJoker(_gameManager.ActiveJoker, voiceInput);
                if(voiceInput.Length > 0)
                    _gameManager.CurrentSession.SetReplies(voiceInput);
            });
        }
        public void StopTellDefault()
        {
            _gameManager.CurrentSession.SetReplies("Tell me a joke");
        }

        internal void StartListen()
        {
            _gameManager.CurrentSession.ListenJoker(_gameManager.ActiveJoker);
        }
    }
}