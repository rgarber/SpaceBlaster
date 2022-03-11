using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject _powerUpPrefab;
    [SerializeField]
    private GameObject _powerUpContainer;
    [SerializeField]
    private GameObject[] powerups;

    private GameObject _speedBoostPowerUp;

    private bool _stopEnemySpawning = false;

    //private bool _stopSpeedBoostSpawning = false;

    private bool _stopPowerUpSpawning = false;

    //*** extra credit***
    [SerializeField]
    private int numEnemies = 3; // enemies in first wave, add +2 for each wave thereafter
    private int _numEnemyOffset = 2; // offset of enemies after each wave
    private int _waves = 4;  // total # of waves
    public int _wavesLevel; // current wave
    private bool nextWave = true; // signal for next wave true = yes/ false = no
    [SerializeField]
    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        StartCoroutine(SpawnPowerUpRoutine());
 
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void StartWaves()
    {
         StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(4);
        for (int w = 1; w < _waves; w++)
        {
            _wavesLevel = w;

            if (_wavesLevel == 4)
            {
                break;
            }
         
            _uiManager.DisplayWaveOn();
            yield return new WaitForSeconds(4.0f);
            _uiManager.DisplayWaveOff();

            
            for (int i = 1; i < numEnemies +1; i++)
            {
                yield return new WaitForSeconds(3);
                Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
                GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity); // Instantiate enemy prefab
                newEnemy.transform.parent = _powerUpContainer.transform;
            }
            numEnemies = numEnemies + _numEnemyOffset; // increase # of enemies for each wave
        }
        LastWave();
    }

    void LastWave()
    {
        _stopPowerUpSpawning = true;    // stops powerups from spawning
        DestroyLeftOvers();             // Cleans screen of enemies at end of game
        _uiManager.YouWon();            // Displays !!! You Won !!!
        _uiManager.GameOverSequence();  // intiates Game Over R to restart
    }

    void DestroyLeftOvers()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Enemy>().ClearEnemy();
        }

    }
    
    /*IEnumerator SpawnEnemyRoutine_org()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopEnemySpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity); // Instantiate enemy prefab
            newEnemy.transform.parent = _powerUpContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }

    }*/

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(4.0f);
        while (_stopPowerUpSpawning == false)
        {
                
            Vector3 postToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int powerupuse = default;
            int randomPowerUp = Random.Range(1,100); // *** Spawn powerUp range ***

            if(randomPowerUp > 0 && randomPowerUp <= 15 )
            {
                powerupuse = 0;                                     // TripleShot = 15%
            } else if (randomPowerUp >= 16 && randomPowerUp <= 26)
            {
                powerupuse = 1;                                     // SpeedBoost = 11%
            }
            else if (randomPowerUp >= 27 && randomPowerUp <= 38)
            {
                powerupuse = 2;                                     // Shields = 38%
            }
            else if (randomPowerUp >= 39 && randomPowerUp <= 58)
            {
                powerupuse = 3;                                     // Ammo = 20%
            }else if (randomPowerUp >= 59 && randomPowerUp <= 73)
            {
                powerupuse = 4;                                     // Health = 15%
            }else if (randomPowerUp >= 74 && randomPowerUp <= 85)
            {
                powerupuse = 5;                                     // Nukem = 12%
            }else if (randomPowerUp >= 85 && randomPowerUp <= 100)
            {
                powerupuse = 6;                                     // Bomb = 15%
            }
            
            Instantiate(powerups[powerupuse], postToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3,8));

        }

    }

    public void OnPlayerDeath()
    {
        _stopEnemySpawning = true;
        _stopPowerUpSpawning = true;
        
    }

}
