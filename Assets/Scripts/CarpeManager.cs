using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CarpeManager : MonoBehaviour {
    [Header ("Variables à tweak")]
    [Tooltip("BPM de la musique")]
    [Range(60, 180)]
    public float bpm;
    [Tooltip("A quel point baisse l'audio quand on se fait hit")]
    [Range(0f, 1f)]
    public float hurtSoundDecrease;
    [Tooltip("Vitesse à laquelle l'audio remonte")]
    [Range(0f, .1f)]
    public float passiveSoundIncrease;
    [Tooltip("Valeur d'offset")]
    public float xOffsetCarpes;
    [Tooltip("Multiplicateur maximal")]
    public int maximumMultiplier;
    [Tooltip("Couleurs du multiplicateur")]
    public Color[] multiplierColors;
    [Tooltip("Quand la map est finie")]
    public float positionWin;

    [Space(10)]
    [Header("Liens à faire")]
    public AudioSource mainMusic;
    public AudioSource playerMusic;
    public Carpe leftCarpe;
    public Carpe rightCarpe;
    public ScoreManager scoreText;
    public ScoreManager multiplierText;
    public Image styleImage;
    public Animator cameraAnimator;
    public TMPro.TextMeshProUGUI highscoreString;
    public TMPro.TextMeshProUGUI rankString;
    public TMPro.TextMeshProUGUI scoreString;
    public GameObject newHighscore;
    public GameObject winScreen;
    public GameObject tapToExit;



    [Space(10)]
    [Header("Variables prog")]
    public bool canMove;
    public float styleInt;
    public int scoreFloat;
    public int multiplier;

    float speed;


    float totalGyro;

    Vector3 deltaGyro;

    float nbObstacles;
    float ObstaclesPassed;
    float percentageOfObstaclesPassed;
    bool end;
    int difficultyManager;

    public void SubmitHighScore(string playerName)
    {
        PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "Player", playerName);
        PlayerPrefs.Save();
    }

    public void End()
    {
        end = true;
        cameraAnimator.SetTrigger("Victory");
        leftCarpe.Win();
        rightCarpe.Win();
        winScreen.SetActive(true);
        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name))
        {
            var lastHighscore = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name);
            if (scoreFloat > lastHighscore)
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, scoreFloat);
                PlayerPrefs.Save();
                newHighscore.SetActive(true);
                tapToExit.SetActive(false);
            }
        }

        else
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, scoreFloat);
            PlayerPrefs.Save();
            tapToExit.SetActive(false);
            newHighscore.SetActive(true);
        }

        highscoreString.SetText(PlayerPrefs.GetInt(SceneManager.GetActiveScene().name).ToString());
        scoreString.SetText(scoreFloat.ToString());

        if (percentageOfObstaclesPassed == 100)
        {
            rankString.SetText("S");
        }

        else if (percentageOfObstaclesPassed > 90)
        {
            rankString.SetText("A");
        }

        else if (percentageOfObstaclesPassed > 75)
        {
            rankString.SetText("B");
        }

        else if (percentageOfObstaclesPassed > 50)
        {
            rankString.SetText("C");
        }


        else if (percentageOfObstaclesPassed <= 50)
        {
            rankString.SetText("Nul");
        }
    }

    void Start ()
    {
#if !UNITY_EDITOR
        Screen.orientation = ScreenOrientation.Portrait;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        difficultyManager = Gamemanager.gamemanager.difficulty + 1;
#endif
        speed = bpm / 60;
        Input.gyro.enabled = true;
        totalGyro -= Input.gyro.attitude.eulerAngles.x;
        totalGyro += Input.gyro.attitude.eulerAngles.z;

        foreach(Obstacles obstacles in GameObject.FindObjectsOfType<Obstacles>())
        {
            nbObstacles++;
        }

#if UNITY_EDITOR
        difficultyManager = 2;
#endif

    }


	void Update ()
    {
        if (transform.position.z > positionWin && !end)
        {
            End();
        }

        deltaGyro += Input.gyro.rotationRate;

#region commandesDebug

        if (Input.GetKeyDown(KeyCode.C))
            PlayerPrefs.DeleteAll();

        if (Input.GetKeyDown(KeyCode.W))
            Debug.Log(PlayerPrefs.GetString("Level 2Player"));

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            leftCarpe.Jump();

        
        if (Input.GetKeyDown(KeyCode.RightArrow))
            rightCarpe.Jump();

        if (Input.GetKey(KeyCode.Q))
        {
            leftCarpe.MoveOut(-xOffsetCarpes);
            rightCarpe.MoveIn();
        }

            totalGyro = 30;

        if (Input.GetKey(KeyCode.D))
        {
            leftCarpe.MoveIn();
            rightCarpe.MoveOut(xOffsetCarpes);
        }

        if (Input.GetKey(KeyCode.S))
        {
            leftCarpe.MoveIn();
            rightCarpe.MoveIn();
        }

#endregion

        totalGyro -= Input.gyro.rotationRate.y;
        totalGyro += Input.gyro.rotationRate.z;

        //if (totalGyro > tiltCap)
        //{
        //    leftCarpe.MoveOut(-xOffsetCarpes);
        //    rightCarpe.MoveIn();
        //}

//else if (totalGyro < -tiltCap)
//{
//    leftCarpe.MoveIn();
//    rightCarpe.MoveOut(xOffsetCarpes);
//}

//else if (-tiltCap < totalGyro && totalGyro < tiltCap)
//{
//    leftCarpe.MoveIn();
//    rightCarpe.MoveIn();
//}

#if !UNITY_EDITOR
        if (Input.acceleration.x <= -.15f)
        {
            leftCarpe.MoveOut(-xOffsetCarpes);
            rightCarpe.MoveIn();
        }

        else if (Input.acceleration.x >= .15f)
        {
            leftCarpe.MoveIn();
            rightCarpe.MoveOut(xOffsetCarpes);
        }

        else
        {
            leftCarpe.MoveIn();
            rightCarpe.MoveIn();
        }

#endif

#region gestion inputs

        if (Input.touchCount > 0)
        {
		    foreach(var fingerInput in Input.touches)
            {

                if (fingerInput.position.x < Screen.width / 2)
                {
                    switch (fingerInput.phase)
                    {
                        case TouchPhase.Began:
                            {
                                leftCarpe.Jump();
                            }
                            break;
                    }
                }

                else if (fingerInput.position.x > Screen.width / 2)
                {
                    switch (fingerInput.phase)
                    {
                        case TouchPhase.Began:
                            {
                                rightCarpe.Jump();
                            }
                            break;
                            
                    }
                }
            }
        }

#endregion
    }



    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }

    }

    public void LaunchGame()
    {
        canMove = true;
        mainMusic.Play();
        playerMusic.Play();

        deltaGyro = Input.gyro.attitude.eulerAngles;
    }

    public void Move()
    {
        transform.position += new Vector3(0, 0, speed * Time.deltaTime);
    }

    public void TurnDownPlayerMusic()
    {
        playerMusic.volume -= hurtSoundDecrease;
    }

    public void IncreaseScore(int increase)
    {
        scoreFloat += increase * multiplier;
        scoreText.UpdateScore(scoreFloat);
        scoreText.UpdateColor(scoreText.text.color = new Color(scoreText.text.color.r, 1 - (percentageOfObstaclesPassed/100), scoreText.text.color.b, scoreText.text.color.a));
    }

    public void ObstaclePassed(int obstacleValue)
    {
        ObstaclesPassed++;
        percentageOfObstaclesPassed = (ObstaclesPassed / nbObstacles) * 100;
        IncreaseStyle(obstacleValue);
    }

    public void UpdateMultiplier(bool increase)
    {
        if (multiplier < maximumMultiplier && increase)
        {
            multiplier ++;
            multiplierText.UpdateMultiplier(multiplier);
            multiplierText.UpdateColor(multiplierColors[multiplier-1]);
        }

        else if (!increase)
        {
            Debug.Log(Mathf.Clamp((Mathf.Round(multiplier) / difficultyManager), 1, 8));
            multiplier = Mathf.FloorToInt(Mathf.Clamp((Mathf.Round(multiplier) / difficultyManager), 1, 8)) ;
            multiplierText.UpdateMultiplier(multiplier);
            multiplierText.UpdateColor(multiplierColors[multiplier - 1]);
            styleInt = 0;
            IncreaseStyle(0);
        }
    }

    public void IncreaseStyle(int increase)
    {
        if (multiplier < maximumMultiplier)
        {
            styleInt += increase / ((Mathf.RoundToInt((multiplier * 2) / difficultyManager))+1);

            if (styleInt >= 100)
            {
                styleInt = 0f;
                UpdateMultiplier(true);
            }

            styleImage.fillAmount = styleInt / 100;
        }
    }

    public bool FastApproximately(float a, float b)
    {
        return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= .01f;
    }

}
