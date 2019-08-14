using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAttack : MonoBehaviour
{
    [HideInInspector] public PlayerCharacter playerHit;
    public Rigidbody projectilePrefab;
    public GameObject projectile;
    [Range(0, 16)] public float maxRangeFromPlayer;
    private Transform shootPosition;

    public float timeUntilShowProjectile;
    public float cooldownTime = 2.5f;

    RangedEnemy enemyScript;

    float castTime;
    float distanceFromPlayer;
    [HideInInspector] public bool playerWithinRange;

    bool currentlyDrawing;

    void Start()
    {
        //Get RangedEnemy script from parent
        enemyScript = GetComponentInParent<RangedEnemy>();

        //Get Shoot position
        shootPosition = GetComponent<Transform>();

        //Get castTime that was set in parent script.
        castTime = enemyScript.castTime;
    }

    void Update()
    {
        DistanceFromPlayer();

        if (playerWithinRange && !currentlyDrawing)
        {
            StartCoroutine("Fire");
        }

    }

    void DistanceFromPlayer()
    {
        //Calculate the distance between this character and the player character
        distanceFromPlayer = Vector3.Distance(SwapCharacters.currentCharacter.transform.position, transform.position);

        //If this enemy and player are within (maxRangeFromPlayer) units of eachother, set playerWithinRange to true. Otherwise set it false and stop any current drawing.
        if (distanceFromPlayer <= maxRangeFromPlayer)
        {
            playerWithinRange = true;
        }
        else
        {
            playerWithinRange = false;

            StopCoroutine("Fire");
            currentlyDrawing = false;
            projectile.SetActive(false);
        }
    }

    //Fire arrow at player
    IEnumerator Fire()
    {
        //Set currentlyDrawing to true
        currentlyDrawing = true;

        //Wait for timeUntilShowProjectile seconds...
        yield return new WaitForSeconds(timeUntilShowProjectile);

        //If the projectile gameobject in the inspector isn't null, set it to true.
        if (projectile != null)
        {
            projectile.SetActive(true);
        }

        //Wait for castTime - timeUntilShowProjectile seconds...
        yield return new WaitForSeconds(castTime - timeUntilShowProjectile);

        //Turn off the projectile that's for show.
        projectile.SetActive(false);

        //Create the arrow instance
        Rigidbody projectileInstance;

        //Instantiate the arrow prefab at shoot position and destroy it after some time.
        projectileInstance = Instantiate(projectilePrefab, shootPosition.position, shootPosition.rotation) as Rigidbody;
        projectileInstance.transform.parent = enemyScript.gameObject.transform;
        projectileInstance.velocity = shootPosition.forward * 65;
        Destroy(projectileInstance.gameObject, 1f);

        //Wait some time until firing again.
        yield return new WaitForSeconds(cooldownTime);

        //Set the currentlyDrawing boolean to false
        currentlyDrawing = false;
    }
}
