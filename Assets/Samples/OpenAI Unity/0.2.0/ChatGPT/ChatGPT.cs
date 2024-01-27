using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Meta.WitAi.Dictation;
using Meta.WitAi.TTS.Utilities;
using KillingJoke.Core.Utils;

namespace OpenAI
{
    public class ChatGPT : Singleton<ChatGPT>
    {
        [SerializeField] private TextMeshProUGUI _voiceText;
        [SerializeField] private MultiRequestTranscription _multiRequestTranscription;
        [SerializeField] private TTSSpeaker _ttsspeaker;
        [SerializeField] private Button _sendButton;
        [SerializeField] private Button _activateButton;
        [SerializeField] private ScrollRect scroll;
        
        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;

        private float height;
        private OpenAIApi openai = new OpenAIApi("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");

        private List<ChatMessage> messages = new List<ChatMessage>();
        private string prompt = "You act as a jester in medieval times. Respond accordingly. Always address king as 'my lord' at the start of your answer. Don't break character.Always give a different answer. Don't ever mention that you are an AI model.";

        private void Start()
        {
            _sendButton.onClick.AddListener(SendReply);
            _sendButton.enabled = false;
        }

        private void AppendMessage(ChatMessage message)
        {
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
            item.anchoredPosition = new Vector2(0, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;
        }

        private async void SendReply()
        {
            _activateButton.onClick.Invoke();
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = _voiceText.text
            };
            
            AppendMessage(newMessage);

            if (messages.Count == 0) newMessage.Content = prompt + "\n" + _voiceText.text; 
            
            messages.Add(newMessage);
            
            _sendButton.enabled = false;
            
            _voiceText.enabled = false;
            
            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0613",
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                
                messages.Add(message);
                AppendMessage(message);
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }
            var messageTTS = messages[messages.Count - 1].Content;
            _ttsspeaker.Speak(messageTTS.ToString());
            _voiceText.text = "";
            _voiceText.enabled = true;
            _multiRequestTranscription.Clear();
        }
    }
}
