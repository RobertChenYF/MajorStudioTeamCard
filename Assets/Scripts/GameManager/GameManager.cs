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
        Combat,    //When the player is in combat
        Tutorial,     //For the tutorial
        Unknown,      //A catch all state
        _PRELOAD,     //Initial State for loading
    }

    public float PlayerHealth;
    public int gameScore;
    //public List<CardFunction> GlobalPlayerHand;
    public bool PlayGame;
    public bool ShowGlossary;
    public bool PlayTutorial;
    //private GameObject TutorialManager;


    StateType currentState;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        currentState = StateType._PRELOAD;
        PlayGame = false;
        ShowGlossary = false;
        PlayTutorial = false;
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
                    SceneManager.LoadScene("TutorialCombat");
                    //SceneManager.LoadScene("Glossary");
                    currentState = StateType.Combat;
                } else if (ShowGlossary == true)
                {
                    ShowGlossary = false;
                    SceneManager.LoadScene("CombatScene");
                    Debug.Log("No glossary scene");
                    currentState = StateType.Glossary;
                } else if (PlayTutorial == true)
                {
                    SceneManager.LoadScene("TutorialCombat");
                    currentState = StateType.Tutorial;
                }
                break;
            case StateType.Combat:
                break;
            case StateType.Glossary:
                break;
            case StateType.Tutorial:
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
}
