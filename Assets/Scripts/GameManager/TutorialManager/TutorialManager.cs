using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    //private GameObject Klippy;
    //private GameObject GameManager;
    public DialogScriptObject[] Dialogs;
    //public DialogScriptObject[] AdvDialogs;
    public DialogScriptObject InitialDialog;

    public GameObject klippyBody;
    public GameObject klippyDialog;
    public GameObject TutorialPositionText;
    public GameObject Hold;
    public GameObject Back;
    public GameObject Next;
    public GameObject[] specialEventHolder;
    //public GameObject[] arrows;

    //public bool advanced;
    private GameObject arrow;
    private int count = -1;
    //private int arrowCount = -1;
    private float timer = 0f;
    private string workingString;
    private string workedString;
    private int dialogLength = 0;

    Color opacityChanger;

    private void Start()
    {
        //TutorialButtonPress(); //do the first dialog
        dialogLength = Dialogs.Length + 1;
        ChangeDialog(InitialDialog);
    }

    public void TutorialButtonPress()
    {
        count++;
        ChangeDialog(Dialogs[count]);
    }

    public void BackButtonPress()
    {
        count--;
        ChangeDialog(Dialogs[count]);
        Hold.SetActive(false);
    }

    private void ChangeDialog(DialogScriptObject dialog)
    {
        klippyBody.GetComponent<Image>().overrideSprite = dialog.klippyImage;
        klippyDialog.GetComponent<TextMeshProUGUI>().text = dialog.klippyDialog;

        if (dialog.holdPlayer == true)
        {
            Hold.SetActive(true);
            timer = 3;
        } 

        if (dialog.arrowActive == true)
        {
            //opacityChanger.a = 1f;    
            //arrow.material.color.a = 1f;
            //arrowCount++;
            //dialog.arrow.SetActive(true);
        } else if (dialog.arrowActive == false)
        {
            //opacityChanger.a = 0;
            //arrow.GetComponent<Renderer>().material.color.a = opacityChanger.a;
            //arrow.material.color.a = 0f;
            //dialog.arrow.SetActive(false);
        }

        //arrow.GetComponent<Image>().overrideSprite = dialog.arrowImage;
        //arrow.GetComponent<RectTransform>().localPosition = dialog.arrowPosition;
        //arrow.transform.rotation = dialog.arrowRotation;

    }

    private void Update()
    {
        if (count == 0)
        {
            Back.SetActive(false);
        } else if (count >= 1)
        {
            Back.SetActive(true);
        }

        if (count == Dialogs.Length)
        {
            Next.SetActive(false);
        } else
        {
            Next.SetActive(true);
        }
        TutorialPositionText.GetComponent<TextMeshProUGUI>().text = $"{count + 2}/{dialogLength + 1}";

        if (Dialogs[count].specialEvent == true)
        {
            foreach (GameObject element in specialEventHolder)
            {
                element.SetActive(true);
            }
        }
    }

    private void FixedUpdate()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        } else if (timer <= 0)
        {
            Hold.SetActive(false);
        }
    }

}
