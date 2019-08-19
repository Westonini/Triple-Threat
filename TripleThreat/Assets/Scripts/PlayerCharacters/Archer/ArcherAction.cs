using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//The archer charges up an arrow and then shoots it
public class ArcherAction : MonoBehaviour
{
    public Rigidbody arrowPrefab;
    public Animator bowAnim;
    public GameObject arrow; public GameObject arrow2; public GameObject arrow3;

    private Transform shootPosition;

    bool arrowCharged;
    bool multiArrowCharged;
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
        StopCoroutine("ChargeSingle");
        StopCoroutine("ChargeMulti");
        StopCoroutine("MultiShoot");
        currentlyCharging = false;
        currentlyAiming = false;
        arrowCharged = false;
        multiArrowCharged = false;
        textAboveHead.text = "";
        arrow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //CHARGE ARROW(S)
        //If the player holds down Fire1 (Single arrow) or Fire2 (Three arrows) while the arrow(s) isnt't/aren't already charged, start charging the arrow(s).
        if (!currentlyCharging && !arrowCharged && !multiArrowCharged && !arrowShot)
        {
            if (Input.GetButton("Fire1"))
            {
                StartCoroutine("ChargeSingle");
            }
            else if (Input.GetButton("Fire2"))
            {
                StartCoroutine("ChargeMulti");
            }
        }
        //If the player releases Fire1 while currently charging but before the arrow is fully charged, stop charging.
        if ((Input.GetButtonUp("Fire1") || Input.GetButtonUp("Fire2")) && currentlyCharging && !arrowCharged && !multiArrowCharged)
        {
            StopCoroutine("ChargeSingle");
            StopCoroutine("ChargeMulti");
            bowAnim.SetBool("Draw", false);
            bowAnim.SetBool("Draw2", false);
            textAboveHead.text = "";
            arrow.SetActive(false); arrow2.SetActive(false); arrow3.SetActive(false);
            currentlyCharging = false;
        }


        //SHOOT ARROW(S)
        //If the player presses Fire1/Fire2 and the arrow(s) is/are charged, shoot.
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
        {
            if (arrowCharged)
            {
                Shoot();
            }
            else if (multiArrowCharged)
            {
                StartCoroutine("MultiShoot");
            }
        }
        //If the player lets go of or presses Fire1/Fire2 while arrowShot is true, set arrowShot to false.
        //This is so that you can't shoot and immediately charge the next arrow without first releasing the key.
        if ((Input.GetButtonUp("Fire1") || Input.GetButtonUp("Fire2") || Input.GetButtonDown("Fire2")) && arrowShot && !currentlyAiming)
        {
            arrowShot = false;
        }

    }

    //Used to charge the arrow.
    IEnumerator ChargeSingle()
    {
        //Drawing the bow
        currentlyCharging = true;
        textAboveHead.text = "DRAWING";

        //Play draw animation
        arrow.SetActive(true);
        bowAnim.SetBool("Draw", true);

        yield return new WaitForSeconds(1.25f); //Bow draw time

        //Ready to fire
        bowAnim.SetBool("Draw", false);
        FindObjectOfType<AudioManager>().Play("ArrowReady");
        arrowCharged = true;
        currentlyCharging = false;
        currentlyAiming = true;
        textAboveHead.text = "";
    }

    IEnumerator ChargeMulti()
    {
        //Drawing the bow
        currentlyCharging = true;
        textAboveHead.text = "DRAWING";

        //Play draw animation
        arrow.SetActive(true); arrow2.SetActive(true); arrow3.SetActive(true);
        bowAnim.SetBool("Draw2", true);

        yield return new WaitForSeconds(0.833f); //Arrow1 ready
        FindObjectOfType<AudioManager>().Play("ArrowReady");
        yield return new WaitForSeconds(0.833f); //Arrow2 ready
        FindObjectOfType<AudioManager>().Play("ArrowReady");
        yield return new WaitForSeconds(0.833f); //Arrow3 ready
        FindObjectOfType<AudioManager>().Play("ArrowReady");

        //Ready to fire
        bowAnim.SetBool("Draw2", false);
        multiArrowCharged = true;
        currentlyCharging = false;
        currentlyAiming = true;
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

    IEnumerator MultiShoot()
    {
        //Shoot Arrow1
        //Play shoot animation
        bowAnim.SetTrigger("Shoot");
        //Disable the arrow that was laying on the bow
        arrow.SetActive(false);
        //Create the arrow instance
        Rigidbody arrowInstance;
        //Instantiate the arrow prefab at shoot position and destroy it after some time.
        arrowInstance = Instantiate(arrowPrefab, arrow.transform.position, arrow.transform.rotation) as Rigidbody;
        arrowInstance.velocity = shootPosition.forward * 65;
        Destroy(arrowInstance.gameObject, 1f);

        multiArrowCharged = false;
        arrowShot = true;

        yield return new WaitForSeconds(0.1f);

        //Shoot Arrow2
        //Play shoot animation
        bowAnim.SetTrigger("Shoot");
        //Disable the arrow that was laying on the bow
        arrow2.SetActive(false);
        //Create the arrow instance
        Rigidbody arrowInstance2;
        //Instantiate the arrow prefab at shoot position and destroy it after some time.
        arrowInstance2 = Instantiate(arrowPrefab, arrow2.transform.position, arrow2.transform.rotation) as Rigidbody;
        arrowInstance2.velocity = shootPosition.forward * 65;
        Destroy(arrowInstance2.gameObject, 1f);

        yield return new WaitForSeconds(0.1f);

        //Shoot Arrow3
        //Play shoot animation
        bowAnim.SetTrigger("Shoot");
        //Disable the arrow that was laying on the bow
        arrow3.SetActive(false);
        //Create the arrow instance
        Rigidbody arrowInstance3;
        //Instantiate the arrow prefab at shoot position and destroy it after some time.
        arrowInstance3 = Instantiate(arrowPrefab, arrow3.transform.position, arrow3.transform.rotation) as Rigidbody;
        arrowInstance3.velocity = shootPosition.forward * 65;
        Destroy(arrowInstance3.gameObject, 1f);

        currentlyAiming = false;
    }
}
