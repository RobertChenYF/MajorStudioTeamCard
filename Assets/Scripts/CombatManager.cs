using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    [Header("OtherScript")]
    [SerializeField] private PlayerResourceManager playerResource;

    [Header("Time Circle")]
    [SerializeField] private Image timeCycleDisplay;
    [SerializeField] private float timeCircleDuration;
    private float CycleSpeed = 1;
    private float CycleTimer = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeCycle();
    }


    public void UpdateTimeCycle()
    {
        if (CycleTimer >= timeCircleDuration)
        {
            CycleTimer -= timeCircleDuration;
            //trigger all time cycle function, fill resource, boss attack timer, card cooldown
            TimeCycleEndEvent();

        }
        timeCycleDisplay.fillAmount = CycleTimer / timeCircleDuration;
        CycleTimer += Time.deltaTime * CycleSpeed;
    }

    public void TimeCycleEndEvent()
    {
        playerResource.TimeCycleEnd();
    }

    public void PauseTimeCycle()
    {
        CycleSpeed = 0;
    }

    public void ContinueTimeCycle()
    {
        CycleSpeed = 1;
    }
}
