using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerResourceManager : MonoBehaviour
{

    [SerializeField] private float defaultMaxBar;
    [SerializeField] private float defaultStartBarValue;

    private float attackBarMaxIncrement;
    private float drawBarMaxIncrement;
    private float currentAttackBarValue;
    private float currentDrawBarValue;
    [SerializeField] private float defaultAttackBarFillAmount;
    [SerializeField] private float defaultDrawBarFillAmount;
    private float attackBarFillAmountIncrement = 0;
    private float drawBarFillAmountIncrement = 0;
    private float currentAttackBarFillAmount;
    private float currentDrawBarFillAmount;

    [Header("Bar UI")]
    [SerializeField] private Image attackBarDisplay;
    [SerializeField] private TextMeshProUGUI attackBarText;
    [SerializeField] private Image drawBarDisplay;
    [SerializeField] private TextMeshProUGUI drawBarText;

    // Start is called before the first frame update
    void Start()
    {

        currentAttackBarFillAmount = defaultAttackBarFillAmount + attackBarFillAmountIncrement;
        currentDrawBarFillAmount = defaultDrawBarFillAmount + drawBarFillAmountIncrement;

        currentAttackBarValue = defaultStartBarValue;
        currentDrawBarValue = defaultStartBarValue;

        Services.eventManager.Register<CombatManager.TimeCycleEnd>(TimeCycleEndRefillResourceBar);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateResourceBarDisplay();
    }



    public void TimeCycleEndRefillResourceBar(AGPEvent e)
    {
        GainAttackBar(currentAttackBarFillAmount);
        GainDrawBar(currentDrawBarFillAmount);
    }

    public void GainAttackBar(float amount)
    {
        currentAttackBarValue += amount;
        currentAttackBarValue = Mathf.Min(defaultMaxBar + attackBarMaxIncrement, currentAttackBarValue);
        Services.actionManager.UpdateCardCanPlay();
    }

    public void GainDrawBar(float amount)
    {
        currentDrawBarValue += amount;
        currentDrawBarValue = Mathf.Min(defaultMaxBar + drawBarMaxIncrement, currentDrawBarValue);
        Services.actionManager.UpdateCardCanPlay();
    }

    public void UpdateResourceBarDisplay()
    {
        attackBarDisplay.fillAmount = currentAttackBarValue / defaultMaxBar;
        attackBarText.text = currentAttackBarValue.ToString();
        drawBarDisplay.fillAmount = currentDrawBarValue / defaultMaxBar;
        drawBarText.text = currentDrawBarValue.ToString();
    }

    public bool CheckAttackBar(float value)
    {
        if (value <= currentAttackBarValue)
        {
            return true;
        }
        else return false;
    }

    public bool CheckDrawBar(float value)
    {
        if (value <= currentDrawBarValue)
        {
            return true;
        }
        else return false;
    }

    public void resetResource()
    {
        currentAttackBarValue = defaultStartBarValue;
        currentDrawBarValue = defaultStartBarValue;
    }
    public void ConsumeDrawBar(float value)
    {
        currentDrawBarValue -= value;
        Services.actionManager.UpdateCardCanPlay();
    }
    public void ConsumeAttackBar(float value)
    {
        currentAttackBarValue -= value;
        Services.actionManager.UpdateCardCanPlay();
    }
}
