using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BonusBase : MonoBehaviour
{
    public GameObject textObject;
    TMP_Text textComponent;
    public Color backgroundColor = Color.yellow;
    public Color textColor = Color.black;
    public string text = "+100";
    public GameDataScript gameData;

    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = backgroundColor;
        }

        if (textObject != null)
        {
            textComponent = textObject.GetComponent<TMP_Text>();

            textComponent.text = text;
            textComponent.color = textColor;

        }
    }

    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            BonusActivate();
        }
        Destroy(gameObject);
    }

    public virtual void BonusActivate()
    {
        gameData.points += 100;
    }
}
