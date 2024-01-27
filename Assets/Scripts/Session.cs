using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace KillingJoke.Core
{
    public class Session : MonoBehaviour
    {
        public UnityEvent<List<Joker>> OnEnd;
        private int jokersCount;
        private List<Joker> jokers;
        private List<Joker> forgivenJokers, killedJokers;

        private void Initialize()
        {
            OnEnd = new UnityEvent<List<Joker>>();
            forgivenJokers = new List<Joker>();
            killedJokers = new List<Joker>();
        }

        /// <summary>
        /// Generate a session from scratch.
        /// </summary>
        /// <param name="jokerCount"></param>
        public void Initialize(JokerFactory jokerFactory, int jokerCount) {
            Initialize();

            jokers = new List<Joker>();
            float width = 1;
            for (int i = 0; i < jokerCount; i++)
            {
                Joker joker = jokerFactory.Generate(attributes: Joker.Attributes.GetRandomAttributes());
                jokers.Add(joker);

                joker.transform.localPosition = new Vector3(i * width - (jokerCount / 2) * width, 0, 5);
            }
            this.jokersCount = jokerCount;

            Begin();
        }

        /// <summary>
        /// Carry on from previous session.
        /// </summary>
        /// <param name="jokers">Preferably forgivenJokers.</param>
        public void Initialize(List<Joker> jokers) {
            Initialize();

            if (jokers == null)
                throw new System.Exception("List of jokers is null!");

            this.jokers = jokers;
            this.jokersCount = jokers.Count;
            
            Begin();
        }

        public void Begin()
        {
            for (int i = 0; i < jokers.Count; i++)
            {
                var joker = jokers[i];
                joker.AssignNewJoke("Hi, this is a joke!");
            }
        }

        private void CheckEndState()
        {
            bool endSessionCondition = forgivenJokers.Count + killedJokers.Count == jokersCount;
            if (endSessionCondition)
                End();
        }

        public void ForgiveJoker(Joker joker)
        {
            if (joker == null)
                return;
            if (forgivenJokers.Contains(joker) || killedJokers.Contains(joker))
                return;
            jokers.Remove(joker);
            forgivenJokers.Add(joker);
            joker.Forgive();

            CheckEndState();
        }

        public void KillJoker(Joker joker)
        {
            if (joker == null)
                return;
            if (forgivenJokers.Contains(joker) || killedJokers.Contains(joker))
                return;
            jokers.Remove(joker);
            killedJokers.Add(joker);
            joker.Kill();

            CheckEndState();
        }

        public void ListenJoker(Joker joker)
        {
            if (joker == null)
                return;
            joker.Speak();
        }

        public void End()
        {
            OnEnd.Invoke(forgivenJokers);
        }

        private void DestroySelf(float delay = 0)
        {
            if(delay > 0) {
                Destroy(gameObject, delay);
            } else
            {
                Destroy(gameObject);
            } 

        }
    }
}