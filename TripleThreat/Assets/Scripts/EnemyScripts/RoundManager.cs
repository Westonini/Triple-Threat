using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Manages the survival rounds and enemy spawning.
public class RoundManager : MonoBehaviour
{
    EnemyCount enemyCountScript;
    static int roundCount;
    [HideInInspector] public int enemiesToSpawn;

    public GameObject spawnPointHolder;
    public TextMeshProUGUI roundCountText;
    Transform aliveEnemiesGroup;

    List<Transform> spawnPoints = new List<Transform>();
    List<string> spawnChancePercentages = new List<string>();
    List<GameObject> inactiveEnemies = new List<GameObject>();

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

    private void OnEnable()
    {
        //Everytime an enemy has died, call spawn enemies so that enemies in queue can replace the fallen ones.
        EnemyDeath._enemyDead += SpawnOneEnemy;
    }
    private void OnDisable()
    {
        EnemyDeath._enemyDead -= SpawnOneEnemy;
    }

    void RoundStart()
    {
        //Increase the roundCount by 1.
        roundCount += 1;

        //Display text that says the round number
        StopCoroutine("ShowRoundStartText");
        StartCoroutine(ShowRoundStartText(5f, false));

        //The amount of enemies to spawn per round.
        enemiesToSpawn = 10 + (roundCount * 2);

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
        //Duplicate the spawnPoints list
        List<Transform> spawnPointsDup = new List<Transform>(spawnPoints);

        //ENEMY SPAWNING
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            //If all the spawn points have been used up and removed, reset it.
            if (spawnPointsDup.Count == 0)
            {
                spawnPointsDup = new List<Transform>(spawnPoints);
            }

            //SPAWN POSITION
            //Get a random number for the spawn point between (0 - spawnPoints.Count + 1)
            int randomSpawnPointNumber = Random.Range(0, spawnPointsDup.Count);
            //Choose the spawn position based off the random number chosen
            Transform spawnPos = spawnPointsDup[randomSpawnPointNumber];
            //Remove the spawn point from the duplicated list so that another enemy cannot spawn there.
            spawnPointsDup.RemoveAt(randomSpawnPointNumber);

            //ENEMY TO SPAWN
            //Get a random number to choose which enemy to spawn between (0 - 100)
            int randomEnemyNumber = Random.Range(0, 100);
            //Choose an enemy to spawn based off the random number chosen
            string enemyToSpawn = spawnChancePercentages[randomEnemyNumber];

            //SPAWN THE ENEMY AT THE SPAWN POSITION
            //Look for the enemy name from the Enemies class and then spawn it at the random spawn point.
            foreach (Enemies enemy in enemies)
            {
                if (enemy.enemyName == enemyToSpawn)
                {
                    GameObject enemyInstance;
                    enemyInstance = Instantiate(enemy.enemyObject, spawnPos) as GameObject;
                    enemyInstance.name = enemy.enemyObject.name;
                    enemyInstance.transform.SetParent(aliveEnemiesGroup);

                    if (aliveEnemiesGroup.childCount > 40) //If there are already 40+ enemies that have been instantiated, set this enemy inactive for now and add them to the inactiveEnemies list.
                    {
                        enemyInstance.SetActive(false);
                        inactiveEnemies.Add(enemyInstance);
                    }
                }
            }
        }

        //Update the enemy count text
        enemyCountScript.UpdateEnemyCountText();
    }

    //Used to activate one inactive enemy once one active enemy dies.
    void SpawnOneEnemy()
    {
        //If there are enemies who are inactive..
        if (inactiveEnemies.Count > 0)
        {
            //Choose a random number between (0 - inactiveEnemies.Count)
            int randomNumber = Random.Range(0, inactiveEnemies.Count);
            //Activate the the randomly selected enemy and remove them from the inactiveEnemies list.
            inactiveEnemies[randomNumber].SetActive(true);
            inactiveEnemies.RemoveAt(randomNumber);
        }
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
