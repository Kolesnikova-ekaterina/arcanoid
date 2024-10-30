using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlockScript : MonoBehaviour
{
    public GameObject textObject;
    protected TMP_Text textComponent;
    public int hitsToDestroy;
    public int points;
    protected PlayerScript playerScript;
    public int ballPower;

    // Start is called before the first frame update
    void Start()
    {
        if (textObject != null)
        {
            textComponent = textObject.GetComponent<TMP_Text>();

            textComponent.text = hitsToDestroy.ToString();

        }
        playerScript = GameObject.FindGameObjectWithTag("Player")
 .GetComponent<PlayerScript>();

    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        {
            hitsToDestroy-=ballPower;
            if (hitsToDestroy <= 0)
            {
                hitsToDestroy = int.MaxValue;
                
                Destroy(gameObject);
                playerScript.BlockDestroyed(points);
            }
            else if (textComponent != null)
                textComponent.text = hitsToDestroy.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
