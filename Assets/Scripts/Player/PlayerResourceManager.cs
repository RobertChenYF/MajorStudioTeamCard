using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerResourceManager : MonoBehaviour
{
    public static PlayerResourceManager instance;
    [SerializeField] private float defaultMaxBar;
    [SerializeField] private float defaultStartBarValue;
    private float attackBarMaxIncrement;
    private float drawBarMaxIncrement;
    private float currentAttackBarValue;
    private float currentDrawBarValue;
    [SerializeField]private float defaultAttackBarFillAmount;
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
        instance = this;
        currentAttackBarFillAmount = defaultAttackBarFillAmount + attackBarFillAmountIncrement;
        currentDrawBarFillAmount = defaultDrawBarFillAmount + drawBarFillAmountIncrement;

        currentAttackBarValue = defaultStartBarValue;
        currentDrawBarValue = defaultStartBarValue;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateResourceBarDisplay();
    }

    public void TimeCycleEnd()
    {
        currentAttackBarValue += currentAttackBarFillAmount;
        currentAttackBarValue = Mathf.Min(defaultMaxBar + attackBarMaxIncrement, currentAttackBarValue);
        currentDrawBarValue += currentDrawBarFillAmount;
        currentDrawBarValue = Mathf.Min(defaultMaxBar + drawBarMaxIncrement, currentDrawBarValue);
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

    public void ConsumeDrawBar(float value)
    {
        currentDrawBarValue -= value;
    }
    public void ConsumeAttackBar(float value)
    {
        currentAttackBarValue -= value;
    }
}
