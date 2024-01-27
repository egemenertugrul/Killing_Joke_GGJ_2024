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
        [SerializeField] HmdRaycaster hmdRaycaster;
        [SerializeField] DictationActivation _dictation_activation;
            
        [SerializeField] private JokerFactory jokerFactory;
        [Range(1, 10)][SerializeField] private int jokerCount;

        [SerializeField] private StateMachine _stateMachine;
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

            StartCoroutine(StartSessionDelayed());
            
        }

        IEnumerator StartSessionDelayed()
        {
            yield return new WaitForSeconds(1);

            _currentSession = StartNewSession();
        }

        private void SetActiveJoker(IHMDHighlightable highlighted)
        {
            Joker highlightedJoker = (Joker)highlighted;
            if(highlightedJoker != null )
            {
                if (highlightedJoker.IsAlive)
                    _activeJoker = highlightedJoker;
            } else
            {
                _activeJoker = null;
            }
        }

        private Session StartNewSession(List<Joker> jokers = null)
        {
            //Session prevSession = null;
            //if (_currentSession)
            //{
            //    Destroy(_currentSession.gameObject);
            //}

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
                    Debug.Log($"All jokers are dead, end the game.");
                    EndGame();
                }
                else
                {
                    Debug.Log($"End of session with {aliveJokers.Count} jokers alive.");
                    _currentSession = StartNewSession(aliveJokers);
                    _stateMachine.CurrentState.SwitchState(new IdleState(_stateMachine)); // Go to idle
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