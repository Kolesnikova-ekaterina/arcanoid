using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;
    [SerializeField] private GameDataScript gameData;
    [SerializeField] private List<TMP_Text> places;

    // Start is called before the first frame update
    void Start()
    {
        //print("AAAAAAAAAAA");
        
        gameData.Load();
        //print("AAAAAAAAAAA");
        //print($"AAAAA {gameData.topResults.Count}");
        for (int i = 0; i < gameData.topResults.Count; i++)
        {
            //print(gameData.topResults[i]);
            places[i].text = $"{gameData.topResults[i].Item1} : {gameData.topResults[i].Item2}";
        }
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void StartGame()
    {
        var nick = input.text;
        if (nick == "")
        {
            nick = "NoName";
        }

        gameData.nickName = nick;
        StartCoroutine(StartMenuCoroutine());
    }
    
    IEnumerator StartMenuCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        gameData.Reset();
        SceneManager.LoadScene("SampleScene");
        
    }
    
    
    
    
}
