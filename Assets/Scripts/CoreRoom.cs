using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreRoom : MonoBehaviour
{
    protected HiveCell parent_cell;

    public HiveCell cell{
        get { return this.parent_cell;  }
        set { this.parent_cell = value; }
    }


}
