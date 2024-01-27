using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendButton : MonoBehaviour
{
    private Button _sendButton;

    private void Start()
    {
        _sendButton = gameObject.GetComponent<Button>();
    }
    public void ToggleActivation()
    {
        _sendButton.enabled = !_sendButton.enabled;
    }
}
