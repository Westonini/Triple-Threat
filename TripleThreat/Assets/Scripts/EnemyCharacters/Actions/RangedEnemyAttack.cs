using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAttack : MonoBehaviour
{
    [HideInInspector] public PlayerCharacter playerHit;
    public Rigidbody projectilePrefab;
    public GameObject projectile;
    [Range(0, 18)] public float maxRangeFromPlayer;
    public Animator weaponAnim;
    public string weaponReadySound;

    private Transform shootPosition;

    public float timeUntilChargingFinishes;
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
            weaponAnim.SetBool("Charge", false);
        }
    }

    //Fire arrow at player
    IEnumerator Fire()
    {
        //Set currentlyDrawing to true
        currentlyDrawing = true;

        //Wait a bit until starting the charging animation
        yield return new WaitForSeconds(0.25f);

        //charging animation
        projectile.SetActive(true);
        weaponAnim.SetBool("Charge", true);

        //Wait for timeUntilShowProjectile seconds...
        yield return new WaitForSeconds(timeUntilChargingFinishes);

        //Stop the charging animation
        weaponAnim.SetBool("Charge", false);
        //Play the weapon ready sound
        FindObjectOfType<AudioManager>().Play(weaponReadySound, transform);

        //Wait for castTime - timeUntilShowProjectile seconds...
        yield return new WaitForSeconds(castTime - timeUntilChargingFinishes);

        //Play weapon shoot animation
        weaponAnim.SetTrigger("Shoot");
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
