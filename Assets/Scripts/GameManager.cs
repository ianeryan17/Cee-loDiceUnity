using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    public MessageManager messageManager;
    public CPUManager cpuManager;

    [Header("Money Section")]
    public static int currMoney = 500;
    public GameObject moneyText;

    public static int stakeAmount = 0;
    public GameObject stakeText;

    [Header("UI Buttons")]
    public Button playerRollButton;
    public Button lockInButton;
    public Button resetButton;
    public Button[] chips = new Button[4];

    [Header("Dice Values")]
    // 0-2 are player dice 1-3, 3-5 are banker dice 1-3
    public int[] diceNumbers = new int[6];

    [Header("Set Points")]
    public int bankerSetValue = 0;
    public int playerSetValue = 0;

    void Start()
    {
        for (int i = 0; i < 6; i++){
            diceNumbers[i] = 1;
        }
        updateStatsDisplay();
        MainLoop();
    }

    void MainLoop(){
        Debug.Log("main loop called");
        chipsActivator(chips, true);
        disappearButton(playerRollButton);
        disappearButton(lockInButton);
        disappearButton(resetButton);  
    }
    
    public void UpdateNumberOnDie(int diceIndex, int newNumber)
    {
        if (diceIndex >= 1 && diceIndex < diceNumbers.Length + 1)
        {
            diceNumbers[diceIndex - 1] = newNumber;
            Debug.Log("Number on die " + diceIndex + " updated to: " + newNumber);
        } else {
            Debug.LogError("Invalid die index");
        }

        if ((diceIndex == 3) || (diceIndex == 6)){
            combinationCheck(diceIndex);
        }
    }

    public void updateStatsDisplay(){
        Debug.Log("updateStatsDisplay called");
        Debug.Log("Starting money: " + currMoney);
        Debug.Log("Starting stake: " + stakeAmount);

        Text healthTextTemp = moneyText.GetComponent<Text>();
        healthTextTemp.text = "Money: " + currMoney;

        Text tokensTextTemp = stakeText.GetComponent<Text>();
        tokensTextTemp.text = "Stake: " + stakeAmount;
    }

    public void updateStakeAmount(int newAmount){
        Debug.Log("Starting stake: " + stakeAmount);
        if ((stakeAmount + newAmount) <= currMoney){
            stakeAmount += newAmount;
        } else {
            Debug.Log("Not enough money!");
        }
        
        Text tokensTextTemp = stakeText.GetComponent<Text>();
        tokensTextTemp.text = "Stake: " + stakeAmount;
        Debug.Log("New stake: " + stakeAmount);
        appearButton(lockInButton);
        appearButton(resetButton);
    }

    public void resetStakeAmount(){
        Debug.Log("Current stake: " + stakeAmount);
        stakeAmount = 0;

        Text tokensTextTemp = stakeText.GetComponent<Text>();
        tokensTextTemp.text = "Stake: " + stakeAmount;
        Debug.Log("New stake: " + stakeAmount);
        disappearButton(lockInButton);
    }

    private void combinationCheck(int diceIndex){
        if (diceIndex == 3){
            Debug.Log("Player dice combo check");
            combinationHelper(diceNumbers[0], diceNumbers[1], diceNumbers[2]);
        } else if (diceIndex == 6){
            Debug.Log("Banker dice combo check");
            bankerHelper(diceNumbers[3], diceNumbers[4], diceNumbers[5]);
        } else {
            Debug.Log("diceIndex" + diceIndex + "is invalid");
        }
    }

    private void combinationHelper(int die1, int die2, int die3){
        if ((die1 == 4 && die2 == 5 && die3 == 6) || 
            (die1 == 4 && die2 == 6 && die3 == 5) || 
            (die1 == 5 && die2 == 4 && die3 == 6) || 
            (die1 == 5 && die2 == 6 && die3 == 4) || 
            (die1 == 6 && die2 == 5 && die3 == 4) || 
            (die1 == 6 && die2 == 4 && die3 == 5))
        {
            //auto win case
            Debug.Log("Auto win case found");
            playerAutoWin();
        } else if ((die1 == die2) && (die2 == die3) && (die1 == die3)){ 
            //triple case
            Debug.Log("Triple case found");
            playerAutoWin();
        } else if ((die1 == die2) && (die1 != die3)){
            //die3 set point case
            Debug.Log("Die3 set point case found");
            playerSet(die3);
        } else if ((die1 == die3) && (die1 != die2)){
            //die2 set point case
            Debug.Log("Die2 set point case found");
            playerSet(die2);
        } else if ((die2 == die3) && (die1 != die2)){
            //die1 set point case
            Debug.Log("Die1 set point case found");
            playerSet(die1);
        } else if ((die1 == 1 && die2 == 2 && die3 == 3) || 
                   (die1 == 1 && die2 == 3 && die3 == 2) || 
                   (die1 == 2 && die2 == 1 && die3 == 3) || 
                   (die1 == 2 && die2 == 3 && die3 == 1) || 
                   (die1 == 3 && die2 == 2 && die3 == 1) || 
                   (die1 == 3 && die2 == 1 && die3 == 2))
        {
            //auto lose case
            Debug.Log("Auto lose case found");
            playerAutoLoss();
        } else {
            //reroll case, no recognized combination found
            Debug.Log("Reroll case found");
            playerReroll();
        }
    }

    private void bankerHelper(int die1, int die2, int die3){
        if ((die1 == 4 && die2 == 5 && die3 == 6) || (die1 == 4 && die2 == 6 && die3 == 5) || (die1 == 5 && die2 == 4 && die3 == 6) || (die1 == 5 && die2 == 6 && die3 == 4) || (die1 == 6 && die2 == 5 && die3 == 4) || (die1 == 6 && die2 == 4 && die3 == 5)){
            //auto win case  
            Debug.Log("Auto win case found");
            StartCoroutine(delayedBankerAction(true));
        } else if ((die1 == die2) && (die2 == die3) && (die1 == die3)){ 
            //triple case
            Debug.Log("Triple case found");
            StartCoroutine(delayedBankerAction(true));
        } else if ((die1 == die2) && (die1 != die3)){
            //die3 set point case
            Debug.Log("Die3 set point case found");
            bankerSet(die3);
        } else if ((die1 == die3) && (die1 != die2)){
            //die2 set point case
            Debug.Log("Die2 set point case found");
            bankerSet(die2);
        } else if ((die2 == die3) && (die1 != die2)){
            //die1 set point case
            Debug.Log("Die1 set point case found");
            bankerSet(die1);
        } else if ((die1 == 1 && die2 == 2 && die3 == 3) || (die1 == 1 && die2 == 3 && die3 == 2) || (die1 == 2 && die2 == 1 && die3 == 3) || (die1 == 2 && die2 == 3 && die3 == 1) || (die1 == 3 && die2 == 2 && die3 == 1) || (die1 == 3 && die2 == 1 && die3 == 2)){
            //auto lose case
            Debug.Log("Auto lose case found");
            StartCoroutine(delayedBankerAction(false));
        } else {
            //reroll case, no recognized combination found
            Debug.Log("Reroll case found");
            bankerReroll();
        }
    }

    private void appearButton(Button button){
        button.gameObject.SetActive(true);
    }

    private void disappearButton(Button button){
        button.gameObject.SetActive(false);
    }

    private void chipsActivator(Button[] chips, bool modifier){
        if (modifier == true){
            for (int i = 0; i < 4; i++){
                enableButton(chips[i]);
            }
            Debug.Log("finished activating chips");
        } else {
            for (int i = 0; i < 4; i++){
                disableButton(chips[i]);
            }
            Debug.Log("finished disactivating chips");  
        }
        
    }

    private void disableButton(Button button){
        Debug.Log("disable called");
        button.interactable = false;
    }

    private void enableButton(Button button){
        Debug.Log("enable called");
        button.interactable = true;
    }

    public void lockInWager(){
        Debug.Log("Wager locked in.");
        disappearButton(lockInButton);
        disappearButton(resetButton);
        chipsActivator(chips, false);
        messageManager.displayMessage("Wager set, Banker to roll.");
        StartCoroutine(delayedCPUCall());
    }

    IEnumerator delayedCPUCall()
    {
        Debug.Log("Before delay");

        // Wait for 3 seconds
        yield return new WaitForSeconds(2);

        Debug.Log("After delay");

        cpuManager.cpuLoop();
    }

    IEnumerator delayedPlayerAction(bool isWin)
    {
        Debug.Log("Before delay");

        // Wait for 3 seconds
        yield return new WaitForSeconds(1);

        Debug.Log("After delay");

        if (isWin){
            playerAutoWin();
        } else {
            playerAutoLoss();
        }
    }

    public void playerAutoWin(){
        Debug.Log("Player auto-won!");
        messageManager.displayMessage("Player auto-won!");
        playerWin();
    }

    public void playerAutoLoss(){
        Debug.Log("Player auto-lost!");
        messageManager.displayMessage("Player auto-lost!");
        playerLose();
    }

    public void playerReroll(){
        StartCoroutine(delayMessage("Player reroll!", 1/2));
    }

    public void playerSet(int setValue){
        Debug.Log("player set value is: " + setValue);
        StartCoroutine(delayMessage("Player set point: " + setValue + "!", 1/2));
        // messageManager.displayMessage("Player set point: " + setValue + "!");
        
        if (setValue == 6){
            StartCoroutine(delayedPlayerAction(true));
        } else if (setValue == 1){
            StartCoroutine(delayedPlayerAction(false));
        } else {
            playerSetValue = setValue;
            StartCoroutine(determineWinner());
        } 
    }

    public void playerWin(){
        currMoney += stakeAmount;
        stakeAmount = 0;
        updateStatsDisplay();
        MainLoop();
    }

    public void playerLose(){
        currMoney -= stakeAmount;
        stakeAmount = 0;
        updateStatsDisplay();
        MainLoop();
    }

    public void bankerAutoWin(){
        Debug.Log("Banker auto-won!");
        messageManager.displayMessage("Banker auto-won!");
        playerLose();
    }

    IEnumerator delayedBankerAction(bool isWin)
    {
        Debug.Log("Before delay");

        // Wait for 3 seconds
        yield return new WaitForSeconds(1/2);

        Debug.Log("After delay");

        if (isWin){
            bankerAutoWin();
        } else {
            bankerAutoLoss();
        }
    }

    public void bankerAutoLoss(){
        Debug.Log("Banker auto-lost!");
        messageManager.displayMessage("Banker auto-lost!");
        playerWin();
    }

    public void bankerReroll(){
        messageManager.displayMessage("Banker reroll!");
        cpuManager.cpuLoop();
    }

    public void bankerSet(int setValue){
        Debug.Log("banker set value is: " + setValue);
        // messageManager.displayMessage("Banker set point: " + setValue + "!");
        StartCoroutine(delayMessage("Banker set point: " + setValue + "!", 1/2));
        
        if (setValue == 6){
            StartCoroutine(delayedBankerAction(true));
        } else if (setValue == 1){
            StartCoroutine(delayedBankerAction(false));
        } else {
            Debug.Log("what's going on here");

            bankerSetValue = setValue;
            appearButton(playerRollButton);
        } 
    }

    IEnumerator determineWinner(){
        yield return new WaitForSeconds(2);
        
        if (playerSetValue > bankerSetValue){ //player has higher set value
            messageManager.displayMessage("Player has higher set value.");
            StartCoroutine(delayMessage("Player wins!", 1));
            playerWin();
        } else if (playerSetValue < bankerSetValue){ //banker has higher set value
            messageManager.displayMessage("Banker has higher set value.");
            StartCoroutine(delayMessage("Player loses!", 1));
            playerLose();
        } else { //tie
            messageManager.displayMessage("Player and Banker have equal set value.");
            StartCoroutine(delayMessage("It's a push!", 1));
            StartCoroutine(delayMessage("Player recieves initial stake.", 1));
            resetStakeAmount();
            MainLoop();
        }
    }

    IEnumerator delayMessage(string message, int time){
        Debug.Log("Before delay");

        // Wait for 3 seconds
        yield return new WaitForSeconds(time);

        Debug.Log("After delay");

        messageManager.displayMessage(message);
    }

    public void Instructions() {
        SceneManager.LoadScene("Instructions");
    }

    public void StartGame() {
        SceneManager.LoadScene("GameScene");
    }

     // Return to MainMenu
    public void RestartGame() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Instructions");
        // Reset all static variables here, for new games:
        currMoney = 500;
        stakeAmount = 0;
    }

    public void QuitGame() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void Credits() {
        SceneManager.LoadScene("Credits");
    }
}
