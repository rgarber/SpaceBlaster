using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 3.0f;
    [SerializeField]
    private GameObject _explosionPrefab;

    private SpawnManager _spawnmanager;

    private void Start()
    {
        _spawnmanager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        
    }



    // Update is called once per frame
    void Update()
    {

        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);   

    }

    // check for Laser collision (Trigger)
    // instantiate explosion at the position of the asteriod
    // destroy explosion after 3 seconds

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnmanager.StartWaves();
            Destroy(this.gameObject, .25f);
        }

        
    }



}
