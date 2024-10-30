using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SteelBonus : BonusBase
{
    public override void BonusActivate()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in balls)
        {
            ball.GetComponent<SpriteRenderer>().color = Color.gray;
        }
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        GameObject[] blue_blocks = GameObject.FindGameObjectsWithTag("BlueBlock");
        foreach (GameObject block in blocks)
        {
            block.GetComponent<BlockScript>().ballPower = 40;
        }
        foreach (GameObject block in blue_blocks)
        {
            block.GetComponent<BlockScript>().ballPower = 40;
        }
    }
}
