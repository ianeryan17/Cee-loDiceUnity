using UnityEngine;

public class Wager : MonoBehaviour
{
    public int wagerIndex;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeWager(){
        int newAmount = wagerIndex;
        gameManager.updateStakeAmount(newAmount);
    }

    // public void ResetWager(){
    //     gameManager.resetStakeAmount();
    // }
}
