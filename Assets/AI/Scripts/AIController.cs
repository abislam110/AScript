using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using LLMUnity;
using System;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
public class AIController : MonoBehaviour
{
    public TMP_Text displayText;
    public TMP_InputField inputField;
    public Button sendButton;
    public LLMCharacter llmCharacter;
    public static string responseMessage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartConversation();
        sendButton.onClick.AddListener(() => GetResponse());
    }
    private void StartConversation()
    {
        inputField.text = "";
        string startString = "Write a scenario that you want to see the AI act out.";
        displayText.text = startString;
        Debug.Log(startString);
    }
    private async void GetResponse()
    {
        if(string.IsNullOrEmpty(inputField.text))
        {
            return;
        }
        sendButton.enabled = false;
        displayText.text += string.Format("\nYou: {0}", inputField.text);
        inputField.text = "";
        responseMessage = await llmCharacter.Chat(inputField.text, HandleReply, ReplyCompleted);
        displayText.text += string.Format("\nAI: {0}", responseMessage);
    }
    void HandleReply(string reply)
    {
       
    }
    void ReplyCompleted()
    {
        //SceneManager.LoadScene("CharacterScene");
    }
}
