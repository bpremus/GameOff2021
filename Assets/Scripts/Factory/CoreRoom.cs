using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreRoom : MonoBehaviour
{
    [SerializeField]
    protected HiveCell parent_cell;

    public virtual void Start()
    {
        
    }

    public HiveCell cell{
        get { return this.parent_cell;  }
        set { this.parent_cell = value; }
    }

    public virtual void DetectEnemy()
    {

    }

    public virtual void Update()
    {
        DetectEnemy();
    }


}
