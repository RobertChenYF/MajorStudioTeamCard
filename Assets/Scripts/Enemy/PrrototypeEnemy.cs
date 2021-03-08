using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrrototypeEnemy : Enemy
{

    [SerializeField] private float enemyMaxHp;
    
    // Start is called before the first frame update

    public override void Start()
    {
        
        maxHp = enemyMaxHp;
        currentHp = maxHp;
        base.Start();
        
    }

}
