using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Manages the survival rounds and enemy spawning.
public class RoundManager : MonoBehaviour
{
    EnemyCount enemyCountScript;
    public static int roundCount;

    [HideInInspector] public int enemiesToSpawn;

    public GameObject enemySpawnPointHolder;
    public GameObject playerSpawnPointHolder;
    public Transform playerTransform;
    public TextMeshProUGUI roundCountText;

    Animator roundCountAnimator;
    Transform aliveEnemiesGroup;

    List<Transform> enemySpawnPoints = new List<Transform>();
    List<Transform> playerSpawnPoints = new List<Transform>();
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

        roundCountAnimator = roundCountText.gameObject.GetComponent<Animator>();

        //Get the alive enemies gameobject
        aliveEnemiesGroup = GameObject.FindGameObjectWithTag("AliveEnemies").transform;

        //Get spawn point locations into a list for player and enemies
        GetSpawnPoints(enemySpawnPointHolder, ref enemySpawnPoints); //Enemies
        GetSpawnPoints(playerSpawnPointHolder, ref playerSpawnPoints); //Player

        //Choose a random spawn location for the player at the start of the game.
        int randomPlayerSpawnPos = Random.Range(0, playerSpawnPoints.Count);
        playerTransform.position = playerSpawnPoints[randomPlayerSpawnPos].position;

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
        List<Transform> spawnPointsDup = new List<Transform>(enemySpawnPoints);
        Transform spawnPos;
        int attemptsToFindPoint = 0;

        //ENEMY SPAWNING
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            //If all the spawn points have been used up and removed, reset it.
            if (spawnPointsDup.Count == 0)
            {
                spawnPointsDup = new List<Transform>(enemySpawnPoints);
            }

            //SPAWN POSITION
            //Get a random number for the spawn point between (0 - spawnPoints.Count + 1)
            while (true)
            {
                attemptsToFindPoint++; //The times this loop has been continued. In other words, the amount of times it failed to get a far enough spawn point.

                int randomSpawnPointNumber = Random.Range(0, spawnPointsDup.Count);
                //Calculate the distance between the player character and the random spawn point chosen.
                float distanceFromPlayer = Vector3.Distance(SwapCharacters.currentCharacter.transform.position, spawnPointsDup[randomSpawnPointNumber].position);

                //If the spawn point selected was within a distance of 30 meters, choose a different point.
                if (distanceFromPlayer <= 30)
                {
                    continue;
                }
                //Otherwise if the spawn point selected was outside 30 meters or there has been 100 attempts made to select a point, select the point and break out of the loop.
                else if (distanceFromPlayer > 30 || attemptsToFindPoint == 100)
                {
                    //Choose the spawn position based off the random number chosen
                    spawnPos = spawnPointsDup[randomSpawnPointNumber];
                    //Remove the spawn point from the duplicated list so that another enemy cannot spawn there.
                    spawnPointsDup.RemoveAt(randomSpawnPointNumber);
                    break;
                }       
            }

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
            roundCountAnimator.SetBool("Enlarge", true);
            yield return new WaitForSeconds(waitSeconds);
            roundCountAnimator.SetBool("Enlarge", false);
        }
        else
        {
            roundCountText.text = "ROUND\n" + "COMPLETE";
            roundCountAnimator.SetBool("Enlarge", true);
            yield return new WaitForSeconds(0.5f);
            FindObjectOfType<AudioManager>().Play("Success");
            yield return new WaitForSeconds(waitSeconds - 0.5f);
            roundCountAnimator.SetBool("Enlarge", false);
        }
    }

    void GetSpawnPoints(GameObject spawnPointHolder, ref List<Transform> spawnPointList)
    {
        //Get the enemy spawn points and put them into a list.
        foreach (Transform spawnPointGroup in spawnPointHolder.transform)
        {
            foreach (Transform point in spawnPointGroup)
            {
                if (point.gameObject.tag == "SpawnPoint")
                {
                    spawnPointList.Add(point);
                }
            }
        }
    }
}
