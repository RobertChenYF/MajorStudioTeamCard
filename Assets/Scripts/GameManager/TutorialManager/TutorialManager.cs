using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    //private GameObject Klippy;
    //private GameObject GameManager;
    public DialogScriptObject[] Dialogs;

    public GameObject klippyBody;
    public GameObject klippyDialog;

    public GameObject arrow;
    private int count = 0;

    Color opacityChanger;

    private void Start()
    {
        TutorialButtonPress(); //do the first dialog
    }

    public void TutorialButtonPress()
    {
        ChangeDialog(Dialogs[count]);
        count++;
    }   

    private void ChangeDialog(DialogScriptObject dialog)
    {
        klippyBody.GetComponent<Image>().overrideSprite = dialog.klippyImage;
        klippyDialog.GetComponent<Text>().text = dialog.klippyDialog;

        if (dialog.arrowActive == true)
        {
            //opacityChanger.a = 1f;    
            //arrow.material.color.a = 1f;
            arrow.GetComponent<Renderer>().enabled = true;
        } else if (dialog.arrowActive == false)
        {
            //opacityChanger.a = 0;
            //arrow.GetComponent<Renderer>().material.color.a = opacityChanger.a;
            //arrow.material.color.a = 0f;
            arrow.GetComponent<Renderer>().enabled = false;
        }

        arrow.GetComponent<SpriteRenderer>().sprite = dialog.arrowImage;
        arrow.transform.position = dialog.arrowPosition;
        arrow.transform.rotation = dialog.arrowRotation;

    }

}
