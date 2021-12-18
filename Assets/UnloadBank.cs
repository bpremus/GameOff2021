using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnloadBank : MonoBehaviour
{

    public void onClick()
    {
        FMODUnity.RuntimeManager.UnloadBank("Master");
    }
}
