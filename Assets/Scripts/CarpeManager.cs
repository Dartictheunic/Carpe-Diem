using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


    [Space(10)]
    [Header("Liens à faire")]
    public AudioSource mainMusic;
    public AudioSource playerMusic;
    public Carpe leftCarpe;
    public Carpe rightCarpe;
    public ScoreManager scoreText;
    public ScoreManager multiplierText;
    public Text debugText;
    public Text debugText2;
    public Text debugText3;
    public Image styleImage;

    [Space(10)]
    [Header("Variables prog")]
    public bool canMove;
    public float styleInt;
    public int scoreFloat;
    public int multiplier;

    float speed;

    float actualDelta;
    float totalGyro;

    Vector3 deltaGyro;

    float nbObstacles;
    float ObstaclesPassed;
    float percentageOfObstaclesPassed;

    void Start ()
    {
#if !UNITY_EDITOR
        Screen.orientation = ScreenOrientation.Portrait;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
#endif
        speed = bpm / 60;
        Input.gyro.enabled = true;
        totalGyro -= Input.gyro.attitude.eulerAngles.x;
        totalGyro += Input.gyro.attitude.eulerAngles.z;

        foreach(Obstacles obstacles in GameObject.FindObjectsOfType<Obstacles>())
        {
            nbObstacles++;
        }
    }


	void Update ()
    {
        debugText.text = Input.gyro.attitude.eulerAngles.ToString();
        debugText2.text = Input.gyro.rotationRateUnbiased.ToString();
        debugText3.text = Input.acceleration.x.ToString();

        deltaGyro += Input.gyro.rotationRate;

#region commandesDebug

        if (Input.GetKeyDown(KeyCode.C))
            UpdateMultiplier(true);

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

        actualDelta = Input.gyro.attitude.w;
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
        scoreText.UpdateColor(scoreText.text.color = new Color(scoreText.text.color.r, 1 - percentageOfObstaclesPassed, scoreText.text.color.b, scoreText.text.color.a));
    }

    public void ObstaclePassed(int obstacleValue)
    {
        ObstaclesPassed++;
        percentageOfObstaclesPassed = ObstaclesPassed / nbObstacles;
        Debug.Log("Obstacles passed : " + ObstaclesPassed + "Number of obstacles : " + nbObstacles + "Percentage : " + percentageOfObstaclesPassed);
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
            multiplier = 1;
            multiplierText.UpdateMultiplier(multiplier);
            multiplierText.UpdateColor(multiplierColors[multiplier - 1]);
        }
    }

    public void IncreaseStyle(int increase)
    {
        if (multiplier < maximumMultiplier)
        {
            styleInt += increase / (multiplier * 2);

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
