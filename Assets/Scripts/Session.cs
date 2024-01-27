using OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
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
        public void Initialize(JokerFactory jokerFactory, int jokerCount)
        {
            Initialize();

            jokers = new List<Joker>();
            float width = 1.5f;
            for (int i = 0; i < jokerCount; i++)
            {
                Joker joker = jokerFactory.Generate(attributes: Joker.Attributes.GetRandomAttributes());
                jokers.Add(joker);

                var zOffset = 1 + 2f * Mathf.Sin((Mathf.PI / (jokerCount - 1)) * i);
                joker.transform.localPosition = new Vector3(i * width - (jokerCount / 2) * width, 0.56f, -zOffset);

                joker.SetMesh(jokerFactory.JokersPool[UnityEngine.Random.Range(0, jokerFactory.JokersPool.Length)]);
            }
            this.jokersCount = jokerCount;

            //SetReplies();
        }

        /// <summary>
        /// Carry on from previous session.
        /// </summary>
        /// <param name="jokers">Preferably forgivenJokers.</param>
        public void Initialize(List<Joker> jokers)
        {
            Initialize();

            if (jokers == null)
                throw new System.Exception("List of jokers is null!");

            this.jokers = jokers;
            this.jokersCount = jokers.Count;

            //SetReplies();
        }

        public void SetReplies(string voiceInput = "")
        {
            ChatGPT.Instance.GetReply(voiceInput, (replies) =>
            {
                var formattedReplies = replies.Split(";").Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                Debug.Log(replies);

                try
                {
                    for (int i = 0; i < jokers.Count; i++)
                    {
                        var joker = jokers[i];
                        joker.AssignNewSpeakPhrase(formattedReplies[i]);
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    Debug.LogError(e);
                }
            }, jokers.Count);
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
            if (delay > 0)
            {
                Destroy(gameObject, delay);
            }
            else
            {
                Destroy(gameObject);
            }

        }
    }
}