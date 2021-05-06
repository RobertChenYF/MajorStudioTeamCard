using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopIntentToolTip : MonoBehaviour
{
    [SerializeField]private GameObject intentText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        intentText.SetActive(true);
    }

    private void OnMouseExit()
    {
        intentText.SetActive(false);
    }
}
