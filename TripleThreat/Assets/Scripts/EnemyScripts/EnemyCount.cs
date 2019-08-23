using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyCount : MonoBehaviour
{
    public TextMeshProUGUI enemyCount;
    Transform aliveEnemiesGroup;
    RoundManager roundManagerScript;

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
        //Get the alive enemies gameobject
        aliveEnemiesGroup = GameObject.FindGameObjectWithTag("AliveEnemies").transform;

        //Get the RoundManager script
        roundManagerScript = GetComponent<RoundManager>();
    }

    //Updates the EnemyCount text by checking how many children are within the enemiesGameObject. Specifically it checks those who are in the "Alive" gameobject within the "Enemies" gameobject.
    public void UpdateEnemyCountText()
    {
        enemyCount.text = aliveEnemiesGroup.childCount.ToString();

        //If there are 0 enemies left, call the RoundEnd function from the RoundManager script
        if (aliveEnemiesGroup.childCount == 0)
            roundManagerScript.RoundEnd();
    }
}
