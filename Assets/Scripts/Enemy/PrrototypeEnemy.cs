using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrrototypeEnemy : Enemy
{

    [SerializeField] private float enemyMaxHp;
    [SerializeField] private TextMeshPro HpText;
    // Start is called before the first frame update

    public override void Start()
    {
        base.Start();
        maxHp = enemyMaxHp;
        currentHp = maxHp;
        enemyHpText = HpText;
        enemyHpText.text = "Enemy Hp: " + currentHp.ToString();
    }

}
