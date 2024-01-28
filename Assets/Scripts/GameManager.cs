using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Meta.Voice.Samples.Dictation;
using Meta.WitAi.Dictation;

namespace KillingJoke.Core
{

    public class GameManager : MonoBehaviour
    {
        [SerializeField] HmdRaycaster hmdRaycaster;
        [SerializeField] DictationActivation _dictation_activation;
        [SerializeField] MultiRequestTranscription _dictation_multirequest_transcription;
        [SerializeField] AudioSource _ambientMusic;
        [SerializeField] private JokerFactory jokerFactory;
        [Range(1, 10)][SerializeField] private int jokerCount;

        [SerializeField] private StateMachine _stateMachine;
        private Joker _activeJoker;

        private Session _currentSession;
        public bool isListening;

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
            _ambientMusic.Play();
            _stateMachine.Init(this);
            hmdRaycaster.OnNewHighlight.AddListener(SetActiveJoker);

            StartCoroutine(StartSessionDelayed());
            StartCoroutine(GoToExecuteState());
        }

        IEnumerator GoToExecuteState()
        {
            yield return new WaitForSeconds(2);
            _stateMachine.CurrentState.SwitchState(new ExecuteState(_stateMachine));
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
                    
                    StartCoroutine(GoToExecuteState());
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

        public void StartListenPlayer()
        {
            isListening = true;
            //_dictation_activation.OnFullTranscription.RemoveAllListeners();

            Debug.Log("Started listening to player..");
            _dictation_activation.IsActive = true;
        }

        public void StopListenPlayer(UnityAction<string> voiceInputCallback)
        {
            isListening = false;
            Debug.Log($"Stopped listening to player.. Waiting for result..");
            voiceInputCallback.Invoke(_dictation_activation.TranscribedText);
            //_dictation_activation.OnFullTranscription.AddListener((UnityAction<string>)voiceInputCallback.Clone());
            //_dictation_activation.OnDictationStopped.AddListener((UnityAction<string>)voiceInputCallback.Clone());
            _dictation_activation.IsActive = false;
            //voiceInputCallback?.Invoke(_dictation_activation.TranscribedText);
            //_dictation_activation.OnDictationStopped.RemoveListener(voiceInputCallback);
        }
    }
}