using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

[CreateAssetMenu(fileName = "GameData", menuName = "Game Data", order = 51)]
public class GameDataScript : ScriptableObject
{
    public bool music = true;
    public bool sound = true;
    public int pointsToBall = 0;

    public bool resetOnStart;
    public int level = 1;
    public int balls = 6;
    public int points = 0;

    public BonusBase[] bonuses;
    public int fireBonusChance;
    public int steelBonusChance;
    public int normBonusChance;

    public List<Tuple<string, int>> topResults = new();
    public string nickName = "";
    
    public void Reset()
    {
        level = 1;
        balls = 6;
        points = 0;
        pointsToBall = 0;
    }
    public void Load()
    {
        level = PlayerPrefs.GetInt("level", 1);
        balls = PlayerPrefs.GetInt("balls", 6);
        points = PlayerPrefs.GetInt("points", 0);
        pointsToBall = PlayerPrefs.GetInt("pointsToBall", 0);
        music = PlayerPrefs.GetInt("music", 1) == 1;
        sound = PlayerPrefs.GetInt("sound", 1) == 1;
        topResults = PlayerPrefs.GetString("top").Split(',').Where(s => s.Length > 0).Select(pair =>
        {
            var d = pair.Split('-');
            return new Tuple<string, int>(d[0], int.Parse(d[1]));
        }).ToList();
    }
    public void Save()
    {
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetInt("balls", balls);
        PlayerPrefs.SetInt("points", points);
        PlayerPrefs.SetInt("pointsToBall", pointsToBall);
        PlayerPrefs.SetInt("music", music ? 1 : 0);
        PlayerPrefs.SetInt("sound", sound ? 1 : 0);
        PlayerPrefs.SetString("top",
            string.Join(',', topResults.Select(pair => pair.Item1 + '-' + pair.Item2).ToList()));
        
    }
    
    public BonusBase GetRandomBonus()
    {
        int totalChance = fireBonusChance + steelBonusChance + normBonusChance;
        int randomValue = UnityEngine.Random.Range(0, totalChance);
        if (randomValue < fireBonusChance)
        {
            return bonuses[0];
        }
        else if (randomValue < fireBonusChance + steelBonusChance)
        {
            return bonuses[1];
        }
        else
        {
            return bonuses[2];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
