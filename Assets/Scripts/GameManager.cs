using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Meta.Voice.Samples.Dictation;

namespace KillingJoke.Core
{

    public class GameManager : MonoBehaviour
    {
        enum GameState
        {
            None,
            Menu,
            Speak,
            Listen,
            Execute
        }

        
        [SerializeField] HmdRaycaster hmdRaycaster;
        [SerializeField] DictationActivation _dictation_activation;
            
        [SerializeField] private JokerFactory jokerFactory;
        [Range(1, 10)][SerializeField] private int jokerCount;

        GameState state = GameState.Execute;
        private StateMachine _stateMachine;
        private Joker _activeJoker;

        private Session _currentSession;
        public Session CurrentSession
        {
            get => _currentSession;
        }
        public Joker ActiveJoker
        {
            get => _activeJoker;
        }
        void Start()
        {

            _stateMachine.Init(this);
            hmdRaycaster.OnNewHighlight.AddListener(SetActiveJoker);

            _currentSession = StartNewSession();
            
        }
        private void SetActiveJoker(IHMDHighlightable highlighted)
        {
            Joker highlightedJoker = (Joker)highlighted;
            if (highlightedJoker.IsAlive)
                _activeJoker = highlightedJoker;
        }

        private Session StartNewSession(List<Joker> jokers = null)
        {
            if (_currentSession)
            {
                jokers = _currentSession.GetJokers();
                _currentSession.End();
            }

            Session newSession = new GameObject("Session").AddComponent<Session>();
            newSession.transform.SetParent(transform);
            if (jokers == null)
                newSession.Initialize(jokerFactory, jokerCount);
            else
                newSession.Initialize(jokers);

            newSession.OnEnd.AddListener((aliveJokers) =>
            {
                if (aliveJokers.Count == 0)
                {
                    EndGame();
                }
                else
                {
                    StartNewSession(jokers);
                }
            });

            return newSession;
        }

        private void EndGame()
        {
            // End the fookin game
            throw new NotImplementedException();
        }

        private void OnDestroy()
        {

            hmdRaycaster.OnNewHighlight.RemoveListener(SetActiveJoker);
        }
    }
}