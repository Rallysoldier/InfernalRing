using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelectButtons : MonoBehaviour
{
    public string mapName;
    // Start is called before the first frame update
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void MatchSettings()
    {
        SceneManager.LoadScene("Game_Options");
    }

    public void Map1Selected()
    {
        mapName = "Map1";
        Debug.Log(mapName);
    }

    public void Map2Selected()
    {
        mapName = "Map2";
        Debug.Log(mapName);
    }

    public void Map3Selected()
    {
        mapName = "Map3";
        Debug.Log(mapName);
    }

    public void Map4Selected()
    {
        mapName = "Map4";
        Debug.Log(mapName);
    }
}
