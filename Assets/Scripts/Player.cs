using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _speedBoostPrefab;
  
    [SerializeField]
    private Vector3 _offset = new Vector3(0f, 0.8f, 0f); // laser offset of y + .8f
    [SerializeField]
    private float _fireRate = 0.5f; // length of pause of fire
    private float _canFire = -1f; // is the laser cool enough to fire

    [SerializeField]
    private float _canFirePowerUp = 5f; // length of PowerUP fire = 5 seconds

    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isShieldBoostActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private GameObject _rightEngine;

    [SerializeField]
    private int _score = 0;
    private UIManager _uiManager;

    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioSource _audioSource;

    // *** Extra Credit ***/// 
    [SerializeField]
    private int _speedShiftAmt = 3; // add +3 to speed for player
    // *** Extra Credit ***/// Ammo Count
    [SerializeField]
    private int _ammocount = 15;
    [SerializeField]
    private int _healthsupply = 3;
    
    public int _hasNuke = 0;

    [SerializeField]
    private CameraShake _cameraRock;
         
    

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, -3.0f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _cameraRock = GameObject.Find("Main Camera").GetComponent<CameraShake>();
       
                
        
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is Null");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is Null");

        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the player is NULL!");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            //LoadMenu();
            Debug.Log("M key is pressed");
        }

        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)  // where the value of _canFire is measured against Time.time and...
                                                                      // if _canFire is greater than the time in the game it's okay to fire
        {
            FireLaser();
        }
    }


    void CalculateMovement()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (Input.GetKey(KeyCode.LeftShift)) // extra credit add speedboost to player movt with left shift key
        {                                        // held down.
            transform.Translate(direction * (_speed + _speedShiftAmt) * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }


        if (transform.position.x > 11.3f)
        {
            (transform.position) = new Vector3(-11.3f, transform.position.y, 0);

        }
        else if (transform.position.x < -11.3f)
        {
            (transform.position) = new Vector3(11.3f, transform.position.y, 0);

        }

    }

    void FireLaser()
    {
        if (_ammocount > 0)
        {
            _ammocount = _ammocount - 1;
        }
        else return;

        _canFire = Time.time + _fireRate;  // canFire is the value of time in game + the fireRate...

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity); //fire one laser from nose

        _audioSource.Play();

        _uiManager.UpdateAmmoCount(_ammocount);

    }

    public void Damage()
    {
 
        if (_isShieldBoostActive == true)
        {
            _shieldVisualizer.SetActive(false);
            _isShieldBoostActive = false;
            return;
        }
        else _lives -= 1;

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }
        
        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }

        _cameraRock.CameraShakeStart();



    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;   // set triple shot to true
        StartCoroutine(TripleShotPowerDownRoutine()); // start 5 sec countdown routine (TripleShotPowerDownRoutine

    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _speed = 8.5f;           // setting _speed of player to 8.5 

        StartCoroutine(SpeedBoostActiveCountDownRoutine()); //Start 5 sec countdown routine SpeedBoostActiveCountDownRoutine()

    }
        
    IEnumerator SpeedBoostActiveCountDownRoutine()
    {

        yield return new WaitForSeconds(5.0f);
        
        _speed = 3.5f; // returning speed of player to 3.5

    }

    public void ShieldBoostActive()
    {
        _isShieldBoostActive = true; //set shields to active!
        _shieldVisualizer.SetActive(true); //set the shield child to Player to true (shows shield aura object)
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void AmmoResupply()
    {
        _ammocount = 15;
        _uiManager.UpdateAmmoCount(_ammocount);
    }

    public void HealthResupply()
    {
        
        if (_lives < 3)
        {
            _lives += 1;
            _uiManager.UpdateLives(_lives);
        }
    }

    public void Nukem_all()
    {
        _hasNuke = 1;
        _uiManager.ShowNuke();
        
    }

    public void BombsAway()
    {

        _ammocount = _ammocount - 7;

        if (_ammocount < 0)
        {
            _ammocount = 0;
        }
        _uiManager.UpdateAmmoCount(_ammocount);
        _cameraRock.CameraShakeStart();
    }


}

        
    








