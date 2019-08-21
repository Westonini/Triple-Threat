using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//If the player or enemy touches the object that has this script attached to it, kill them.
public class DeathBorder : MonoBehaviour
{
    public GameObject smokeParticles;

    private void OnTriggerEnter(Collider other)
    {
        //if the player touches this object and isn't already dead...
        if (other.gameObject.tag == "Player" && PlayerHealth.playerHealth > 0)
        {
            //Find all the playerCharacters and add them to an array.
            GameObject[] playerCharacters = GameObject.FindGameObjectsWithTag("Player");

            //Disable all objects tagged "Player"
            for (var i = 0; i < playerCharacters.Length; i++)
            {
                playerCharacters[i].SetActive(false);
            }

            //Instantiate smoke particles and set player health to 0.
            InstantiateParticles.InstantiateParticle(SwapCharacters.currentCharacter.transform, smokeParticles, 2f, 0.25f);
            PlayerCharacter playerScript = other.gameObject.GetComponent<PlayerCharacter>();
            playerScript.TakeDamage(10, gameObject.transform.position); //PlayerDeath will check if player is already inactive, in which case the scene will restart faster.
        }

        //if the player touches this object, call their TakeDamage() function and pass in 1000 damage.
        if (other.gameObject.tag == "Enemy")
        {
            //Instantiate smoke particles.
            InstantiateParticles.InstantiateParticle(other.gameObject.transform, smokeParticles, 2f, 0.25f);

            EnemyCharacter enemyScript = other.gameObject.GetComponent<EnemyCharacter>();
            enemyScript.TakeDamage(1000, gameObject.transform.position, 0);
            other.gameObject.SetActive(false);
        }
    }
}
