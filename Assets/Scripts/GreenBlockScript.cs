using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GreenBlockScript : BlockScript
{
    public GameDataScript gameData;
    
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        {
            hitsToDestroy -= ballPower;
            if (hitsToDestroy <= 0)
            {
                hitsToDestroy = int.MaxValue;
                print(points);
                Destroy(gameObject);
                Instantiate(gameData.GetRandomBonus(), transform.position, transform.rotation);
                playerScript.BlockDestroyed(points);
            }
            else if (textComponent != null)
                textComponent.text = hitsToDestroy.ToString();
        }
    }
}
