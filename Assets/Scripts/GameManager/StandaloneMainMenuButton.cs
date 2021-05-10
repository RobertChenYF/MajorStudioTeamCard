using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandaloneMainMenuButton : MonoBehaviour
{
    private GameObject Manager;
    // Start is called before the first frame update
    void Start()
    {
        Manager = GameObject.FindWithTag("GameManager");
        Debug.Log(Manager);
    }


    public void MainMenuButton()
    {
        Manager.GetComponent<GameManager>().SwitchScenes("MainMenu", GameManager.StateType.MainMenu);
        Manager.GetComponent<GameManager>().OpenPauseMenu = false;
    }
}
