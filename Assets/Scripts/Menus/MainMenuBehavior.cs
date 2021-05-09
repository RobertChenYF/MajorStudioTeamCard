using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBehavior : MonoBehaviour
{
    public GameObject MainMenu;
    private GameObject Manager;
    // Start is called before the first frame update
    void Start()
    {
        Manager = GameObject.FindWithTag("GameManager");
        Debug.Log(Manager);
    }

    public void PlayButton()
    {
        Manager.GetComponent<GameManager>().PlayGame = true;
    }

    public void GlossaryButton()
    {
        Manager.GetComponent<GameManager>().ShowGlossary = true;
    }

    public void TutorialButton()
    {
        Manager.GetComponent<GameManager>().PlayTutorial = true;
    }

    public void QuitButton()
    {
        Manager.GetComponent<GameManager>().ExitGame();
    }
}
