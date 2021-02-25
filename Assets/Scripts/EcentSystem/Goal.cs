using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public WhoScored whoScored;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnHeal()
    {
        Service.EventManager.Fire(new PlayerGainsHealth(4));
    }





    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Böll") // My fun for the week
        {
            Service.EventManager.Fire(new ScoreEvent(whoScored));
        }
    }
}
