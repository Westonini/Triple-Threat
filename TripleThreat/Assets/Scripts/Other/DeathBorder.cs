using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBorder : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            PlayerHealth.gameOver = true;
            PlayerHealth.playerHealth = 0;
        }

        if (other.gameObject.tag == "Enemy")
        {
            EnemyCharacter enemyScript = other.gameObject.GetComponent<EnemyCharacter>();
            enemyScript.TakeDamage(1000, gameObject.transform.position, 0);
        }
    }
}
