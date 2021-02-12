using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy: MonoBehaviour
{
    protected float maxHp;
    protected float currentHp;
    protected TextMeshPro enemyHpText;
    private void Start()
    {
       // currentHp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        enemyHpText.text = "Enemy Hp: " + currentHp.ToString();
        if (currentHp <= 0)
        {
            Debug.Log("enemy died");
        }
    }

}
