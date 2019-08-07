using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//The archer charges up an arrow and then shoots it
public class ArcherAction : MonoBehaviour
{
    public Rigidbody arrowPrefab;
    public Animator bowAnim;
    public GameObject arrow;

    private Transform shootPosition;

    bool arrowCharged;
    [HideInInspector] public bool currentlyCharging;
    [HideInInspector] public bool currentlyAiming;
    bool arrowShot;

    public TextMeshProUGUI textAboveHead;

    void Start()
    {
        shootPosition = gameObject.transform;

        bowAnim.keepAnimatorControllerStateOnDisable = true;
    }

    void OnDisable()
    {
        //Reset everything when character gets swapped
        StopCoroutine("Charge");
        currentlyCharging = false;
        currentlyAiming = false;
        arrowCharged = false;
        textAboveHead.text = "";
        arrow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //If the player holds down Fire1 while an arrow isn't already charged, start charging the arrow.
        if (Input.GetButton("Fire1") && !currentlyCharging && !arrowCharged && !arrowShot)
        {
            StartCoroutine("Charge");
        }
        //If the player releases Fire1 while currently charging but before the arrow is fully charged, stop charging.
        if (Input.GetButtonUp("Fire1") && currentlyCharging && !arrowCharged)
        {
            StopCoroutine("Charge");
            currentlyCharging = false;
            textAboveHead.text = "";
        }


        //If the player presses Fire1 and the arrow is charged, shoot.
        if (Input.GetButtonDown("Fire1") && arrowCharged)
        {
            Shoot();
        }
        //If the player lets go of Fire1 while arrowShot is true, set arrowShot to false.
        //This is so that you can't shoot and immediately charge the next arrow without first releasing the key.
        if (Input.GetButtonUp("Fire1") && arrowShot)
        {
            arrowShot = false;
        }

    }

    //Used to charge the arrow.
    IEnumerator Charge()
    {
        currentlyCharging = true;
        textAboveHead.text = "DRAWING";

        yield return new WaitForSeconds(1f);

        arrowCharged = true;
        currentlyCharging = false;
        currentlyAiming = true;
        arrow.SetActive(true);
        textAboveHead.text = "";
    }

    void Shoot()
    {
        //Play shoot animation
        bowAnim.SetTrigger("Shoot");
        //Disable the arrow that was laying on the bow
        arrow.SetActive(false);

        //Create the arrow instance
        Rigidbody arrowInstance;

        //Instantiate the arrow prefab at shoot position and destroy it after some time.
        arrowInstance = Instantiate(arrowPrefab, shootPosition.position, shootPosition.rotation) as Rigidbody;
        arrowInstance.velocity = shootPosition.forward * 65;
        Destroy(arrowInstance.gameObject, 1f);

        arrowShot = true;
        arrowCharged = false;

        currentlyAiming = false;
    }
}
