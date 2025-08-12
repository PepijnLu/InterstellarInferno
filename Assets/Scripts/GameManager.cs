using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> enemyPrefabList = new List<GameObject>();
    public List<GameObject> lootLibrary, powerUpList;
    public List<Material> planetMaterials;
    public List<Material> skyboxMaterials;
    public Planet currentPlanet;
    public bool onTeleporter;
    bool ableToAdvance, paused, won;
    public int enemyDivider, chanceForLoot, xpLevel = 1, nextAbilityUnlockLevel, levelsNeededForNextAbility, abilitiesUnlocked;
    int currentDifficulty, currentAverageEnemies;
    public float playerXP, maxHpPerLevel, gravityMultiplier, xpToNextLevel;
    public GameObject player;
    public Damagable playerDamagable;
    private GameObject[] deactivates;
   
    public Animator animator;

    void Awake()
    {
        instance = this;
        xpToNextLevel = 20;
        xpLevel = 1;
        playerDamagable = player.GetComponent<Damagable>();
    }
    

    // Start is called before the first frame update
    void Start()
    {
        deactivates = GameObject.FindGameObjectsWithTag("DeactivateOnStart");
        foreach (GameObject deactivate in deactivates) {deactivate.SetActive(false);}
        int planetMaterial = Random.Range(0, planetMaterials.Count - 1);
        currentPlanet.Activate(currentPlanet.difficulty, currentPlanet.averageEnemies, planetMaterials[planetMaterial], skyboxMaterials[planetMaterial]);
        Damagable playerDamagable = player.GetComponent<Damagable>();
        UIManager.instance.UpdateSlider(UIManager.instance.healthSlider, playerDamagable.health);
        UIManager.instance.ChangeText(UIManager.instance.hPText, playerDamagable.health.ToString());
        UIManager.instance.UpdateSliderMax(UIManager.instance.healthSlider, playerDamagable.maxHealth);
        InputManager.instance.SetKeybindsText();
        UIManager.instance.UpdateSlider(UIManager.instance.musicSlider, GameData.musicVolume);
        UIManager.instance.UpdateSlider(UIManager.instance.sfxSlider, GameData.sfxVolume);
        GameData.score = 0;
        GameData.planetsConquered = 0;

        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if ((onTeleporter == true) && (ableToAdvance == true) )
        {
            AdvancePlanet();
            Debug.Log("ADVANCE");
            ableToAdvance = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name == "GameSceneNorm")
        {
            if (paused == false)
            {
                Time.timeScale = 0f;
                paused = true;
                UIManager.instance.SetElementActive(UIManager.instance.settingsMenu, true);
                InputManager.instance.settingsOpen = true;
            }
            else
            {
                Time.timeScale = 1f;
                paused = false;
                UIManager.instance.SetElementActive(UIManager.instance.settingsMenu, false);
                InputManager.instance.settingsOpen = false;
            }
        }
    }

    public void RemoveFromList(GameObject objectToRemove)
    {
        enemies.Remove(objectToRemove);
        int numberOfEnemies = enemies.Count;
        if (numberOfEnemies <= 0) { ableToAdvance = true;   currentPlanet.teleporterPortal.SetActive(true); AudioManager.instance.PlaySound(AudioManager.instance.teleporterActive); }       
    }

    void AdvancePlanet()
    {   
        Debug.Log("Advance Planet");
        AudioManager.instance.PlaySound(AudioManager.instance.teleporterUsed);
        GameObject[] deadEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject deadEnemy in deadEnemies)
        {
            Destroy(deadEnemy);
        }
        GameObject nextPlanet = Instantiate(currentPlanet.nextPlanetPrefab, currentPlanet.gameObject.transform.position + new Vector3(200f, 200f, 200f), currentPlanet.transform.rotation);
        Debug.Log(currentPlanet.gameObject.name);
        currentPlanet.gameObject.SetActive(false);
        currentDifficulty = currentPlanet.difficulty;
        currentAverageEnemies = currentPlanet.averageEnemies;
        Planet oldPlanet = currentPlanet;
        currentPlanet = nextPlanet.GetComponent<Planet>();
        int planetMaterial = Random.Range(0, planetMaterials.Count - 1);
        currentPlanet.Activate(currentDifficulty + 1, currentAverageEnemies + 2, planetMaterials[planetMaterial], skyboxMaterials[planetMaterial]);
        player.transform.position = currentPlanet.playerSpawnLoc.position;
        onTeleporter = false;
        GameData.planetsConquered++;
        if (GameData.planetsConquered == 10 )
        {
            WinGame();
        }
        Destroy(oldPlanet);
    }

    void WinGame()
    {
        Time.timeScale = 0f;
        won = true;
        UIManager.instance.winScreen.SetActive(true);
        AudioManager.instance.PlaySound(AudioManager.instance.audioSources["winTrack"]);
        AudioManager.instance.audioSources["gameTrack1"].Pause();
        UIManager.instance.planetsConquered.text = ("Planets Conquered: " + GameData.planetsConquered);
            if (GameData.score > GameData.highScore)
            {
                GameData.highScore = GameData.score;
            }
            UIManager.instance.score.text = ("Score: " + GameData.score);
            UIManager.instance.highScore.text = ("High Score: " + GameData.highScore);
    }
    public void ChangeHP(GameObject objectHurt, float damage)
    {   
        Damagable damagable = objectHurt.GetComponent<Damagable>();
        if (damagable.invincible != true)
        {
            damagable.health -= damage;
            if (AbilityLibrary.instance.ability2 == true && objectHurt.tag != "Player")
            {
                playerDamagable.health += Mathf.RoundToInt(damage);
                if (playerDamagable.health > playerDamagable.maxHealth)
                {
                    playerDamagable.health = playerDamagable.maxHealth;
                }
                UIManager.instance.UpdateSlider(UIManager.instance.healthSlider, playerDamagable.health);
                UIManager.instance.ChangeText(UIManager.instance.hPText, playerDamagable.health.ToString());
            }
        }
        if ( (damagable.health < 0))
        {
            damagable.health = 0;
        }
        if (damagable.health > damagable.maxHealth)
        {
            damagable.health = damagable.maxHealth;
        }
        if (objectHurt.tag == "Player")
        {
            UIManager.instance.UpdateSlider(UIManager.instance.healthSlider, damagable.health);
            UIManager.instance.ChangeText(UIManager.instance.hPText, damagable.health.ToString());
            PlayerCombat.instance.SetTriggerToSomething("TakeDamage");
        }

        if (damagable.health <= 0 && damagable.dead == false) 
        {
            damagable.dead = true;
            if (objectHurt.tag == "Player")
            {
                SceneManager.LoadScene("GameOver");
            }
            if (objectHurt.tag == "Enemy")
            {
                objectHurt.GetComponent<Enemy>().Die();
            }    
            
            GameData.score += Mathf.FloorToInt(damagable.maxHealth);
            playerXP += damagable.xpReward;
            UIManager.instance.UpdateSlider(UIManager.instance.xpSlider, playerXP);
            GenerateRandomLoot(damagable.lootQuality, objectHurt.transform); 
            
            if (objectHurt.tag != "Enemy")
            {
                Destroy(objectHurt);
            }

            if (playerXP >= xpToNextLevel) {    LevelUp();  }  
        }  

        if (objectHurt.tag == "Enemy")
        {
            objectHurt.GetComponent<Enemy>().SetAnimatorTrigger("TakeDamage");
        }
            
        damagable = null;
    }

    void LevelUp()
    {
        xpLevel++;
        playerXP = 0;
        xpToNextLevel += xpToNextLevel / 2;

        Damagable playerDamagable = player.GetComponent<Damagable>();
        playerDamagable.maxHealth += maxHpPerLevel;
        playerDamagable.health = playerDamagable.maxHealth;
        playerDamagable.damageMultiplier += 0.2f;
        PlayerMovement.instance.playerSpeed += 0.75f;
        
        if (xpLevel >= nextAbilityUnlockLevel) 
        {  
            abilitiesUnlocked++;
            nextAbilityUnlockLevel += levelsNeededForNextAbility;

            if (AbilityLibrary.instance.abilitySprites.Count >= abilitiesUnlocked)
            {
                UIManager.instance.UpdateAbilitySlot(abilitiesUnlocked - 1, AbilityLibrary.instance.abilitySprites[abilitiesUnlocked - 1]);
            }
            
        }
  
        UIManager.instance.UpdateSliderMax(UIManager.instance.xpSlider, xpToNextLevel);
        UIManager.instance.UpdateSlider(UIManager.instance.xpSlider, playerXP); 
        UIManager.instance.ChangeText(UIManager.instance.xpLevelText, xpLevel.ToString());
        UIManager.instance.ChangeText(UIManager.instance.xpNeededText, (xpLevel + 1).ToString());  
        UIManager.instance.UpdateSliderMax(UIManager.instance.healthSlider, playerDamagable.maxHealth);
        UIManager.instance.UpdateSlider(UIManager.instance.healthSlider, playerDamagable.health);
        UIManager.instance.ChangeText(UIManager.instance.maxHpText, playerDamagable.health.ToString());
    }

    void GenerateRandomLoot(int quality, Transform lootTransform)
    {
        int lootChance = Random.Range(1, 6);
        int lootRangeNumber = Random.Range(1, 6);
        int whichLootNumber = lootRangeNumber + quality;
        Debug.Log("Lootchance = " + lootChance + "whichLootNumber " + whichLootNumber);
        if (lootChance <= chanceForLoot)
        {
            switch(whichLootNumber)
            {
                case <= 5: 
                    Instantiate(lootLibrary[0], lootTransform.position, lootTransform.rotation);
                    break;
                case 6:
                    //weapon
                    GameObject newWeapon = Instantiate(lootLibrary[1], lootTransform.position, lootTransform.rotation);
                    newWeapon.GetComponent<Weapon>().SetRandomStats(quality);
                    break;
                case 7:
                    int whichPowerUp = Random.Range(0, powerUpList.Count);
                    Instantiate(powerUpList[whichPowerUp], lootTransform.position, lootTransform.rotation);
                    break;
                
                case 8:
                    //weapon
                    GameObject newWeapon1 = Instantiate(lootLibrary[1], lootTransform.position, lootTransform.rotation);
                    newWeapon1.GetComponent<Weapon>().SetRandomStats(quality);
                    break;
                
                case 9:
                    //powerup
                    int whichPowerUp2 = Random.Range(0, powerUpList.Count);
                    Instantiate(powerUpList[whichPowerUp2], lootTransform.position, lootTransform.rotation);
                    break;

                case 10:
                    //weapon
                    GameObject newWeapon2 = Instantiate(lootLibrary[1], lootTransform.position, lootTransform.rotation);
                    newWeapon2.GetComponent<Weapon>().SetRandomStats(quality);
                    break;
            }
            
        }
    }
}