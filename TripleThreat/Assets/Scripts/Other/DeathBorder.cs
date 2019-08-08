using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//If the player or enemy touches the object that has this script attached to it, kill them.
public class DeathBorder : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //if the player touches this object, kill them and set gameOver to true.
        if (other.gameObject.tag == "Player")
        {
            PlayerHealth.gameOver = true;
            PlayerHealth.playerHealth = 0;
        }

        //if the player touches this object, call their TakeDamage() function and pass in 1000 damage.
        if (other.gameObject.tag == "Enemy")
        {
            EnemyCharacter enemyScript = other.gameObject.GetComponent<EnemyCharacter>();
            enemyScript.TakeDamage(1000, gameObject.transform.position, 0);
        }
    }
}
