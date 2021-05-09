using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuBehavior : MonoBehaviour
{
    private GameObject Manager;
    // Start is called before the first frame update
    void Start()
    {
        Manager = GameObject.FindWithTag("GameManager");
    }

    public void ResumeButton()
    {
        Manager.GetComponent<GameManager>().OpenPauseMenu = false;
    }

    public void MainMenuButton()
    {
        Manager.GetComponent<GameManager>().SwitchScenes("MainMenu", GameManager.StateType.MainMenu);
        Manager.GetComponent<GameManager>().OpenPauseMenu = false;
    }

    public void QuitButton()
    {
        Manager.GetComponent<GameManager>().ExitGame();
    }
}
