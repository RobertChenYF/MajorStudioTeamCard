using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    [Header("Time Circle")]
    [SerializeField] private Image timeCycleDisplay;
    [SerializeField] private float timeCircleDuration;
    private float CycleSpeed = 1;
    private float CycleTimer = 0;

    [Header("Manager Script")]
    [SerializeField] private PlayerActionManager actionManager;
    [SerializeField] private PlayerStatsManager statsManager;
    [SerializeField] private PlayerResourceManager resourceManager;
    // Start is called before the first frame update
    private void Awake()
    {
        Services.eventManager = new EventManager();
        Services.combatManager = this;
        Services.actionManager = actionManager;
        Services.resourceManager = resourceManager;
        Services.statsManager = statsManager;

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeCycle();
    }

    public class TimeCycleEnd: AGPEvent
    {
        
        public TimeCycleEnd()
        {
            
        }
    }

    public void UpdateTimeCycle()
    {
        if (CycleTimer >= timeCircleDuration)
        {
            CycleTimer -= timeCircleDuration;
            //trigger all time cycle function, fill resource, boss attack timer, card cooldown
            Services.eventManager.Fire(new TimeCycleEnd());

        }
        timeCycleDisplay.fillAmount = CycleTimer / timeCircleDuration;
        CycleTimer += Time.deltaTime * CycleSpeed;
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

public static class Services
{
    public static PlayerResourceManager resourceManager;
    public static PlayerStatsManager statsManager;
    public static CombatManager combatManager;
    public static EventManager eventManager;
    public static PlayerActionManager actionManager;
}
