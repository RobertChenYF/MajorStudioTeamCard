using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBehavior : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject KlippyMain;
    public GameObject Glossary;
    private GameObject Manager;
    private bool showGlossary = false;
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
        showGlossary = true;
    }

    public void TutorialButton()
    {   
        if (KlippyMain.activeInHierarchy != true)
        {
            KlippyMain.SetActive(true);
        } else if (KlippyMain.activeInHierarchy == true)
        {
            KlippyMain.SetActive(false);
        }
        //Manager.GetComponent<GameManager>().PlayTutorial = true;
    }

    public void SimpleTutorialButton()
    {
        Manager.GetComponent<GameManager>().PlayTutorial = true;
    }

    public void AdvancedTutorialButton()
    {
        Manager.GetComponent<GameManager>().AdvancedTutorial = true;
    }

    public void QuitButton()
    {
        Manager.GetComponent<GameManager>().ExitGame();
    }

    public void ReturnButton()
    {
        showGlossary = false;
    }

    public void Update()
    {
        if (showGlossary == false)
        {
            Glossary.SetActive(false);
        }
        else if (showGlossary == true)
        {
            Glossary.SetActive(true);
        }
    }
}
