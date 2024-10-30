using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBonus : BonusBase
{
    public override void BonusActivate()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in balls)
        {
            ball.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0f);
        }
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject block in blocks)
        {
            block.GetComponent<BlockScript>().ballPower = 4;
        }
        GameObject[] blue_blocks = GameObject.FindGameObjectsWithTag("BlueBlock");
        foreach (GameObject block in blue_blocks)
        {
            block.GetComponent<BlockScript>().ballPower = 4;
        }
    }
}
