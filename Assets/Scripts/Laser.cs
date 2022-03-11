using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _espeed = 3.25f;  // enemy laser speed = 4
    [SerializeField]
    private float _speed = 8.0f;   // friendly laser speed = 8

    private bool _isEnemyLaser = false;
    


    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser == true)        
        {
            MoveDown();
        }

        if (_isEnemyLaser == false)
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);

            if (transform.position.y > 8f)
            {
                if (transform.parent != null) // check to see if object has a parent
                {
                    Destroy(transform.parent.gameObject);
                }
                Destroy(this.gameObject);
            }
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _espeed * Time.deltaTime);
        if (transform.position.y < -10f)
        {
            if (transform.parent != null) // check to see if object has a parent
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }   
   

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
        }
    }
}
