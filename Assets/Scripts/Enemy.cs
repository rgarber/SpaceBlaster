using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 3;  // Enemy moves 4 units down per second
    [SerializeField]
    private GameObject _laserPrefab;

    private Player _player;

    // handle to animator component
    private Animator _anim;

    //handle to audio source
    private AudioSource _audioSource;

    private float _fireRate = 4.0f; // default enemy fire rate (laser)
    private float _canFire = -1;
    // ** extra credit **

    private UIManager _uiManager;

    // *** extra credit ***
    private int _jitterbugchance;
    private bool _jitterbug = false;
    private bool _crippled = false;
    private int _eoffset;
    private bool _enemydead = false;


    [SerializeField]
    private GameObject _enemyLaserPrefab;

    
    [SerializeField]
    private GameObject _EnemyShield;
    [SerializeField]
    private SpawnManager _spawnmanager;
    private bool _isenemyshieldactive = false;
    
    [SerializeField]
    private GameObject _enemyRadar;
    [SerializeField]
    private GameObject _radarglow;
    private EnemyRadar _radar;
    private bool _isEnemyRadarActive;
    

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _spawnmanager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _radar = _enemyRadar.GetComponent<EnemyRadar>();
        
        
        int randomradar = Random.Range(1, 100);     // routine to give out radar to enemy 15%
        if (randomradar < 10)
        {
            _isEnemyRadarActive = true;
            RadarActivation();
        }


        int randomEShield = Random.Range(1, 100); // routine to give out shields to enemy - 15%
        if (randomEShield < 20)
        {
            _isenemyshieldactive = true;
            ShieldActivation();
        }

        if (_player == null)
        {
            Debug.LogError("The player is NULL.");
        }
        
        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("The animator is NULL.");
        }

        _jitterbugchance = Random.Range(1, 100);           // decides by random if this enemy jitterbugs or not. > 14 is yes. 

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && _player._hasNuke == 1)
        {
            _uiManager.HideNuke();
            Nuke_Enemies();
            _uiManager.WhoaText();
            Nuke_Enemies2();
        }

        CalculateMovement();
        
        if (Time.time > _canFire)                   // intial Enemy fire routine
        {
            _fireRate = Random.Range(5f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemylaser = Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0,-1f,0), Quaternion.identity);

            enemylaser.GetComponent<Laser>().AssignEnemyLaser();
        }


    }

    void CalculateMovement()                        // enemy movement also calls jitterbug
    {
        if (_isEnemyRadarActive == true && _radar.evasiveManeuver == true)
        {
            StartCoroutine(ReverseJitterbug());
        }

        else if (_jitterbugchance > 95 && transform.position.y < 4 && _jitterbug == false && _crippled == false)  // if enemy is at 5 or less, give it a chance to jitterbug (move to either side)
        {
            StartCoroutine(Jitterbug());
        }

 
        else if (_jitterbug == true && _crippled == false)
        {
            transform.Translate(Vector3.right * _eoffset * _enemySpeed * Time.deltaTime);
            transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
         
        }

        else transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y < -5 && _crippled == false)  // respawn to top
        {
            float randomX = Random.Range(-9, 9);
            transform.position = new Vector3(randomX, 7, 0);
        }

    }

    IEnumerator Jitterbug()                         // shift enemy left or right
    {
        if (_enemydead == true)
        {
            yield break;
        }
        _enemySpeed = 1.5f;
        _jitterbug = true;
        _eoffset = Random.Range(-1, 2);   // horizontal shift to offset enemy
        yield return new WaitForSeconds(0.5f);
        _jitterbug = false;
        _enemySpeed = 3f;
   
    }

    IEnumerator ReverseJitterbug()                         // happens if radar detects incoming
    {
        _jitterbug = true;
        _eoffset = Random.Range(-4, 3);   // extreme horizontal shift to offset enemy
        _enemySpeed = -1.5f;
        _radar.evasiveManeuver = false;
        yield return new WaitForSeconds(.35f);
        _enemySpeed = 3f;               // reset enemy parameters after evasive maneuver
        _jitterbug = false;



    }

    private void OnTriggerEnter2D(Collider2D other) // Enemy damage (includes enemy shield)
    {
        if (other.tag == "Player")                  // Enemy damage with collision to player
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();                    // Damage the player if collision
            }
            _anim.SetTrigger("OnEnemyDeath");       // trigger enemy explodes anim
            _enemySpeed = 0;                        // stop enemy for explosion animation
            _enemydead = true;                      // to stop all movt of the enemy
            _audioSource.Play();
            EnemyShieldVisualizerOff();             // stops enemy shield display
            RadarShutDown();                        // shuts down enemy radar
            Destroy(GetComponent<Collider2D>());   
            Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Laser")                   // Enemy damage caused by laser
        {
            Destroy(other.gameObject);              // destroy laser (other)
            
            
            if (_isenemyshieldactive == false)      // enemy destroyed, shield must be down
            {
                _anim.SetTrigger("OnEnemyDeath");       // trigger enemy explodes anim
                _enemySpeed = 0;                        // stop enemy for explosion animation
                _enemydead = true;                      // to stop all movt of the enemy
                _audioSource.Play();
                _player.AddScore(10);
                RadarShutDown();                        // shuts down enemy radar glow
                Destroy(GetComponent<Collider2D>());    // Why this line??
                Destroy(this.gameObject, 2.8f);
            }

            if (_isenemyshieldactive == true)
            {
                EnemyShieldVisualizerOff();
                _isenemyshieldactive = false;
                _audioSource.Play();
                _crippled = true;
                _enemySpeed = -1f;
            }
        }

    }

    public void Nuke_Enemies()
    {
        _anim.SetTrigger("OnEnemyDeath"); // trigger enemy explodes anim
        _enemySpeed = 0; // stop enemy for explosion animation
        _audioSource.Play();
        EnemyShieldVisualizerOff();
        _isenemyshieldactive = false;
        RadarShutDown();
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 2.8f);
        _player.AddScore(10);
    }

    public void Nuke_Enemies2 ()
    {
        StartCoroutine(NukePause());
    }

    IEnumerator NukePause()
    {
        yield return new WaitForSeconds(2);
        _player._hasNuke = 0;
    }

    public void ClearEnemy()
    {
        Destroy(this.gameObject);
    }

    public void ShieldActivation()
    {
        _EnemyShield.SetActive(true); //set the enemy shield child for enemy to true (shows shield aura object)
    }

    public void EnemyShieldVisualizerOff()
    {
        _EnemyShield.SetActive(false); 
    }

    public void RadarActivation()
    {
        _enemyRadar.SetActive(true); //set the enemy radar child to true
        _radarglow.SetActive(true);  //set the enemy radar glow to on
    }

    public void RadarShutDown()
    {
        _enemyRadar.SetActive(false); //set the enemy detector to off
        _radarglow.SetActive(false);  //set the enemy radar glow to off
    }
}


