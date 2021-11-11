using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapGenerator : MonoBehaviour
{
    public int max_zones = 10;
    public float offset = 1;

    public GameObject worldCellPrefab;

    List<WorldMapCell> cells = new List<WorldMapCell>();

    public void Start()
    {
        BuildSpiralMap(max_zones, max_zones);
        SolveCells();
    }

    void BuildSpiralMap(int X, int Y)
    {
        int index = 0;
        int x, y, dx, dy;
        x = y = dx = 0;
        dy = -1;
        int t = Mathf.Max(X, Y);
        int maxI = t * t;
        for (int i = 0; i < maxI; i++)
        {
            if ((-X / 2 <= x) && (x <= X / 2) && (-Y / 2 <= y) && (y <= Y / 2))
            {
                Vector3 position = new Vector3(x * offset, 0, y * offset);
                GameObject c = Instantiate(worldCellPrefab, position, Quaternion.identity);
                c.name = "c_" + index;
                c.transform.SetParent(this.transform);
                WorldMapCell wc = c.GetComponent<WorldMapCell>();
                wc.cell_distance = Mathf.Max(Mathf.Abs(x),Mathf.Abs(y));
                cells.Add(wc);
                index++;
            }
            if ((x == y) || ((x < 0) && (x == -y)) || ((x > 0) && (x == 1 - y)))
            {
                t = dx;
                dx = -dy;
                dy = t;
            }
            x += dx;
            y += dy;
        }
    }

    public void SolveCells()
    {
        // 0 are we 

        // 2 good fields
        // 4 food fiels 
        // 6 good fields 
        // n ... 
        for (int i = 0; i < cells.Count; i++)
        {
          
            WorldMapCell c = cells[i];
            if (i == 0)
            {
                // place us 
                c.BuildTile(0);
                continue;
            }

            // for the test 
            int idx = Random.RandomRange(1, 3);
            c.BuildTile(idx);
            continue;

        }



    }



}
