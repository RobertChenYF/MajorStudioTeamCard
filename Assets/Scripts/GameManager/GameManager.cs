using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum StateType
    {
        MainMenu,      //Main menu state
        Glossary,      //When the player is in the Staging area between Encounters. Here is where the player picks their deck or picks the next Encounter.
        Combat,    //When the player is in combat - Use InGame instead
        Tutorial,     //For the tutorial - Use InGame instead
        InGame,       //For any type of scene where the player will start combat (Tutorial and normal runs)
        Unknown,      //A catch all state
        _PRELOAD,     //Initial State for loading
    }

    //public float PlayerHealth;
    //public int gameScore;
    //public List<CardFunction> GlobalPlayerHand;
    public bool PlayGame;
    public bool ShowGlossary;
    public bool PlayTutorial;
    public bool OpenPauseMenu;
    public GameObject PauseMenuCanvas;
    //private GameObject TutorialManager;


    StateType currentState;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        currentState = StateType._PRELOAD;
        PlayGame = false;
        ShowGlossary = false;
        PlayTutorial = false;
        OpenPauseMenu = false;
    }

    private void Update()
    {
        switch (currentState)
        {
            case StateType._PRELOAD:
                if (currentState == StateType._PRELOAD)
                {
                    SwitchScenes("MainMenu", StateType.MainMenu);
                }
                break;
            case StateType.MainMenu:
                if (PlayGame == true)
                {
                    PlayGame = false;
                    SceneManager.LoadScene("CombatScene");
                    //SceneManager.LoadScene("Glossary");
                    currentState = StateType.InGame;
                } else if (ShowGlossary == true)
                {
                    ShowGlossary = false;
                    SceneManager.LoadScene("CombatScene");
                    Debug.Log("No glossary scene");
                    currentState = StateType.Glossary;
                } else if (PlayTutorial == true)
                {
                    PlayTutorial = false;
                    SceneManager.LoadScene("TutorialCombat");
                    currentState = StateType.InGame;
                }
                break;
            case StateType.Combat:
                break;
            case StateType.Glossary:
                break;
            case StateType.Tutorial:
                break;
            case StateType.InGame:
                if (Input.GetKeyDown(KeyCode.Escape) && OpenPauseMenu == false)
                {
                    OpenPauseMenu = true;
                } else if (Input.GetKeyDown(KeyCode.Escape) && OpenPauseMenu == true)
                {
                    OpenPauseMenu = false;
                }

                if (OpenPauseMenu == true)
                {
                    PauseMenuCanvas.SetActive(true);
                } else if (OpenPauseMenu == false)
                {
                    PauseMenuCanvas.SetActive(false);
                }
                break;
            case StateType.Unknown: 
                //empty
                break;
            default:
                print("Hold on partner, you went too far.");
                break;
        }
    }

    public void SwitchScenes(string sceneName, StateType stateName)
    {
        SceneManager.LoadScene(sceneName);
        currentState = stateName;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
