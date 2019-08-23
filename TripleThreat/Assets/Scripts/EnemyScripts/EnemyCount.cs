using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyCount : MonoBehaviour
{
    TextMeshProUGUI enemyCount;
    public GameObject enemiesGameObject;

    //Subscribe/Unsubscribe to _enemyDead to call UpdateEnemyCountText() whenever an enemy dies.
    private void OnEnable()
    {
        EnemyDeath._enemyDead += UpdateEnemyCountText;
    }
    private void OnDisable()
    {
        EnemyDeath._enemyDead -= UpdateEnemyCountText;
    }

    void Awake()
    {
        //Get the TextMeshProUGUI component from the object this script is attached to
        enemyCount = GetComponent<TextMeshProUGUI>();

        //Update the enemy count at start of scene load.
        UpdateEnemyCountText();
    }

    //Updates the EnemyCount text by checking how many children are within the enemiesGameObject. Specifically it checks those who are in the "Alive" gameobject within the "Enemies" gameobject.
    void UpdateEnemyCountText()
    {
        enemyCount.text = enemiesGameObject.transform.childCount.ToString();
    }
}
