using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUManager : MonoBehaviour
{
    public DiceRoller die1;
    public DiceRoller die2;
    public DiceRoller die3;

    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void cpuLoop(){
        Debug.Log("starting CPULOOP");
        die1.RollDie();
        die2.RollDie();
        die3.RollDie();
    }
}
