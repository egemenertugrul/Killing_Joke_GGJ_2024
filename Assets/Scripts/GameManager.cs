using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

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

        [SerializeField] SelectorUnityEventWrapper thumbsUpLeft, thumbsUpRight;
        [SerializeField] SelectorUnityEventWrapper thumbsDownLeft, thumbsDownRight;
        [SerializeField] HmdRaycaster hmdRaycaster;

        [SerializeField] private JokerFactory jokerFactory;
        [Range(1, 10)][SerializeField] private int jokerCount;

        List<SelectorUnityEventWrapper> thumbsUps, thumbsDowns;
        GameState state = GameState.Execute;
        private Joker activeJoker;

        private Session currentSession;

        void Start()
        {
            thumbsUps = new List<SelectorUnityEventWrapper> { thumbsUpLeft, thumbsUpRight };
            thumbsDowns = new List<SelectorUnityEventWrapper> { thumbsDownLeft, thumbsDownRight };

            thumbsUps.ForEach(u => u.WhenSelected.AddListener(OnThumbsUpAction));
            thumbsDowns.ForEach(u => u.WhenSelected.AddListener(OnThumbsDownAction));

            hmdRaycaster.OnNewHighlight.AddListener(SetActiveJoker);

            currentSession = StartNewSession();
        }

        private void SetActiveJoker(IHMDHighlightable highlighted)
        {
            Joker highlightedJoker = (Joker)highlighted;
            if (highlightedJoker.IsAlive)
                activeJoker = highlightedJoker;
        }

        private Session StartNewSession(List<Joker> jokers = null)
        {
            if (currentSession)
            {
                jokers = currentSession.GetJokers();
                currentSession.End();
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

        private void OnThumbsUpAction()
        {
            if (state != GameState.Execute)
                return;
            currentSession.ForgiveJoker(activeJoker);
            //state = GameState.None;
        }

        private void OnThumbsDownAction()
        {
            if (state != GameState.Execute)
                return;

            currentSession.KillJoker(activeJoker);
            //state = GameState.None;
        }

        void Update()
        {

        }

        private void OnDestroy()
        {
            thumbsUps.ForEach(u => u.WhenSelected.RemoveListener(OnThumbsUpAction));
            thumbsDowns.ForEach(u => u.WhenSelected.RemoveListener(OnThumbsDownAction));

            hmdRaycaster.OnNewHighlight.RemoveListener(SetActiveJoker);
        }
    }
}