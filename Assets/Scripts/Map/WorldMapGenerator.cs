using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapGenerator : MonoBehaviour
{

    public GameObject Hexagon;
    public uint Radius;
    public float HexSideMultiplier = 1;
    private const float sq3 = 1.7320508075688772935274463415059F;

    List<Vector3> tiles = new List<Vector3>();
    List<int> distance = new List<int>();
    void Start()
    {
        Vector3 currentPoint = transform.parent.position;
       if (Hexagon.transform.localScale.x != Hexagon.transform.localScale.z) return;
 
        Vector3[] mv = {
            new Vector3(1.5f,0, -sq3*0.5f),       
            new Vector3(0,0, -sq3),               
            new Vector3(-1.5f,0, -sq3*0.5f),      
            new Vector3(-1.5f,0, sq3*0.5f),       
            new Vector3(0,0, sq3),                
            new Vector3(1.5f,0, sq3*0.5f)         
        };

        int lmv = mv.Length;
        float HexSide = Hexagon.transform.localScale.x * HexSideMultiplier;
        int range = 0;
        for (int mult = 0; mult <= Radius; mult++)
        {
            int hn = 0;
            for (int j = 0; j < lmv; j++)
            {
                for (int i = 0; i < mult; i++, hn++)
                {
                    distance.Add(range);
                    tiles.Add(currentPoint);
                    currentPoint += (mv[j] * HexSide);
                }
                if (j == 4)
                {
                    distance.Add(range);
                    tiles.Add(currentPoint);
                    currentPoint += (mv[j] * HexSide);
                    hn++;

                    range++;

                    if (mult == Radius)
                        break;   
                }
            }
        }


        // close enoug distance
        for (int i = 0; i < tiles.Count; i++)
        {
            Vector3 pos = tiles[i];
            Vector3 rot_pos = new Vector3(pos.x, pos.z, 0) + new Vector3(0, Screen.height / 2 , 0); 
            GameObject wc = Instantiate(Hexagon, rot_pos, Quaternion.identity, transform);      
           // wc.cell_distance = Mathf.FloorToInt(distance[i]);
           // wc.name = "m_" + wc.cell_distance;
           // wc.BuildTile(0);
        }

        
    }

}
