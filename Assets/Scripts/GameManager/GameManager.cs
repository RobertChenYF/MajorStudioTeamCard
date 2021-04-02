using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum StateType
    {
        MainMenu,      //Main menu state
        Staging,      //When the player is in the Staging area between Encounters. Here is where the player picks their deck or picks the next Encounter.
        Encounter,    //When the player is in an Encounter
        Unknown,      //A catch all state
        _PRELOAD,     //Initial State for loading
    }

    public float PlayerHealth;
    public int gameScore;
    public List<CardFunction> GlobalPlayerHand;
    public bool PlayGame;


    StateType currentState;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        currentState = StateType._PRELOAD;
        PlayerHealth = 100;
        PlayGame = false;
    }

    private void Update()
    {
        switch (currentState)
        {
            case StateType._PRELOAD:
                if (currentState == StateType._PRELOAD)
                {
                    Debug.Log(currentState);
                    SwitchScenes("MainMenu", StateType.MainMenu);
                }
                break;
            case StateType.MainMenu:
                if (PlayGame == true)
                {
                    Debug.Log("Reached the Game Manager");
                    SceneManager.LoadScene("Staging");
                    currentState = StateType.Staging;
                    PlayGame = false;
                }
                break;
            case StateType.Staging:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SceneManager.LoadScene("Encounter");
                    currentState = StateType.Encounter;
                }
                break;
            case StateType.Encounter:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SceneManager.LoadScene("Staging");
                    currentState = StateType.Staging;
                }
                break;
            case StateType.Unknown:
                //empty
                break;
            default:
                print("Incorrect intelligence level.");
                break;
        }
    }

    public void SwitchScenes(string sceneName, StateType stateName)
    {
        SceneManager.LoadScene(sceneName);
        currentState = stateName;
    }
}
