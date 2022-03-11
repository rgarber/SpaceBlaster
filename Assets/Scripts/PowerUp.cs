using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _allPowerUpSpeed = 2.5f; // all powerups have the same speed

    
    [SerializeField]
    private int PowerupID; // 0 = Triple Shot  1 = Speed Boost   2 = Shield powerup activated 4 = Ammo
    [SerializeField]
    private AudioClip _clip;

    // *** extra credit ***
    
    

    

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.down * _allPowerUpSpeed * Time.deltaTime);
        
        if (transform.position.y <= -7f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if (player != null)
            {

                switch(PowerupID)
                {

                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldBoostActive();
                        break;
                    case 3:
                        player.AmmoResupply();
                        break;
                    case 4:
                        player.HealthResupply();
                        break;
                    case 5:
                        player.Nukem_all();
                        break;
                    case 6:
                        player.BombsAway();
                        break;
                    default:
                        break;


                }
                
            }

            Destroy(this.gameObject);
        }

    }
 
}
