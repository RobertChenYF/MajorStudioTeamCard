using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSquareControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Services.actionManager.currentTargetEnemy != null)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            transform.position = Services.actionManager.currentTargetEnemy.transform.position;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
        
    }
}
