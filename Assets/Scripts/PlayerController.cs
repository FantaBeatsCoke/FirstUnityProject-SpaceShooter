using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
public class PlayerController : MonoBehaviour
{
    private float timePassed=0f;
    public float thrust = 1f;
    public float maxSpeed = 5f;
    private float score = 0f;
    public float scoreMultiplier = 10f;
    public GameObject boosterFlame;
    Rigidbody2D rb;
    public UIDocument uiDocument;
    private Label scoreText;
    public GameObject explosionEffect;
    private Button restartButton;
    private static float oldTopScore = 0f;
    private Label highScore;
    private int secondTimePassed;
    private float oldScore;
    public int howManyFramesPerSwitchColor=5;
    private bool alive;
    private bool greenNow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        alive = true;
        greenNow = false;
        oldScore = oldTopScore;
        rb=GetComponent<Rigidbody2D>();
        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        restartButton= uiDocument.rootVisualElement.Q<Button>("RestartButton");
        highScore = uiDocument.rootVisualElement.Q<Label>("HighScore");
        highScore.style.backgroundColor = Color.black;
        highScore.style.color = Color.white;

        highScore.style.borderTopWidth = 2;
        highScore.style.borderBottomWidth = 2;
        highScore.style.borderLeftWidth = 2;
        highScore.style.borderRightWidth = 2;

        highScore.style.borderTopColor = Color.white;
        highScore.style.borderBottomColor = Color.white;
        highScore.style.borderLeftColor = Color.white;
        highScore.style.borderRightColor = Color.white;

        highScore.style.paddingTop = 5;
        highScore.style.paddingBottom = 5;
        highScore.style.paddingLeft = 10;
        highScore.style.paddingRight = 10;

        highScore.style.display = DisplayStyle.None;

        restartButton.style.display = DisplayStyle.None;
        restartButton.clicked += ReloadScene;
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            timePassed += Time.deltaTime;
            score = Mathf.FloorToInt(timePassed * scoreMultiplier);
            scoreText.text = "Score: " + score;
            if (Mouse.current.leftButton.isPressed)
            {

                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);

                Vector2 direction = (mousePos - transform.position).normalized;
                transform.up = direction;
                rb.AddForce(direction * thrust);
                if (rb.linearVelocity.magnitude > maxSpeed)
                {
                    rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;

                }
            }
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                boosterFlame.SetActive(true);
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                boosterFlame.SetActive(false);
            }
        }
        if (!alive && oldTopScore > oldScore)
        {
            highScore.text = "New High Score:" + oldTopScore;
            secondTimePassed++;

            if (secondTimePassed % howManyFramesPerSwitchColor == 0)
            {
                greenNow = !greenNow;

                if (greenNow)
                {
                    highScore.style.color = Color.green;
                    highScore.style.borderTopColor = Color.green;
                    highScore.style.borderBottomColor = Color.green;
                    highScore.style.borderLeftColor = Color.green;
                    highScore.style.borderRightColor = Color.green;
                }
                else
                {
                    highScore.style.color = Color.white;
                    highScore.style.borderTopColor = Color.white;
                    highScore.style.borderBottomColor = Color.white;
                    highScore.style.borderLeftColor = Color.white;
                    highScore.style.borderRightColor = Color.white;
                }
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        alive = false;
        rb.simulated = false;
        foreach (var r in GetComponentsInChildren<SpriteRenderer>())
        {
            r.enabled = false;
        }
        Instantiate(explosionEffect, transform.position, transform.rotation);
        restartButton.style.display = DisplayStyle.Flex;
        if(score>oldTopScore)
        {
            oldTopScore = score;
        }
        highScore.text= "High Score:" +oldTopScore;
        highScore.style.display = DisplayStyle.Flex;
   
    }
    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
