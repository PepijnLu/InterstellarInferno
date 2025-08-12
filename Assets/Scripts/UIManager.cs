using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Damagable playerHealth;
    public List<Image> invSlots;
    public List<Image> abilitySlots;
    public Slider healthSlider, xpSlider, musicSlider, sfxSlider;
    public GameObject equipToSlot;
    public Text enemiesRemaning, xpNeededText, xpLevelText, maxHpText, hPText, pressAnyKey;
    public Text planetsConquered, score, highScore;
    public Text slot1, slot2, slot3, slot4, slot5, abilitySlot1, abilitySlot2, abilitySlot3;
    public GameObject mainMenu, settingsMenu, youSureMenu, winScreen, ability1CD, ability2CD, ability3CD;
    public GameObject ability1CDBorder, ability2CDBorder, ability3CDBorder;
    public Image hpFullImage;
    public Text ability1CDText, ability2CDText, ability3CDText;
    public List<Image> powerupSlots;
    public int powerupSlotsUsed;
    public List<string> powerupQueue = new List<string>();
    void Awake() => instance = this;
    GameObject[] deactivate;

    // Start is called before the first frame update
    void Start()
    {
        deactivate = GameObject.FindGameObjectsWithTag("UIDeactivate");
        foreach (GameObject deactivate in deactivate)
        {
            deactivate.SetActive(false);
        }
        if (healthSlider != null && xpSlider != null)
        {
            healthSlider.maxValue = playerHealth.health;
            healthSlider.value = playerHealth.health;
            xpSlider.maxValue = GameManager.instance.xpToNextLevel;
            xpSlider.value = 0;
            ChangeText(xpLevelText, GameManager.instance.xpLevel.ToString());
            ChangeText(xpNeededText, (GameManager.instance.xpLevel + 1).ToString());
            ChangeText(hPText, playerHealth.health.ToString());
        }
        if (sfxSlider != null && musicSlider != null)
        {
            UpdateSlider(musicSlider, GameData.musicVolume);
            UpdateSlider(sfxSlider, GameData.sfxVolume);
            sfxSlider.onValueChanged.AddListener(ChangeSFX);
            musicSlider.onValueChanged.AddListener(ChangeMusic);
        }
        
        if (invSlots.Count != 0)
        {
            for (int i = 0; i < 5; i++)
            {
                invSlots[i].sprite = PlayerInventory.instance.inventory[i].GetComponent<Weapon>().weaponSprite;  
                if (i < 3)
                {
                    abilitySlots[i].sprite = AbilityLibrary.instance.lockedAbilitySprite;
                }  
            }
        } 
        if (SceneManager.GetActiveScene().name == "GameOver")
        {
            planetsConquered.text = ("Planets Conquered: " + GameData.planetsConquered);
            if (GameData.score > GameData.highScore)
            {
                GameData.highScore = GameData.score;
            }
            score.text = ("Score: " + GameData.score);
            highScore.text = ("High Score: " + GameData.highScore);
            GameData.score = 0;
            GameData.planetsConquered = 0;
        }

        if (SceneManager.GetActiveScene().name == "GameSceneNorm")
        {
            SetSlotTexts();
            equipToSlot.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ActivatePowerupSlot(Sprite powerupSprite, bool maybe)
    {
        if (maybe == true)
        {
            powerupQueue.Add(powerupSprite.name);
        
            powerupSlots[powerupSlotsUsed - 1].gameObject.SetActive(maybe);
            powerupSlots[powerupSlotsUsed - 1].sprite = powerupSprite;
        }

        if (maybe == false)
        {
            //set slot 3 to false
            powerupSlots[powerupSlotsUsed - 1].gameObject.SetActive(maybe);
                for (int i = 0; i < powerupSlots.Count; i++)
                {
                    if ((powerupSlots[i].sprite.name == powerupQueue[0]))
                    {
                        if (i == 0)
                        {
                            powerupSlots[i].sprite = powerupSlots[i + 1].sprite;
                            powerupSlots[i + 1].sprite = powerupSlots[i + 2].sprite;
                        }
                        if (i == 1)
                        {
                            powerupSlots[i].sprite = powerupSlots[i + 1].sprite;
                        }
                    }
                }
                powerupQueue.Remove(powerupQueue[0]);
        }
    }
    void ReplaceAlpha()
    {
        equipToSlot.SetActive(true);
        GameObject[] texts = GameObject.FindGameObjectsWithTag("text");
        foreach (GameObject text in texts)
        {
            Text textToReplace = text.GetComponent<Text>();
            if (textToReplace.text.Contains("Alpha") || textToReplace.text.Contains("Arrow"))
            {
                string newString = textToReplace.text.Replace("Alpha", "");
                string evenNewerString = newString.Replace("Arrow", "");
                textToReplace.text = evenNewerString;
            }
        }
        equipToSlot.SetActive(false);
    }
    public void SetSlotTexts()
    {
        slot1.text = GameData.keycodes["slot1"].ToString();
        slot2.text = GameData.keycodes["slot2"].ToString();
        slot3.text = GameData.keycodes["slot3"].ToString();
        slot4.text = GameData.keycodes["slot4"].ToString();
        slot5.text = GameData.keycodes["slot5"].ToString();
        abilitySlot1.text = GameData.keycodes["ability1"].ToString();
        abilitySlot2.text = GameData.keycodes["ability2"].ToString();
        abilitySlot3.text = GameData.keycodes["ability3"].ToString();
        equipToSlot.GetComponent<Text>().text = ("Equip To Slot: (" + GameData.keycodes["slot1"] + "," + GameData.keycodes["slot2"] + "," + GameData.keycodes["slot3"] + "," + GameData.keycodes["slot4"] + "," + GameData.keycodes["slot5"] + ")");
        ReplaceAlpha(); 
    }

    public void SetElementActive(GameObject element, bool active)
    {
        if (active) {   element.SetActive(true);   }
        else        {   element.SetActive(false);  }
        
    }
    public void UpdateInvSlot(int inventorySlot, GameObject equipping)
    {
        invSlots[inventorySlot].sprite = equipping.GetComponent<Weapon>().weaponSprite;
        if (equipping.GetComponent<Weapon>().isNull == false)
        {
            invSlots[inventorySlot].GetComponent<Image>().color = equipping.GetComponent<Weapon>().weaponColor;
        }
        else
        {
            invSlots[inventorySlot].GetComponent<Image>().color = new Color(1f, 1f, 1f);
        }
    }

    public void UpdateAbilitySlot(int abilitySlot, Sprite newSprite)
    {
        abilitySlots[abilitySlot].sprite = newSprite;
    }

    public void UpdateSliderMax(Slider slider, float newMaxValue)
    {
        slider.maxValue = newMaxValue;
    }

    public void UpdateSlider(Slider slider, float value)
    {
        slider.value = value;
    } 

    public void ChangeTMPro(TextMeshProUGUI textElement, string newString)
    {
        textElement.text = newString; 
    }

    public void ChangeText(Text textElement, string newString)
    {
        textElement.text = newString; 
    }
    
    void ChangeMusic(float value)
    {
        GameData.musicVolume = value;
        AudioManager.instance.UpdateVolume();
    }

    void ChangeSFX(float value)
    {
        GameData.sfxVolume = value;
        AudioManager.instance.UpdateVolume();
    }
}
