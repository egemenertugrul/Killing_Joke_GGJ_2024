/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Meta.WitAi.Dictation;
using Meta.WitAi.Events;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Meta.Voice.Samples.Dictation
{
    public class DictationActivation : MonoBehaviour
    {
        [FormerlySerializedAs("dictation")]
        [SerializeField] private DictationService _dictation;
        private string _transcribedText;

        public UnityEvent<string> OnDictationStopped = new UnityEvent<string>();

        public bool IsActive
        {
            get
            {
                return _dictation.MicActive;
            }
            set
            {
                if (!value)
                {
                    //OnDictationStopped.Invoke(TranscribedText);
                    //TranscribedText = "";
                    _dictation.Deactivate();
                }
                else
                {
                    TranscribedText = "";
                    _dictation.Activate();
                }
            }
        }

        public string TranscribedText { get => _transcribedText.Split('\n').Last(); set => _transcribedText = value; }

        public WitTranscriptionEvent OnPartialTranscription = new WitTranscriptionEvent();
        public WitTranscriptionEvent OnFullTranscription = new WitTranscriptionEvent();

        private void Awake()
        {
            _dictation.DictationEvents.OnFullTranscription.AddListener((transcription)=> {
                OnFullTranscription.Invoke(transcription);
            });
            _dictation.DictationEvents.OnPartialTranscription.AddListener((transcription) => {
                OnPartialTranscription.Invoke(transcription);
            });

            _dictation.DictationEvents.OnAborting.AddListener(() =>
            {
                OnDictationStopped.Invoke(TranscribedText);
            });
        }
    }
}
