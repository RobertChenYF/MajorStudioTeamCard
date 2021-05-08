using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossFollower : Enemy
{
    public Enemy Mainbody;

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
