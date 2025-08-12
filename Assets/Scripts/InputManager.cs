using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public GameObject currentButton;
    public Button currentButtonScript;
    public string currentAction;
    public bool settingsOpen;

    void Awake()
    {
        instance = this;

        if (SceneManager.GetActiveScene().name == "Menu" && (GameData.notFirstTimeMenu == false))
        {
            SetDefaultKeybinds();
            Debug.Log("Set dictionary");
            GameData.notFirstTimeMenu = true;
        }
        SetKeybindsText();

    }

    public void ReplaceAlpha()
    {
        Debug.Log("replace alpha");
        GameObject[] texts = GameObject.FindGameObjectsWithTag("text");
        foreach (GameObject text in texts)
        {
            Text textToReplace = text.GetComponent<Text>();
            if (textToReplace.text.Contains("Alpha") || textToReplace.text.Contains("Arrow"))
            {
                Debug.Log(textToReplace.text);
                string newString = textToReplace.text.Replace("Alpha", "");
                string evenNewerString = newString.Replace("Arrow", "");
                textToReplace.text = evenNewerString;
                Debug.Log(newString);
            }
        }
    }

    public void SetDefaultKeybinds()
    {
        GameData.keycodes["forward"] = KeyCode.W;
        GameData.keycodes["backward"] = KeyCode.S;
        GameData.keycodes["left"] = KeyCode.A;
        GameData.keycodes["right"] = KeyCode.D;
        GameData.keycodes["pickup"] = KeyCode.E;
        GameData.keycodes["drop"] = KeyCode.G;
        GameData.keycodes["ability1"] = KeyCode.Space;
        GameData.keycodes["ability2"] = KeyCode.Q;
        GameData.keycodes["ability3"] = KeyCode.R;
        GameData.keycodes["slot1"] = KeyCode.Alpha1;
        GameData.keycodes["slot2"] = KeyCode.Alpha2;
        GameData.keycodes["slot3"] = KeyCode.Alpha3;
        GameData.keycodes["slot4"] = KeyCode.Alpha4;
        GameData.keycodes["slot5"] = KeyCode.Alpha5;
    }
    public void SetKeybindsText()
    {
        GameObject[] buttons  = GameObject.FindGameObjectsWithTag("KeybindButton");
        foreach (GameObject button in buttons)
        {
            Button buttonScript = button.GetComponent<Button>();
            buttonScript.keybindText.text = GameData.keycodes[buttonScript.action].ToString();
            Debug.Log(GameData.keycodes[buttonScript.action].ToString());
            ReplaceAlpha();
            if (buttonScript.keybindText.text.Length != 1)
            {
                buttonScript.keybindText.fontSize = Mathf.RoundToInt((50 / buttonScript.keybindText.text.Length) * 2.5f);
            }
            else
            {
                buttonScript.keybindText.fontSize = 50;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (settingsOpen == true)
        {
            RemapKeybind(currentButton, currentAction, currentButtonScript);
        }
    }

    public void RemapKeybind(GameObject button, string action, Button buttonScript)
    {
        UIManager.instance.pressAnyKey.gameObject.SetActive(false);
        if (currentButton != null && currentAction != null && buttonScript != null) 
        {
            UIManager.instance.pressAnyKey.gameObject.SetActive(true);
            //Escape to cancel
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                currentButton = null; currentAction = null;
            }
            else if (Input.anyKeyDown)
            {   
                
                if (Input.GetMouseButton(0))
                {

                }
                else
                {
                    KeyCode pressedKey = GetPressedKey();
                    GameData.keycodes[action] = pressedKey;
                    buttonScript.keybindText.text = pressedKey.ToString();
                    ReplaceAlpha();
                    if (buttonScript.keybindText.text.Length != 1)
                    {
                        buttonScript.keybindText.fontSize = Mathf.RoundToInt((50 / buttonScript.keybindText.text.Length) * 2.5f);
                    }
                    else
                    {
                        buttonScript.keybindText.fontSize = 50;
                    }
                    currentButton = null; currentAction = null;
                    Debug.Log(GameData.keycodes[action].ToString());
                    UIManager.instance.SetSlotTexts();
                    ReplaceAlpha();
                }
            }
        }  
    }

    KeyCode GetPressedKey()
    {
        foreach(KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                return keyCode;
            }
        }
        return KeyCode.None;
    }
}
