using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchTimer : MonoBehaviour
{
    float toStartTime = 4f;
    float currentTime = 0f;
    float matchLength = 180f;

    bool intro;

    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] TextMeshProUGUI messageText;

    void Start()
    {
        intro = true;
        currentTime = toStartTime;
        messageText.text = "";
    }

    void Update()
    {
        if(intro)
        {
            currentTime -= 1 * Time.deltaTime;
            

            if (currentTime <= -2)
            {
                intro = false;
                currentTime = matchLength;
                messageText.text = "";
            }
            else if (currentTime <= 0)
            {
                messageText.text = "FIGHT";
            }
            else
            {
                countdownText.text = currentTime.ToString("0");
            }
        }
        else if(currentTime > 0)
        {
        currentTime -= 1 * Time.deltaTime;
        countdownText.text = currentTime.ToString("0");
        }
        else
        {
            currentTime = 0;
            EndGame();
        }
    }




    void EndGame()
    {
        countdownText.text = "00";
        messageText.text = "GAME OVER";
        Time.timeScale = 0;
    }
}