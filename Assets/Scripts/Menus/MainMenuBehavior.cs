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

    public void MainMenuButton()
    {
        Manager.GetComponent<GameManager>().PlayGame = true;
    }
}
