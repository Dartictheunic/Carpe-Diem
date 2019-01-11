using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class tamer : MonoBehaviour {
    bool tamerlaput;
    public AudioSource music;
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

    public void PlayPause()
    {
        if (!tamerlaput)
        {
        Time.timeScale = 0f;
            music.Pause();
            tamerlaput = true;
        }

        else
        {
        Time.timeScale = 1f;
            music.UnPause();
            tamerlaput = false;
        }
    }
}
