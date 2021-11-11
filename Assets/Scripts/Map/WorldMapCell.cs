using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapCell : MonoBehaviour
{

    public GameObject[] worldCellPrefabs;

    public int cell_distance = 0;

    public void BuildTile(int index)
    {
        GameObject g = Instantiate(worldCellPrefabs[index], transform.position, Quaternion.identity);
        g.transform.SetParent(this.transform);
    }

}
