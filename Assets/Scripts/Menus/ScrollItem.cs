using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollItem : MonoBehaviour
{
    private GameObject imageChild;
    private GameObject textChild;


    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "ScrollBackground")
            {
                imageChild = child.gameObject;
            }
            else if (child.name == "Text")
            {
                textChild = child.gameObject;
            }

        }

        //imageChild.GetComponent<RectTransform>(). = 100;
        //imageChild.Height = 100;
    }
}
