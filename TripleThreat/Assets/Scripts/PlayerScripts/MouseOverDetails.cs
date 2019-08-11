using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//When the player mouses over an enemy, show the enemy's name and health
public class MouseOverDetails : MonoBehaviour
{
    LayerMask enemy;
    Camera mainCam;

    GameObject enemyInfoText;


    void Start()
    {
        enemy = LayerMask.GetMask("Enemy");
        mainCam = Camera.main;
    }

    void Update()
    {
        //Move character's y rotation to look at where mouse is pointing
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        //If character's mouse is on the Enemy layer...
        if (Physics.Raycast(ray, out RaycastHit hitEnemy, Mathf.Infinity, enemy))
        {
            //Get the enemy's text gameobject instance and active it while the mouse is hovering over the Enemy collider.
            enemyInfoText = hitEnemy.collider.gameObject.GetComponentInChildren<EnemyMoveTextWithParent>().enemyTextAboveHeadInstance;
            enemyInfoText.SetActive(true);
        }
        else
        {
            //If the mouse if off of the collider, set the enemy's text gameobject to inactive.
            if (enemyInfoText != null)
            {
                enemyInfoText.SetActive(false);
            }
        }
    }
}
