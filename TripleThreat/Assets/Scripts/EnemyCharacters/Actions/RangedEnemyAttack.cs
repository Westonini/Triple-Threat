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
    private LayerMask wallLayer;
    [HideInInspector] public bool wallHit;

    bool currentlyDrawing;

    void Start()
    {
        //Get RangedEnemy script from parent
        enemyScript = GetComponentInParent<RangedEnemy>();

        //Get Shoot position
        shootPosition = GetComponent<Transform>();

        //Get castTime that was set in parent script.
        castTime = enemyScript.castTime;

        //Get wall layer
        wallLayer = LayerMask.GetMask("Wall");
    }

    void Update()
    {
        //Calculates the distance between this character and the player character
        DistanceFromPlayer();

        //If the player's character is within range...
        if (playerWithinRange)
        {
            //Check to see if there's a wall object in the way.
            CheckForWall();

            if (!wallHit && !currentlyDrawing) //If there's no wall object in the way, start the Fire coroutine
            {
                StartCoroutine("Fire");
            }
            else if (wallHit && currentlyDrawing) //If there's a wall in the way while the character is currentlyDrawing, stop the Fire coroutine
            {
                StopCharging();
            }
        }
    }

    void DistanceFromPlayer()
    {
        //Calculate the distance between this character and the player character
        distanceFromPlayer = Vector3.Distance(SwapCharacters.currentCharacter.transform.position, transform.position);

        //If this enemy and player are within (maxRangeFromPlayer) units of eachother, set playerWithinRange to true. Otherwise set it false and stop any current drawing.
        if (distanceFromPlayer <= maxRangeFromPlayer)
        {
            StartCoroutine(FocusOnTarget("playerWithinRange")); //Set playerWithinRange to true if the player has been within range for half a second.          
        }
        else
        {
            StopCoroutine("FocusOnTarget");
            StopCharging();
        }
    }

    void CheckForWall()
    {
        RaycastHit hit;

        //Turn this on if you want to see the ray (only viewable in scene view with gizmos on)
        //Debug.DrawRay(shootPosition.position, shootPosition.forward * distanceFromPlayer, Color.yellow);

        //Check if this character is currently aiming at a wall
        if (Physics.Raycast(shootPosition.position, shootPosition.forward, out hit, distanceFromPlayer, wallLayer))
        {
            wallHit = true;
            StopCoroutine("FocusOnTarget");
        }
        else
        {
            StartCoroutine(FocusOnTarget("wallHit"));
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

    //This checks playerWithinRange and wallHit for half a second longer just so that the movement and animations aren't janky and are more accurate.
    IEnumerator FocusOnTarget(string boolCheck)
    {
        yield return new WaitForSeconds(0.5f);

        if (boolCheck == "playerWithinRange")
        {
            playerWithinRange = true;
        }
        else
        {
            wallHit = false;
        }
    }

    void StopCharging()
    {
        playerWithinRange = false;

        StopCoroutine("Fire");
        currentlyDrawing = false;
        projectile.SetActive(false);
        weaponAnim.SetBool("Charge", false);
    }
}
