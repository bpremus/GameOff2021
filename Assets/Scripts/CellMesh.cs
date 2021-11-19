using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellMesh : MonoBehaviour
{
    [SerializeField]
    public int[] connections = new int[4] { 0, 0, 0, 0 };

    public enum Cell_type {dirt, top, corridor, room, entrance, outside};

    public Cell_type cell_Type = Cell_type.dirt;
    

}
