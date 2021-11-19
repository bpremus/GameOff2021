using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapCell : MonoBehaviour
{

    public GameObject[] worldCellPrefabs;

    public int cell_distance = 0;
    GameObject map_mesh;
    GameObject map_mesh_selected;

    int hover = 0;
    public void Update()
    {
        if (hover == 0)
        {
            map_mesh_selected.gameObject.SetActive(false);
            map_mesh.gameObject.SetActive(true);
        }
        else
        {
            map_mesh_selected.gameObject.SetActive(true);
            map_mesh.gameObject.SetActive(false);
        }

        hover = 0;
    }

    public void BuildTile(int index)
    {
        int idx = 0;
        int idx_hover = 1;
        if (cell_distance == 0)
        {
            idx = 2;
            idx_hover = 2;
        }

        map_mesh = Instantiate(worldCellPrefabs[idx], transform.position, Quaternion.identity);
        map_mesh.transform.SetParent(this.transform);

        map_mesh_selected = Instantiate(worldCellPrefabs[idx_hover], transform.position, Quaternion.identity);
        map_mesh_selected.transform.SetParent(this.transform);
        map_mesh_selected.gameObject.SetActive(false);
    }

    public void OnHover()
    {
        if (cell_distance != 0)
            hover = 1;
    }

    public void OnSelect()
    { 
    
    }



}
