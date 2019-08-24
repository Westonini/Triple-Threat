using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This script will always move text to be where the parent object is. This enemy version will instantiate a text gameobject onto the canvas when the enemy is spawned
public class EnemyMoveTextWithParent : MonoBehaviour
{
    private Camera mainCam;
    private TextMeshProUGUI infoText;

    [HideInInspector] public GameObject enemyTextAboveHeadInstance;
    public GameObject enemyTextAboveHeadPrefab;
    private string enemyName;
    private int enemyMaxHealth;

    private EnemyCharacter enemyScript;

    void Start()
    {
        //Get the camera tagged "CameraMain"
        mainCam = Camera.main;
        //Get the enemy script from the parent object
        enemyScript = GetComponentInParent<EnemyCharacter>();
        //Get enemy's name from parent object
        enemyName = transform.parent.name;
        //Get enemy's starting health amount
        enemyMaxHealth = enemyScript.health;



        //Instantiate text gameobject and set its parent to "TextAboveHead - Enemies" 
        enemyTextAboveHeadInstance = Instantiate(enemyTextAboveHeadPrefab, transform.position, transform.rotation) as GameObject;
        enemyTextAboveHeadInstance.transform.SetParent(GameObject.Find("TextAboveHead - Enemies").gameObject.transform);

        //Get the TextMeshProUGUI component from the instantiated object
        infoText = enemyTextAboveHeadInstance.GetComponent<TextMeshProUGUI>();

        enemyScript._enemyTookDmg += UpdateEnemyHealthText;
        enemyScript._enemyDied += DestroyText;

        //Start the scene with the enemy's health updated.
        UpdateEnemyHealthText();
    }

    void Update()
    {
        if (enemyScript.health > 0)
        {
            //Set text transform to the transform of the object this script is attached to
            Vector3 namePos = mainCam.WorldToScreenPoint(this.transform.position);
            infoText.transform.position = namePos;
        }
    }

    //Updates the health text
    private void UpdateEnemyHealthText()
    {
        infoText.text = enemyName + "\n" + "HP: " + enemyScript.health + " / " + enemyMaxHealth;
    }

    //When the enemy dies, destroy the text instance as well.
    private void DestroyText()
    {
        //When the enemy is doing their death ragdoll get rid of the text and instance.
        infoText.text = "";
        Destroy(enemyTextAboveHeadInstance);

        enemyScript._enemyTookDmg -= UpdateEnemyHealthText;
        enemyScript._enemyDied -= DestroyText;
    }
}
