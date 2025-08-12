using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


public class Button : MonoBehaviour
{
    public int buttonInt;
    public string action;
    public Text keybindText;

    public void WhenClicked()
    {
        switch(buttonInt)
        {
            case 1:
                //START GAME
                SceneManager.LoadScene("GameSceneNorm");
                break;
            case 2:
                //SETTINGS (from main menu)
                UIManager.instance.SetElementActive(UIManager.instance.settingsMenu, true);
                UIManager.instance.SetElementActive(UIManager.instance.mainMenu, false);
                InputManager.instance.settingsOpen = true;
                break;
            case 3:
                //QUIT
                Application.Quit();
                Debug.Log("quit");
                break;
            case 4:
                //EXIT SETTINGS (main menu)
                if (SceneManager.GetActiveScene().name == "Menu")
                {
                    UIManager.instance.SetElementActive(UIManager.instance.mainMenu, true);
                }
                else
                {
                    Time.timeScale = 1f;
                }
                UIManager.instance.SetElementActive(UIManager.instance.settingsMenu, false);
                InputManager.instance.currentButton = null;
                InputManager.instance.currentAction = null;
                InputManager.instance.settingsOpen = false;
                break;
            case 5:
                //BACK TO MAIN MENU
                SceneManager.LoadScene("Menu");
                break;
            case 6:
                InputManager.instance.SetDefaultKeybinds();
                InputManager.instance.SetKeybindsText();
                InputManager.instance.ReplaceAlpha();
                UIManager.instance.UpdateSlider(UIManager.instance.musicSlider, 50);
                UIManager.instance.UpdateSlider(UIManager.instance.sfxSlider, 50);
                break;
            case 7:
                UIManager.instance.youSureMenu.SetActive(true);
                break;
            case 8:
                UIManager.instance.youSureMenu.SetActive(false);
                Debug.Log("case 8");
                break;
            case 9:
                UIManager.instance.winScreen.SetActive(false);
                AudioManager.instance.audioSources["gameTrack1"].UnPause();
                AudioManager.instance.audioSources["winTrack"].Stop();
                Time.timeScale = 1f;
                break;
        }
    }

    public void RemapKeybind()
    {
        InputManager.instance.currentButton = gameObject;
        InputManager.instance.currentAction = action;
        InputManager.instance.currentButtonScript = gameObject.GetComponent<Button>();
    }
}
