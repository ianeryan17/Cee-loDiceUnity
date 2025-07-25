using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    public Sprite[] diceSides;
    private SpriteRenderer rend;
    public int diceIndex;
    // private int whosTurn = 1;
    private int numberOfSides = 6;
    
    public GameManager gameManager;
    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = diceSides[0];
    }

    public void RollDie()
    {
        int result = Random.Range(1, numberOfSides + 1);
        Debug.Log("Result of Dice Roll: " + result);
        rend.sprite = diceSides[result - 1];
        gameManager.UpdateNumberOnDie(diceIndex, result);
    }

    // public void RollBankerDice()
    // {
    //     // Perform dice rolling logic here...
        
    //     // For example, let's say you roll a random number between 1 and 6 for each die
    //     for (int i = 0; i < 6; i++)
    //     {
    //         int randomNumber = Random.Range(1, 7);
            
    //         // Update the GameManager's diceNumbers array with the new number for this die
    //         gameManager.UpdateNumberOnDie(i, randomNumber);
    //     }
    // }
}
