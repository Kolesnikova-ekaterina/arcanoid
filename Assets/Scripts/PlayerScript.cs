using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    const int maxLevel = 30;
    [Range(1, maxLevel)]
    public int level = 1;
    public float ballVelocityMult = 0.02f;
    public GameObject bluePrefab;
    public GameObject ExtrabluePrefab;
    public GameObject redPrefab;
    public GameObject greenPrefab;
    public GameObject yellowPrefab;
    public GameObject ballPrefab;
    
    static Collider2D[] colliders = new Collider2D[50];
    static ContactFilter2D contactFilter = new ContactFilter2D();
    public GameDataScript gameData;
    static bool gameStarted = false;
    
    AudioSource audioSrc;
    public AudioClip pointSound;

    public GameObject pausePanel;
    public GameObject endPanel;
    public GameObject newRecordCard;
    
    int requiredPointsToBall
    { get { return 400 + (level - 1) * 20; } }

    // Start is called before the first frame update

    void CreateBlocks(GameObject prefab, float xMax, float yMax,
 int count, int maxCount)
    {
        if (count > maxCount)
            count = maxCount;
        for (int i = 0; i < count; i++)
            for (int k = 0; k < 20; k++)
            {
                var obj = Instantiate(prefab,
                new Vector3((Random.value * 2 - 1) * xMax,
                Random.value * yMax, 0),
                Quaternion.identity);
                if (obj.GetComponent<Collider2D>()
                .OverlapCollider(contactFilter.NoFilter(), colliders) == 0)
                    break;
                Destroy(obj);
            }
    }

    void CreateExtraBlocks(GameObject prefab, float xMax, float yMax,
 int count, int maxCount)
    {
        if (count > maxCount)
            count = maxCount;
        for (int i = 0; i < count; i++)
            for (int k = 0; k < 20; k++)
            {
                var x1 = Random.value * xMax;
                var y1 = Random.value * yMax;
                var objstart = Instantiate(prefab,
                new Vector3(x1, y1, 0),
                Quaternion.identity);

                var x2 = - Random.value * xMax;
                var y2 = Random.value * yMax;
                var objstop = Instantiate(prefab,
                new Vector3(x2, y2, 0),
                Quaternion.identity);
                //print($"{x1},{y1},{x2},{y2}");
                if (objstart.GetComponent<Collider2D>()
                .OverlapCollider(contactFilter.NoFilter(), colliders) == 0 &&
                objstop.GetComponent<Collider2D>()
                .OverlapCollider(contactFilter.NoFilter(), colliders) == 0)
                {
                    objstart.GetComponent<ExtraBlueBlockScript>().direction = new Vector2(x2 - x1, y2 - y1);
                    objstart.GetComponent<ExtraBlueBlockScript>().pos1 = new Vector2(x1, y1);
                    objstart.GetComponent<ExtraBlueBlockScript>().pos2 = new Vector2(x2, y2);
                    Destroy(objstop);
                    break;
                }
                Destroy(objstart);
                Destroy(objstop);
            }
    }
    void CreateBalls()
    {
        int count = 2;
        if (gameData.balls == 1)
            count = 1;
        for (int i = 0; i < count; i++)
        {
            var obj = Instantiate(ballPrefab);
            var ball = obj.GetComponent<BallScript>();
            ball.ballInitialForce += new Vector2(10 * i, 0);
            ball.ballInitialForce *= 1 + level * ballVelocityMult;
        }
    }
    void SetBackground()
    {
        var bg = GameObject.Find("Background").GetComponent<SpriteRenderer>();
        bg.sprite = Resources.Load(level.ToString("d2"),
        typeof(Sprite)) as Sprite;
    }
    void StartLevel()
    {
        SetBackground();
        var yMax = Camera.main.orthographicSize * 0.8f;
        var xMax = Camera.main.orthographicSize * Camera.main.aspect * 0.85f;
        CreateBlocks(bluePrefab, xMax, yMax, level, 8);
        CreateBlocks(redPrefab, xMax, yMax, 1 + level, 10);
        CreateBlocks(greenPrefab, xMax, yMax, 1 + level, 12);
        CreateBlocks(yellowPrefab, xMax, yMax, 2 + level, 15);
        CreateExtraBlocks(ExtrabluePrefab, xMax, yMax, level / 10 + 1 , 3);
        CreateBalls();
    }
    
    void Start()
    {
        audioSrc = Camera.main.GetComponent<AudioSource>();
        Cursor.visible = false;
        if (!gameStarted)
        {
            gameStarted = true;
            if (gameData.resetOnStart)
                gameData.Load();
        }
        level = gameData.level;
        SetMusic();
        StartLevel();
        
    }
    
    void SetMusic()
    {
        if (gameData.music)
            audioSrc.Play();
        else
            audioSrc.Stop();
    }
    
    void OnApplicationQuit()
    {
        gameData.Save();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            gameData.Reset();
            SceneManager.LoadScene("SampleScene");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }


        if (Time.timeScale > 0)
        {
            var mousePos = Camera.main
                .ScreenToWorldPoint(Input.mousePosition);
            var pos = transform.position;
            pos.x = mousePos.x;
            transform.position = pos;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            gameData.music = !gameData.music;
            SetMusic();
        }

        if (Input.GetKeyDown(KeyCode.S))
            gameData.sound = !gameData.sound;
        
        
        if (Input.GetButtonDown("Pause"))
            if (Time.timeScale > 0)
            {
                Time.timeScale = 0;
                if (pausePanel != null)
                {
                    pausePanel.SetActive(true);
                    Cursor.visible = true;
                }
            }
            else
            {
                Time.timeScale = 1;
                pausePanel.SetActive(false);
                Cursor.visible = false;
            }
    

}
    IEnumerator BallDestroyedCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        if (GameObject.FindGameObjectsWithTag("Ball").Length == 0)
            if (gameData.balls > 0)
                CreateBalls();
            else
            {
                UpdateTopIfNeeded();
                
                gameData.Reset();
                SceneManager.LoadScene("SampleScene");
            }

    }

    public void BallDestroyed()
    {
        gameData.balls--;
        StartCoroutine(BallDestroyedCoroutine());

    }
    IEnumerator BlockDestroyedCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        if (GameObject.FindGameObjectsWithTag("Block").Length == 0)
        {
            if (level < maxLevel)
                gameData.level++;
            else 
            {
                Time.timeScale = 0;
                endPanel.SetActive(true);
                Cursor.visible = true;
            }
            if(Time.timeScale > 0)
                SceneManager.LoadScene("SampleScene");
        }

    }
    IEnumerator BlockDestroyedCoroutine2()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.2f);
            audioSrc.PlayOneShot(pointSound, 5);
        }
    }
    public void BlockDestroyed(int points)
    {
        gameData.points += points;
        if (gameData.sound)
            audioSrc.PlayOneShot(pointSound, 5);
        gameData.pointsToBall += points;
        if (gameData.pointsToBall >= requiredPointsToBall)
        {
            gameData.balls++;
            gameData.pointsToBall -= requiredPointsToBall;
            if (gameData.sound)
                StartCoroutine(BlockDestroyedCoroutine2());
        }

        StartCoroutine(BlockDestroyedCoroutine());
    }
    string OnOff(bool boolVal)
    {
        return boolVal ? "on" : "off";
    }
    void OnGUI()
    {
        GUI.Label(new Rect(5, 4, Screen.width - 10, 100),
        string.Format(
        "<color=yellow><size=30>Level <b>{0}</b> Balls <b>{1}</b>" +
        " Score <b>{2}</b></size></color>",
        gameData.level, gameData.balls, gameData.points));
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.UpperRight;
        GUI.Label(new Rect(5, 14, Screen.width - 10, 100),
        string.Format(
            "<color=yellow><size=20><color=white>Space</color>-pause {0}" +
             " <color=white>N</color>-new" +
             " <color=white>J</color>-jump" +
             " <color=white>M</color>-music {1}" +
             " <color=white>S</color>-sound {2}" +
             " <color=white>Esc</color>-exit</size></color>",
             OnOff(Time.timeScale > 0), OnOff(!gameData.music),
             OnOff(!gameData.sound)), style);
    }
    
    
    public void ReturnToGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        Cursor.visible = false;
    }
    
    public void RestartThisLevel()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        Cursor.visible = false;
        SceneManager.LoadScene("SampleScene");
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
