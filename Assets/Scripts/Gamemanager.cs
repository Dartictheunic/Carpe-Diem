using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour {

    public static Gamemanager gamemanager;

    public AudioSource menuMusic;

    public int levelsUnlocked;
    public int difficulty;
    public float volume;

    public TMPro.TMP_Dropdown difficultyDropdown;
    public TMPro.TextMeshProUGUI level1Highscore;
    public TMPro.TextMeshProUGUI level2Highscore;
    public Slider volumeSlider;

    public GameObject[] allMenuItemsArray;

    public void Quit()
    {
        Application.Quit();
    }

    void Awake()
    {
#if !UNITY_EDITOR
        Screen.orientation = ScreenOrientation.Portrait;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
#endif
        DontDestroyOnLoad(this.gameObject);
        gamemanager = this;

        if (PlayerPrefs.HasKey("Levels"))
        {
            levelsUnlocked = PlayerPrefs.GetInt("Levels");
        }

        else
        {
            PlayerPrefs.SetInt("Levels", 0);
            SavePlayerPrefs();
        }

        if (PlayerPrefs.HasKey("Difficulty"))
        {
            difficulty = PlayerPrefs.GetInt("Difficulty");
            difficultyDropdown.value = difficulty;
        }

        else
        {
            PlayerPrefs.SetInt("Difficulty", 1);
            difficulty = 1;
            difficultyDropdown.value = difficulty;
            SavePlayerPrefs();
        }

        if (PlayerPrefs.HasKey("Volume"))
        {
            volume = PlayerPrefs.GetFloat("Volume");
            volumeSlider.value = volume;
        }

        else
        {
            PlayerPrefs.SetFloat("Volume", 1);
            volume = 1;
            volumeSlider.value = volume;
            SavePlayerPrefs();
        }

        if (PlayerPrefs.HasKey("Level 1") && PlayerPrefs.HasKey("Level 1Player"))
        {
            level1Highscore.SetText("Highscore : " + PlayerPrefs.GetInt("Level 1").ToString() + "\nBy : " + PlayerPrefs.GetString("Level 1Player"));
        }

        if (PlayerPrefs.HasKey("Level 2") && PlayerPrefs.HasKey("Level 2Player"))
        {
            level1Highscore.SetText("Highscore : " + PlayerPrefs.GetInt("Level 2").ToString() + "\nBy : " + PlayerPrefs.GetString("Level 2Player"));
        }

    }

    public void SetVolume(float value)
    {
        PlayerPrefs.SetFloat("Volume", value);
        volume = value;
        menuMusic.volume = volume;
        SavePlayerPrefs();
    }

	public void SetDifficulty(int value)
    {
        PlayerPrefs.SetInt("Difficulty", value);
        difficulty = value;
        difficultyDropdown.value = difficulty;
        SavePlayerPrefs();
    }
	
    public void ActivateMenuItem(GameObject objectToActivate)
    {
        for (int i = 0; i < allMenuItemsArray.Length; i++)
        {
            if (allMenuItemsArray[i] != objectToActivate)
            {
                if (allMenuItemsArray[i].activeInHierarchy)
                {
                    allMenuItemsArray[i].SetActive(false);
                }
            }

            else
            {
                allMenuItemsArray[i].SetActive(true);
            }
        }
    }

    public void LoadLevel(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void SavePlayerPrefs()
    {
        PlayerPrefs.Save();
    }
}
