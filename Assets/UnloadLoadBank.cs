using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnloadLoadBank : MonoBehaviour
{

    public void onClick()
    {
        FMODUnity.RuntimeManager.UnloadBank("Master");
        FMODUnity.RuntimeManager.LoadBank("Master");
    }
}
