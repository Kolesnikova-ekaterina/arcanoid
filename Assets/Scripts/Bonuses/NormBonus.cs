using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormBonus : BonusBase
{
    public override void BonusActivate()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in balls)
        {
            ball.GetComponent<SpriteRenderer>().color = Color.white;
        }
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject block in blocks)
        {
            block.GetComponent<BlockScript>().ballPower = 1;
        }
    }
}
