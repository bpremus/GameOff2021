using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMapCell : MonoBehaviour
{

    protected WorldMapGenerator generator;

    [SerializeField] Sprite[] prefab_icons;
    [SerializeField] Image res_image;
    [SerializeField] Text res_text;

    [SerializeField] List<WorldMapCell> borders = new List<WorldMapCell>();
    int x = 0;
    int y = 0;
    
    public void SetPos(int x, int y, WorldMapGenerator generator)
    {
        this.x = x;
        this.y = y;
        this.generator = generator;
    }

    public enum Cell_type { hidden, player_hive, hive, food, wood, empty };
    public Cell_type cell_Type = Cell_type.hidden;
    public float peral = 0;
    public float dist = 0;
    public int uuid = 0;

    public void OnBugVisit()
    {
        dist--;
    }

    public void SetNeighbours(List<List<WorldMapCell>> cells)
    {
        Debug.Log("setting neighbour");
        if (y > 0)
        {
            borders.Add(cells[x][y - 1]);
        }
        if (y < cells[0].Count - 1) 
        {
            borders.Add(cells[x][y + 1]);
        }

        if (x > 0)
        {
            borders.Add(cells[x - 1][y]);

            if (x % 2 == 0)
            {
                if (y < cells[0].Count - 1)
                    borders.Add(cells[x - 1][y + 1]);
            }
            else 
            {
                if (y > 0)
                    borders.Add(cells[x - 1][y - 1]);
            }
           
        }
        if (x < cells.Count - 1)
        {
            borders.Add(cells[x + 1][y]);

            if (x % 2 == 0)
            {
                if (y < cells[0].Count - 1)
                    borders.Add(cells[x + 1][y + 1]);
            }
            else
            {
                if (y > 0)
                    borders.Add(cells[x + 1][y - 1]);
            }
        }
    }

    protected void Update()
    {
        if (cell_Type == Cell_type.hidden)
        {
            this.GetComponentInChildren<Button>().gameObject.SetActive(false);
        }
        if (cell_Type == Cell_type.player_hive)
        {
            res_text.text = "hive";
        }
        if (cell_Type == Cell_type.food)
        {
            res_image.sprite = prefab_icons[0];
        }
        if (cell_Type == Cell_type.wood)
        {
            res_image.sprite = prefab_icons[1];
        }
        if (cell_Type == Cell_type.hive)
        {
            res_image.sprite = prefab_icons[3];
        }
    }

    public void ShowContent()
    {
        if (cell_Type == Cell_type.player_hive) return;

        res_text.text = "" + (int)dist +1;
        if (peral > 0.3 && peral < 0.32)
        {
            cell_Type = Cell_type.food;
        }
        else if (peral > 0.32 && peral < 0.35)
        {
            cell_Type = Cell_type.wood;
        }
        else if (peral > 0.4 && peral < 0.5)
        {
            cell_Type = Cell_type.food;
        }
        else if (peral > 0.367 && peral < 0.368)
        {
            cell_Type = Cell_type.hive;
        }
        else
        {
            cell_Type = Cell_type.empty;
            res_text.text = "";
        }
    }

    public void OnClick()
    {
        if (cell_Type == Cell_type.player_hive)
        {
            return;
        }
        generator.SetRoomAsDesitnation(this);
    }

    public void ExpandCell()
    {
        Debug.Log("on click " + borders.Count);
        for (int i = 0; i < borders.Count; i++)
        {
            WorldMapCell w = borders[i];
            w.ShowContent();
            w.gameObject.SetActive(true);
            
        }
    }

    public void SetSelectColor()
    {
        Button b = GetComponentInChildren<Button>();
        ColorBlock cb = b.colors;
        cb.normalColor = Color.red;
        b.colors = cb;
    }
}
