using KillingJoke.Core;
using KillingJoke.Core.Utils;
using Meta.WitAi.TTS.Integrations;
using Meta.WitAi.TTS.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTSManager : Singleton<TTSManager>
{
    [SerializeField] private TTSWit wit;
    private TTSWitVoiceSettings[] presets;
    [SerializeField] private TTSSpeaker ttsSpeaker;
    [SerializeField] private AudioSource audioSource;

    private int lastSpokeEntityID = -1;

    public TTSWitVoiceSettings[] Presets { get => presets; }
    public int LastSpokeEntityID { get => lastSpokeEntityID; }

    // Start is called before the first frame update
    void Start()
    {
        presets = wit.PresetWitVoiceSettings;
    }

    public void Speak(TTSWitVoiceSettings voiceSetting, string phrase, int id, Transform transform = null)
    {
        lastSpokeEntityID = id;
        ttsSpeaker.SetVoiceOverride(voiceSetting);
        if (transform)
            audioSource.transform.position = transform.position;

        StopSpeaking();
        ttsSpeaker.Speak(phrase);
    }

    public void StopSpeaking()
    {
        ttsSpeaker.StopSpeaking();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
