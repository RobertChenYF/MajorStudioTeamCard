using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum StateType
    {
        MAINMENU,      //Main menu state
        STAGING,      //When the player is in the staging area between encounters. Here is where the player picks their deck or picks the next encounter.
        ENCOUNTER,    //When the player is in an encounter
        UNKNOWN,      //A catch all state
    }

    StateType currentState;

    bool playGame = false;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        currentState = StateType.MAINMENU;
    }

    void Update()
    {
        switch (currentState)
        {
            case StateType.MAINMENU:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SceneManager.LoadScene("Staging");
                    currentState = StateType.STAGING;
                }
                break;
            case StateType.STAGING:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SceneManager.LoadScene("Encounter");
                    currentState = StateType.ENCOUNTER;
                }
                break;
            case StateType.ENCOUNTER:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SceneManager.LoadScene("Staging");
                    currentState = StateType.STAGING;
                }
                break;
            case StateType.UNKNOWN:
                //empty
                break;
            default:
                print("Incorrect intelligence level.");
                break;
        }
    }


}
