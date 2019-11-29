using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class BallScript : MonoBehaviour
{
    #region Variable Decleration

    bool ready;
    bool aiming;
    Vector2 startPos, endPos;
    Vector3 shootPos;

    float maxDist = 5f;
    float speed = 100f;

    public GameObject line;
    public GameObject tutorialText;

    public GameObject tinyPad, smallPad, mediumPad, bigPad;
    GameObject padSpawn;
    int tinyPer = 100, smallPer = 97, mediumPer = 92, bigPer = 70;
    bool startPad;
    Vector3 padPos;

    public bool touchingWall = false;

    public Text scoreText;
    int score, bestScore;
    public Text creditText;
    int credits;
    public GameObject creditPrefab, currentCredit;
    int lives = 3;
    public Text livesText;

    public Image reviveTimer;
    public GameObject reviveMenu;
    public GameObject gameOverMenu;
    bool usedRevived = false;

    public AudioClip hitBall;
    public AudioClip padScore;
    public AudioClip wallBounce;
    public new AudioSource audio;

    public GameObject leftWall, rightWall, topWall, bottomWall;
    Collider2D leftColl, rightColl, topColl, bottomColl;

    Vector2 bounceL;
    Vector2 bounceR;
    Vector2 bounceU;
    Vector2 bounceD;

    public Sprite[] ballSprites = new Sprite[23];

    #endregion



    // Use this for initialization
    void Start()
    {
        SetupPlayerPrefs();

        ChangeBallSprite();

        bounceL = new Vector2(-4, 0);
        bounceR = new Vector2(4, 0);
        bounceU = new Vector2(0, 4);
        bounceD = new Vector2(0, -4);

        Time.timeScale = 1;
        this.gameObject.SetActive(true);

        score = -1;

        audio = GetComponent<AudioSource>();

        startPad = true;
        Scored();
        SpawnCredit();
        PlayerPrefs.SetInt("Credits", credits);
        creditText.text = credits.ToString();
        livesText.text = lives.ToString();

        leftColl = leftWall.GetComponent<Collider2D>();
        rightColl = rightWall.GetComponent<Collider2D>();
        topColl = topWall.GetComponent<Collider2D>();
        bottomColl = bottomWall.GetComponent<Collider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (credits < 0)
        {
            credits = 0;
            creditText.text = credits.ToString();
        }

        if (GetComponent<Collider2D>().IsTouching(leftColl))
            WallBounce(bounceR);
        else if (GetComponent<Collider2D>().IsTouching(rightColl))
            WallBounce(bounceL);
        else if (GetComponent<Collider2D>().IsTouching(topColl))
            WallBounce(bounceD);
        else if (GetComponent<Collider2D>().IsTouching(bottomColl))
            WallBounce(bounceU);

        //if (this.GetComponent<Rigidbody2D> ().velocity.sqrMagnitude < 0.01f) {
        if (this.GetComponent<Rigidbody2D>().IsSleeping())
        {
            ready = true;
        }
        else
        {
            ready = false;
        }

        #region User Touch Input
        if (Input.GetMouseButton(0) == true && !aiming && ready)
        {
            aiming = true;
            tutorialText.SetActive(false);
        }

        if (aiming)
        {
            //ENABLE LINE
            line.GetComponent<LineRenderer>().enabled = true;
            //CONTROL INPUT && SHOOT POSITIONS
            startPos = this.transform.position;
            line.GetComponent<LineRenderer>().SetPosition(0, this.transform.position);
            shootPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            shootPos.z = 0;
            shootPos = this.transform.position + (this.transform.position - shootPos);
            endPos = shootPos;
            //DISPLAY LINE
            if (Vector3.Distance(startPos, shootPos) > maxDist)
            {
                Vector3 dir = endPos - startPos;
                endPos = this.transform.position + (dir.normalized * maxDist);
            }
            line.GetComponent<LineRenderer>().SetPosition(1, endPos);
        }
        else
        {
            line.GetComponent<LineRenderer>().enabled = false;
        }

        if (aiming && (Vector3.Distance(startPos, shootPos) < 0.7f))
        {
            line.GetComponent<LineRenderer>().enabled = false;
            aiming = false;
        }

        if (Input.GetMouseButtonUp(0) == true && aiming && ready)
        {
            endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Shoot();
        }
    }

    void Shoot()
    {
        aiming = false;
        if (Vector2.Distance(startPos, endPos) > maxDist)
        {
            line.GetComponent<LineRenderer>().enabled = false;
        }
        Vector2 direction = startPos - endPos;
        this.GetComponent<Rigidbody2D>().AddForce(direction * speed);
        audio.clip = hitBall;
        audio.Play();
    }

    #endregion

    void SetupPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("Credits"))
        {
            credits = PlayerPrefs.GetInt("Credits");
            creditText.text = credits.ToString();
        }
        else
        {
            credits = 0;
            PlayerPrefs.SetInt("Credits", credits);
        }

        bestScore = PlayerPrefs.GetInt("Best Score");

    }

    #region Pad Spawning and Incrementing Score

    void Scored()
    {

        if (!startPad)
        {
            Destroy(currentCredit);
            audio.clip = padScore;
            audio.Play();
        }

        ready = true;
        score++;
        scoreText.text = score.ToString();

        Vector3 ballPos = this.transform.position;
        float padSpacer = 1.0f;
        padPos = new Vector3(Random.Range(1.7f, -1.7f), Random.Range(3.1f, -3.1f), 0);

        while (((padPos.x < ballPos.x + padSpacer) && (padPos.x > ballPos.x - padSpacer)) || ((padPos.y < ballPos.y + padSpacer) && (padPos.y > ballPos.y - padSpacer)))
            padPos = new Vector3(Random.Range(1.7f, -1.7f), Random.Range(3.1f, -3.1f), 0);

        int levelScore; // Changes the spawn percentage of pads, based on your score.
        levelScore = 5;
        if (score > levelScore)
        {
            bigPer -= 3;
            mediumPer -= 2;
            smallPer -= 1;
            tinyPer -= 0;
            levelScore += 5;
        }

        ChoosePadSize();
        Instantiate(padSpawn, padPos, Quaternion.identity);

        //RANDOMLY SPAWN A CREDIT -- HAS A 20% CHANCE.
        int i = Random.Range(0, 6);
        if (i == 5)
            SpawnCredit();
    }

    void ChoosePadSize()
    { //Weights the pad size spawn percentage
        if (startPad)
        {
            padSpawn = bigPad;
            startPad = false;
        }
        else
        {
            int z = Random.Range(0, 101);
            if (z <= bigPer)
            {
                padSpawn = bigPad;
            }
            else if (z <= mediumPer)
            {
                padSpawn = mediumPad;
            }
            else if (z <= smallPer)
            {
                padSpawn = smallPad;
            }
            else if (z <= tinyPer)
            {
                padSpawn = tinyPad;
            }
        }
    }

    #endregion

    #region Credit Spawning and Incrementing Credits

    void SpawnCredit()
    {
        Vector3 creditPos;
        Vector3 ballPos = this.transform.position;

        creditPos = padPos;

        GameObject newCredit = Instantiate(creditPrefab, creditPos, Quaternion.identity) as GameObject;
        currentCredit = newCredit;
    }

    void AddCredits()
    {
        credits++;
        PlayerPrefs.SetInt("Credits", credits);
        creditText.text = credits.ToString();
    }

    #endregion

    public void WallBounce(Vector2 bounceDir)
    {
        audio.clip = wallBounce;
        audio.Play();
        this.GetComponent<Rigidbody2D>().AddForce(bounceDir * 15);
        score++;
        scoreText.text = score.ToString();
    }

    public void DeductLives()
    {
        lives--;

        if (lives <= 0)
        {
            lives = 0;
            GameOver();
        }

        livesText.text = lives.ToString();
        if (PlayerPrefs.GetInt("Vibrate") == 1)
        {
            Handheld.Vibrate();
        }
    }

    void GameOver()
    {
        GetComponent<Rigidbody2D>().drag = 9999; // Makes the ball unable to move
        GetComponent<Rigidbody2D>().isKinematic = true;
        PlayerPrefs.SetInt("Score", score);

        if (score > bestScore)
        {
            PlayerPrefs.SetInt("Best Score", score);
        }

        if (!usedRevived && credits > 25)
        {
            StartCoroutine(ReviveTimer(0f, 5f));
        }

        gameOverMenu.SetActive(true);
        gameOverMenu.GetComponent<GameOverScript>().StartGameOverSequence();
        Button pauseButton = GameObject.Find("PauseButton").GetComponent<Button>();
        pauseButton.interactable = false;
#if !UNITY_EDITOR
        PlayGamesScript.AddScordToLeaderboard(GPGSIds.leaderboard_leaderboard_for_golf_pads, score);
#endif
    }

    IEnumerator ReviveTimer(float time, float duration)
    {
        reviveMenu.SetActive(true);

        while (time <= 1f)
        {
            reviveTimer.fillAmount = time;
            time += 0.01f / duration;
            yield return null;
        }

        reviveMenu.SetActive(false);
    }

    public void Revive()
    {
        usedRevived = true;
        Time.timeScale = 1;
        creditText.text = PlayerPrefs.GetInt("Credits").ToString();
        lives = 1;
        livesText.text = lives.ToString();

        gameOverMenu.SetActive(false);
        reviveMenu.SetActive(false);

        GetComponent<Rigidbody2D>().drag = 2; // Makes the ball unable to move
        GetComponent<Rigidbody2D>().isKinematic = false;
    }

    void ChangeBallSprite()
    {
        int BallID = PlayerPrefs.GetInt("Current Ball");

        for (int i = 0; i < ballSprites.Length; i++)
        {
            if (BallID == i)
                GetComponent<SpriteRenderer>().sprite = ballSprites[i];
        }
    }
}

