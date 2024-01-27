﻿using Meta.WitAi;
using Meta.WitAi.TTS.Data;
using Meta.WitAi.TTS.Integrations;
using Meta.WitAi.TTS.Utilities;
using OpenAI;
using TMPro;
using UnityEngine;

namespace KillingJoke.Core
{
    public partial class Joker : BaseEntity<Joker.Attributes>, IHMDHighlightable
    {

        private Outline outline;
        private bool isAlive = true;
        private TTSWitVoiceSettings voiceSetting;
        private Attributes attributes;
        public bool IsAlive { get => isAlive; }

        private string _speakPhrase = "";
        [SerializeField] private TextMeshProUGUI subtitleText;

        private void Awake()
        {
            outline = gameObject.AddComponent<Outline>();
            outline.OutlineWidth = 0;
        }

        private void ResetHighlight()
        {
            outline.OutlineColor = Color.white;
        }

        public void AssignNewSpeakPhrase(string joke)
        {
            _speakPhrase = joke;
            ResetHighlight();
            HideSubtitle();
        }

        public override void Initialize(Attributes attributes)
        {
            //this.tts = tts;
            this.attributes = attributes;
            SelectRandomVoiceSetting();
        }

        private void SelectRandomVoiceSetting()
        {
            int randomIndex = Random.Range(0, TTSManager.Instance.Presets.Length);
            voiceSetting = TTSManager.Instance.Presets[randomIndex];
        }

        public void Forgive()
        {
            if (!isAlive)
                return;
            Debug.Log($"Forgiven Joker {attributes.ID}");
            outline.OutlineColor = Color.green;
        }

        public void Kill()
        {
            Debug.Log($"Killed Joker {attributes.ID}");
            isAlive = false;
            HideSubtitle();

            if (TTSManager.Instance.LastSpokeEntityID == attributes.ID)
                TTSManager.Instance.StopSpeaking();

            outline.OutlineColor = Color.red;

            // TODO: Dummy
            //Destroy(GetComponentInChildren<MeshRenderer>());
        }

        public void Speak()
        {
            if (!isAlive)
            {
                Debug.Log($"Joker {attributes.ID} is dead. What a joke.");
                return;
            }

            Debug.Log($"Joker {attributes.ID} is speaking: {_speakPhrase}");
            subtitleText.text = _speakPhrase;
            Invoke("HideSubtitle", 10f); // stinky code
            TTSManager.Instance.Speak(voiceSetting, _speakPhrase, attributes.ID, transform);
        }

        private void HideSubtitle()
        {
            subtitleText.text = "";
        }

        public void Highlight()
        {
            this.outline.OutlineWidth = 4;

        }
        public void Unhighlight()
        {
            this.outline.OutlineWidth = 0;
        }
    }
}