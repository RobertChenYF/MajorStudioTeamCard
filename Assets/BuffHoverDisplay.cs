using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuffHoverDisplay : MonoBehaviour
{
    public Buff thisBuff;
    private string buffDescription;
    [SerializeField]private TextMeshPro stackdisplay;
    [SerializeField] private GameObject textBox;
    [SerializeField] private TextMeshPro textDescription;
    private int stack = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCount(int newCount)
    {
        stack = newCount;
        if (newCount == 1)
        {
            stackdisplay.text = "";

        }
        else
        {
            stackdisplay.text = newCount.ToString();
        }
    }
    public void MakeBuff()
    {
        buffDescription = thisBuff.getBuffDescription();
        GetComponent<SpriteRenderer>().sprite = thisBuff.getBuffIcon();
        textDescription.text = buffDescription;
        textBox.SetActive(false);

    }
    private void OnMouseEnter()
    {
        textBox.SetActive(true);
    }
    private void OnMouseExit()
    {
        textBox.SetActive(false);
    }
}
