using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Game_Settings : MonoBehaviour
{
    public double healthMultiplier;
    public double dmgMultiplier;
    public double roundLimit;
    public double spclMultiplier;
    public double timerLimit;

    
    
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HealthDropDownHandler(int val)
    {
        Dictionary<int, double> healthDict = new Dictionary<int, double>();

        healthDict.Add(0, 1);
        healthDict.Add(1, 0.25);
        healthDict.Add(2, 0.5);
        healthDict.Add(3, 2);
        healthDict.Add(4, 5);

        healthMultiplier = healthDict[val];
        Debug.Log(healthMultiplier);
    }

    public void DamageDropDownHandler(int val)
    {
        Dictionary<int, double> dmgDict = new Dictionary<int, double>();

        dmgDict.Add(0, 1);
        dmgDict.Add(1, 0.25);
        dmgDict.Add(2, 0.5);
        dmgDict.Add(3, 2);
        dmgDict.Add(4, 5);

        dmgMultiplier = dmgDict[val];
        Debug.Log(dmgMultiplier);
    }

    public void SpecialDropDownHandler(int val)
    {
        Dictionary<int, double> spclDict = new Dictionary<int, double>();

        spclDict.Add(0, 1);
        spclDict.Add(1, 0.25);
        spclDict.Add(2, 0.5);
        spclDict.Add(3, 2);
        spclDict.Add(4, 5);

        spclMultiplier = spclDict[val];
        Debug.Log(spclMultiplier);
    }

    public void RoundDropDownHandler(int val)
    {
        Dictionary<int, double> roundDict = new Dictionary<int, double>();

        roundDict.Add(0, 3);
        roundDict.Add(1, 1);
        roundDict.Add(2, 5);
        roundDict.Add(3, 7);
        roundDict.Add(4, 9);

        roundLimit = roundDict[val];
        Debug.Log(roundLimit);
    }

    public void TimerDropDownHandler(int val)
    {
        Dictionary<int, double> timerDict = new Dictionary<int, double>();

        timerDict.Add(0, 90);
        timerDict.Add(1, 120);
        timerDict.Add(2, 150);
        timerDict.Add(3, 180);
        timerDict.Add(4, 210);
        timerDict.Add(5, 240);
        timerDict.Add(6, 270);
        timerDict.Add(7, 300);

        timerLimit = timerDict[val];
        Debug.Log(timerLimit);
    }
    public void BackToCharSelec()
    {
        Debug.Log("Back");
        SceneManager.LoadScene("MainMenu");
    }
}
