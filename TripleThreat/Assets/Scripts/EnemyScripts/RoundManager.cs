using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Manages the survival rounds and enemy spawning.
public class RoundManager : MonoBehaviour
{
    EnemyCount enemyCountScript;
    static int roundCount;

    public GameObject spawnPointHolder;
    public TextMeshProUGUI roundCountText;
    Transform aliveEnemiesGroup;

    List<Transform> spawnPoints = new List<Transform>();
    List<string> spawnChancePercentages = new List<string>();

    [Space]
    public Enemies[] enemies;

    //Allows me to add enemies into the inspector with their name, prefab, and spawn chance.
    [System.Serializable]
    public class Enemies
    {
        public string enemyName;
        public GameObject enemyObject;
        public float spawnChance;
    }

    private void Awake()
    {
        //Get the EnemyCount script from the same gameobject
        enemyCountScript = GetComponent<EnemyCount>();
    }

    void Start()
    {
        //Reset at Start()
        roundCount = 0;

        //Get the alive enemies gameobject
        aliveEnemiesGroup = GameObject.FindGameObjectWithTag("AliveEnemies").transform;

        //Get the enemy spawn points and put them into a list.
        foreach (Transform spawnPointGroup in spawnPointHolder.transform)
        {
            foreach (Transform point in spawnPointGroup)
            {
                if (point.gameObject.tag == "SpawnPoint")
                {
                    spawnPoints.Add(point);
                }
            }
        }

        //For each enemy element that was made in the inspector..
        foreach (Enemies enemy in enemies)
        {
            //Fill the list "spawnChancePercentages" with the enemy's name "spawnChance" amount of times.
            //For example: enemyName = Grunt and spawnChance = 40. so put the string "Grunt" into the list 40 times.
            for (int i = 0; i < enemy.spawnChance; i++)
                spawnChancePercentages.Add(enemy.enemyName);
        }

        //Call RoundStart as the scene loads
        RoundStart();
    }

    void RoundStart()
    {
        //Increase the roundCount by 1.
        roundCount += 1;

        //Display text that says the round number
        StopCoroutine("ShowRoundStartText");
        StartCoroutine(ShowRoundStartText(5f, false));

        //Spawn enemies 2.5 seconds after the round starts
        Invoke("SpawnEnemies", 2.5f);
    }

    public void RoundEnd()
    {
        //Display text that says the round number
        StopCoroutine("ShowRoundStartText");
        StartCoroutine(ShowRoundStartText(5f, true));

        //Invoke RoundStart 2.5 seconds after the previous round was completed
        Invoke("RoundStart", 7.5f);
    }

    void SpawnEnemies()
    {
        //ENEMY SPAWNING
        //The amount of enemies to spawn per round.
        int enemiesToSpawn = 10 + (roundCount * 2);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            //Get a random number for the spawn point between (0 - spawnPoints.Count + 1)
            int randomSpawnPointNumber = Random.Range(0, spawnPoints.Count);
            //Choose the spawn position based off the random number chosen
            Transform spawnPos = spawnPoints[randomSpawnPointNumber];

            //Get a random number to choose which enemy to spawn between (0 - 100)
            int randomEnemyNumber = Random.Range(0, 100);
            //Choose an enemy to spawn based off the random number chosen
            string enemyToSpawn = spawnChancePercentages[randomEnemyNumber];

            //Look for the enemy name from the Enemies class and then spawn it at the random spawn point.
            foreach (Enemies enemy in enemies)
            {
                if (enemy.enemyName == enemyToSpawn)
                {
                    GameObject enemyInstance;
                    enemyInstance = Instantiate(enemy.enemyObject, spawnPos) as GameObject;
                    enemyInstance.name = enemy.enemyObject.name;
                    enemyInstance.transform.SetParent(aliveEnemiesGroup);
                }
            }
        }

        //Update the enemy count text
        enemyCountScript.UpdateEnemyCountText();
    }

    IEnumerator ShowRoundStartText(float waitSeconds, bool roundComplete)
    {
        //ROUND TEXT
        if (!roundComplete)
        {
            roundCountText.text = "ROUND\n" + roundCount.ToString();
            yield return new WaitForSeconds(waitSeconds);
            roundCountText.text = "";
        }
        else
        {
            roundCountText.text = "ROUND\n" + "COMPLETE";
            yield return new WaitForSeconds(waitSeconds);
            roundCountText.text = "";
        }
    }
}
