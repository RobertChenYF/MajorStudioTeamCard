using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

    }
    public override void Die()
    {
        //end current battle
        base.Die();
        
    }
}
