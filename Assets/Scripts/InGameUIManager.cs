using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUIManager : MonoBehaviour {

    bool isGamePaused;
    public AudioSource music;
    public GameObject pauseItem;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayPause()
    {
        if (!isGamePaused)
        {
            pauseItem.SetActive(true);
            Time.timeScale = 0f;
            music.Pause();
            isGamePaused = true;
        }

        else
        {
            Time.timeScale = 1f;
            pauseItem.SetActive(false);
            music.UnPause();
            isGamePaused = false;
        }
    }
}
