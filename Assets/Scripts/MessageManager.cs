using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    public Text messageText;
    public GameObject messageBG;
    public float displayTime = 2f;

    

    void Start()
    {
        disableBG(messageBG);
    }

    void Update()
    {
        
    }

    public void displayMessage(string message){
        Debug.Log("Message displayed.");
        enableBG(messageBG);
        messageText.text = message;
        StartCoroutine(hideMessage());
    }

    IEnumerator hideMessage(){
        yield return new WaitForSeconds(displayTime);
        disableBG(messageBG);
        messageText.text = "";
    }

    // public void displayIntroMessage(string message){
    //     Debug.Log("Message displayed.");
    //     enableBG(messageBG);
    //     messageText.text = message;
    //     StartCoroutine(hideIntroMessage());
    // }

    // IEnumerator hideIntroMessage(){
    //     yield return new WaitForSeconds(10f);
    //     disableBG(messageBG);
    //     messageText.text = "";
    // }

    private void enableBG(GameObject image){
        image.SetActive(true);
    }

    private void disableBG(GameObject image){
        image.SetActive(false);
    }
}
