using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public Planet nextPlanet;
    public GameObject nextPlanetPrefab;
    public Transform playerSpawnLoc;
    public List<Transform> enemySpawnLocs = new List<Transform>();
    public int averageEnemies, difficulty;
    int whichEnemy;
    public GameObject teleporterPortal;

    // Start is called before the first frame update
    void Start()
    {
        teleporterPortal.SetActive(false);
    }

    public void Activate(int difficultyNum, int averageEnemiesNum, Material newMaterial, Material newSkybox)
    {
        gameObject.GetComponent<Renderer>().material = newMaterial;
        RenderSettings.skybox = newSkybox;
        averageEnemies = averageEnemiesNum;
        difficulty = difficultyNum;
        int enemyRange = averageEnemies / GameManager.instance.enemyDivider;
        int numberOfEnemies = Random.Range((averageEnemies - enemyRange), (averageEnemies + enemyRange + 1));
        for (int i = 1; i <= numberOfEnemies; i++)
        {
            int whichEnemyRandomInt = Random.Range(1, 4);
            int whichEnemyDifficultyApplied = whichEnemyRandomInt + difficulty;
            switch(whichEnemyDifficultyApplied)
            {
                case >= 11:
                    whichEnemy = 4;
                    break;
                case >= 9:
                    whichEnemy = 3;
                    break;
                case >= 7:
                    whichEnemy = 2;
                    break;
                case >= 5:
                    whichEnemy = 1;
                    break;
                case >= 2:
                    whichEnemy = 0;
                    break;
            }

            int whichLoc = Random.Range(0, enemySpawnLocs.Count);
            Transform spawnLocation = enemySpawnLocs[whichLoc];
            GameObject newEnemy = Instantiate(GameManager.instance.enemyPrefabList[whichEnemy], spawnLocation.position, transform.rotation);
            newEnemy.transform.Translate(i - 0.5f, 0, i - 0.5f);
        }

        Debug.Log("spawnloc: " + playerSpawnLoc.position);
        Debug.Log("planet loc: " + transform.position);
    }
}
