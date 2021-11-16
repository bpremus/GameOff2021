using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtPrefabsInstance : MonoBehaviour
{
    // changed to singleton 
    private static ArtPrefabsInstance _instance;
    public static ArtPrefabsInstance Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
    }

    
    public GameObject[] RoomPrefabs;
    public GameObject[] BugsPrefabs;

}
