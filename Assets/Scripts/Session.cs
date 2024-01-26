using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace KillingJoke.Core
{
    public class Session : MonoBehaviour
    {
        public UnityEvent<List<Joker>> OnEnd = new UnityEvent<List<Joker>>();
        private int jokersCount;
        private List<Joker> jokers;
        private List<Joker> forgivenJokers = new List<Joker>(), killedJokers = new List<Joker>();

        /// <summary>
        /// Generate a session from scratch.
        /// </summary>
        /// <param name="jokerCount"></param>
        public void Initialize(JokerFactory jokerFactory, int jokerCount) {
            jokers = new List<Joker>();
            float width = 1;
            for (int i = 0; i < jokerCount; i++)
            {
                Joker joker = jokerFactory.Generate(attributes: Joker.Attributes.GetRandomAttributes());
                jokers.Add(joker);

                joker.transform.localPosition = new Vector3(i * width - (jokerCount / 2) * width, 0, 5);
            }
            this.jokersCount = jokerCount;
        }

        /// <summary>
        /// Carry on from previous session.
        /// </summary>
        /// <param name="jokers">Preferably forgivenJokers.</param>
        public void Initialize(List<Joker> jokers) {
            if (jokers == null)
                throw new System.Exception("List of jokers is null!");

            this.jokers = jokers;
            this.jokersCount = jokers.Count;
        }

        public void Begin()
        {
            for (int i = 0; i < jokers.Count; i++)
            {
                //currentJokers.Enter(new Vector3());
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
            if (forgivenJokers.Contains(joker) || killedJokers.Contains(joker))
                return;
            jokers.Remove(joker);
            forgivenJokers.Add(joker);
            joker.Forgive();

            CheckEndState();
        }

        public void KillJoker(Joker joker)
        {
            if (forgivenJokers.Contains(joker) || killedJokers.Contains(joker))
                return;
            jokers.Remove(joker);
            killedJokers.Add(joker);
            joker.Kill();

            CheckEndState();
        }

        public void End()
        {
            OnEnd.Invoke(forgivenJokers);
        }

        public List<Joker> GetJokers()
        {
            return jokers;
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