using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameDataScript gameData;
    public GameObject newRecordCard;
    
    public void ReturnToGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        Cursor.visible = false;
    }

    public void StartNewGame()
    {
        UpdateTopIfNeeded();
        Time.timeScale = 1;
        Thread.Sleep(1000);
        gameData.Reset();
        SceneManager.LoadScene("SampleScene");
    }

    public void MoveStartScreen()
    {
        UpdateTopIfNeeded();
        Time.timeScale = 1;
        Thread.Sleep(1000);
        SceneManager.LoadScene("StartMenu");
    }

    private void UpdateTopIfNeeded()
    {
        if (gameData.topResults.Count == 0 || gameData.points > gameData.topResults.Last().Item2)
        {
            newRecordCard.SetActive(true);
            StartCoroutine(HidePanel());
        }

        gameData.topResults.Add(new(gameData.nickName, gameData.points));
        gameData.topResults.Sort((tuple, tuple1) => tuple1.Item2.CompareTo(tuple.Item2));
        if (gameData.topResults.Count > 5)
        {
            gameData.topResults.RemoveAt(gameData.topResults.Count - 1);
        }
        gameData.Save();
    }

    IEnumerator HidePanel()
    {
        yield return new WaitForSeconds(2f);
        newRecordCard.SetActive(false);
    }
   
}
