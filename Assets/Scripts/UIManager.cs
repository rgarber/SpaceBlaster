using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    
    // Start is called before the first frame update

    [SerializeField]
    private Text _ScoreText;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restart_Text;

    private GameManager _gameManager;


    // *** extra credit additions
    [SerializeField]
    private Text _speedShiftA; // display Speed Boost when activated
    [SerializeField]
    private Text _AmmoText; // display ammo count left
    [SerializeField]
    private Text _whoaText; // for displaying Whoa!
    [SerializeField]
    private Text _waveText; // for display wave #
    [SerializeField]
    private Text _youWon;   // for display You Won!
    
    private SpawnManager _spawnManager;

    // *** Extra Credit ***
    [SerializeField]
    private Image _nukeDisplay;
    


    // Start is called before the first frame update
    void Start()
    {
        _AmmoText.text = "Ammo = " + 15;
        _ScoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        _nukeDisplay.enabled = false;

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn_Manager is NULL");
        }

        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL");

        }

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.LeftShift)) // display Speed Boost text
        {
            _speedShiftA.gameObject.SetActive(true);
        }
        else _speedShiftA.gameObject.SetActive(false);

    }
 
    public void UpdateScore(int playerScore)
    {
        _ScoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
          GameOverSequence();
        }

    }

    public void GameOverSequence()
    {
        _gameManager.GameOver(); 
        _gameOverText.gameObject.SetActive(true);
        _restart_Text.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());

    }

    IEnumerator GameOverFlickerRoutine()
    {
        
        while (true)
        {
            _gameOverText.text = "GAME OVER!";
            yield return new WaitForSeconds(0.35f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.35f);

        }
    }

    public void UpdateAmmoCount(int ammoCount)
    {
        _AmmoText.text = "Ammo = " + ammoCount.ToString();
    }

    public void WhoaText()
    {
        StartCoroutine(NukePause());
    }

    IEnumerator NukePause()
    {
        _whoaText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        _whoaText.gameObject.SetActive(false);
    }
    
    public void DisplayWaveOn()
    {
        _waveText.gameObject.SetActive(true);
        _waveText.text = "Wave " + _spawnManager._wavesLevel;
    }
    
    public void DisplayWaveOff()
    {
        _waveText.gameObject.SetActive(false);
    }

    public void YouWon()
    {
        _youWon.gameObject.SetActive(true);
    }

    public void ShowNuke()
    {
        _nukeDisplay.enabled = true;
    }

    public void HideNuke()
    {
        _nukeDisplay.enabled = false;
        
    }
}

