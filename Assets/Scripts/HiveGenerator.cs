using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveGenerator : MonoBehaviour
{
    [SerializeField]
    int width = 10;
    [SerializeField]
    int height = 10;
    [SerializeField]
    float offset = 1.2f;

    [SerializeField]
    GameObject cell_prefab;

    [SerializeField]
    public List<List<HiveCell>> cells = new List<List<HiveCell>>();

    public void Start()
    {
        //  BuildGrid();
        //  Setneighbours();
        //  BuildTopLevel();
        //  BuildOusideEntry();
        //  BuildDebugPath();
    }

    public void DeleteGrid()
    {

        for (int i = 0; i < this.transform.childCount; i++)
        {
            DestroyImmediate(this.transform.GetChild(i).gameObject);
        }

        cells.Clear();
    }

    public void GenerateStaticGrid()
    {
        // grid does not change
        BuildGrid();
        Setneighbours();
      //  BuildDebugPath();

    }

    public void BuildGrid()
    {
        for (int i = 0; i < width; i++)
        {
            List<HiveCell> rows = new List<HiveCell>();
            for (int j = 0; j < height; j++)
            {
                Vector3 pos = new Vector3(i * offset - width / 2, j * offset - height, 0);
                HiveCell c = CreateTile(pos);
                c.SetNode(i, j);
                rows.Add(c);
            }
            cells.Add(rows);
        }
    }

    public void Setneighbours()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                HiveCell hc = cells[i][j];
                if (hc)
                {
                    if (j + 1 < height)
                        hc.Setneighbour(cells[i][j + 1], 0);
                    if (i + 1 < width)
                        hc.Setneighbour(cells[i + 1][j], 1);
                    if (j - 1 > 0)
                        hc.Setneighbour(cells[i][j - 1], 2);
                    if (i - 1 > 0)
                        hc.Setneighbour(cells[i - 1][j], 3);
                }
            }
        }
    }

    public HiveCell CreateTile(Vector3 position)
    {
        GameObject g = Instantiate(cell_prefab, position, Quaternion.identity);
        if (g)
        {
            HiveCell c = g.GetComponent<HiveCell>();
            if (c)
            {
                g.transform.SetParent(this.transform);
                return c;
            }
        }
        return null;
    }

    [SerializeField]
    GameObject[] dbg_rooms;

    public void BuildDebugPath()
    {
        HiveCell a = cells[3][height - 1];
        a.BuildRoom(dbg_rooms[0]);
        a = cells[3][height - 2];
        a.BuildRoom(dbg_rooms[0]);
        a = cells[3][height - 3];
        a.BuildRoom(dbg_rooms[0]);
        a = cells[3][height - 4];
        a.BuildRoom(dbg_rooms[0]);
        a = cells[3][height - 5];
        a.BuildRoom(dbg_rooms[0]);
        a = cells[4][height - 5];
        a.BuildRoom(dbg_rooms[0]);
        a = cells[5][height - 5];
        a.BuildRoom(dbg_rooms[0]);
        a = cells[6][height - 5];
        a.BuildRoom(dbg_rooms[1]);
    }

}
