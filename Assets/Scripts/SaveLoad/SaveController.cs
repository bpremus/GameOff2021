using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    [SerializeField]
    HiveGenerator hiveGenerator;

    [System.Serializable]
    public class Drone_data
    {
        public string name;
        public int bug_type;
        public float health;
        public float damage;
        public int bug_kill_count;
        public int bug_task_count;
        public int bug_base_level;
        public int sieged;

        public Drone_data(CoreBug bug)
        {
            this.name = bug.name;
            this.bug_type = (int) bug.bug_evolution;
            this.health = bug.health;
            this.damage = bug.damage;
            this.bug_kill_count = bug.bug_kill_count;
            this.bug_task_count = bug.bug_kill_count;
            this.bug_base_level = bug.bug_base_level;
            this.sieged = 0; // fix me
        }
    }

    [System.Serializable]
    public class Cell_data
    {
        public int room_type;
        public int room_position_x;
        public int room_position_y;
        public int max_asisgn;
        public float room_max_range;
        public Drone_data[] assigned_bugs;

        public Cell_data(HiveCell cell)
        {
            CoreRoom room = cell.GetRoom();

            room_type = GetRoomType(room);

            room_position_x = cell.x;
            room_position_y = cell.y;
            max_asisgn = room.GetMAxAssignUnits();
            room_max_range = room.GetRomRange();

            List<CoreBug> bugs = room.GetAssignedBugs();
            assigned_bugs = new Drone_data[bugs.Count];
            for (int i = 0; i < bugs.Count; i++)
            {
                CoreBug b = bugs[i];
                if (b)
                {
                    Drone_data drone = new Drone_data(b);
                    assigned_bugs[i] = drone;
                }
            }
        }
    }
    [System.Serializable]
    public class Hive_data
    {
        // player state

        public int day;
        public int food;
        public int wood;
        public int population;
        public Cell_data[] hive_cell;

        // enemy AI state 


        public Hive_data(int cell_size)
        {
            this.day = 0;
            this.food = 0;
            this.wood = 0;
            this.population = 0;
            hive_cell = new Cell_data[cell_size];
        }
    }

    [SerializeField]
    List<HiveCell> cells_to_save = new List<HiveCell>();
    public void Save()
    {
        Debug.Log("Saving game");
        cells_to_save.Clear();

        List<List<HiveCell>> cells = hiveGenerator.cells;
        for (int i = 0; i < cells.Count; i++)
        {
            for (int j = 0; j < cells[i].Count; j++)
            {
                HiveCell hs = cells[i][j];
                if (hs.GetRoom())
                {
                    cells_to_save.Add(hs);
                }
            }
        }

        if (cells_to_save.Count > 0)
        {
            OnSave(cells_to_save);
        }

        // save to actuall file 
        SaveToFile();
    }

    Hive_data hive_data;

    public void ClarHive()
    {
        FlushHive();
    }

    public void Load()
    {
        Debug.Log("Load game");

        LoadFromFile();

        if (hive_data.Equals(default(Hive_data)) == false)
        {
            Cell_data[] cells = hive_data.hive_cell;
            for (int i = 0; i < cells.Length; i++)
            {
                Cell_data cd = cells[i];
                HiveCell hc = hiveGenerator.BuildOnCell(cd.room_position_x, cd.room_position_y, GetRoomType(cd.room_type));
                CoreRoom cr = hc.GetRoom();
                cr.SetMaxUnits(cd.max_asisgn);
                cr.SetRoomRange(cd.room_max_range);

                // room stats 
                Drone_data[] drones = cd.assigned_bugs;

                for (int d = 0; d < drones.Length; d++)
                {
                    Drone_data drone = drones[d];
                    // build these drones and assign to this room;
                    ArtPrefabsInstance.Instance.SpawnBug((CoreBug.BugEvolution) drone.bug_type, hc);
                }
            }
        }
    }

 
    public void OnSave(List<HiveCell> cells_to_save)
    {
        int hive_size = cells_to_save.Count;
        hive_data = new Hive_data(hive_size);

        for (int i = 0; i < cells_to_save.Count; i++)
        {
            HiveCell cell = cells_to_save[i];
            if (cell)
            {
                Cell_data cell_data = new Cell_data(cell);
                hive_data.hive_cell[i] = cell_data;
            }
        }
    }

    public void FlushHive()
    {
        // rest hive 
        hiveGenerator.CleanGrid();
    }


    public static HiveCell.RoomContext GetRoomType(int index)
    {
        if (index == 1) return HiveCell.RoomContext.corridor;
        if (index == 2) return HiveCell.RoomContext.harvester;
        if (index == 3) return HiveCell.RoomContext.hive;
        if (index == 4) return HiveCell.RoomContext.queen;
        if (index == 5) return HiveCell.RoomContext.war;
        if (index == 6) return HiveCell.RoomContext.salvage;

        return HiveCell.RoomContext.empty;
    }

    public static int GetRoomType(CoreRoom room)
    {

        HiveCorridor hcorridor = room.GetComponent<HiveCorridor>();
        if (hcorridor)
        {
            return 1;
        }

        HarversterRoom hrom = room.GetComponent<HarversterRoom>();
        if (hrom)
        {
            return 2;
        }

        QueenRoom qroom = room.GetComponent<QueenRoom>();
        if (qroom)
        {
            return 3;
        }

        CommandCenter ccroom = room.GetComponent<CommandCenter>();
        if (ccroom)
        {
            return 4;
        }

        WarRoom wroom = room.GetComponent<WarRoom>();
        if (wroom)
        {
            return 5;
        }

        SalvageRoom srom = room.GetComponent<SalvageRoom>();
        if (srom)
        {
            return 6;
        }

        return 0;
    }


    // save nn to file 
    public void SaveToFile()
    {
        if (hive_data.Equals(default(Hive_data)) == false)
        {
            string _path = Application.persistentDataPath + "/GT2_bugs_save.gd";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(_path);
            bf.Serialize(file, hive_data);
            file.Close();
        }
    }

    // load from file 
    public void LoadFromFile()
    {
        string _path = Application.persistentDataPath + "/GT2_bugs_save.gd";
        if (File.Exists(_path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(_path, FileMode.Open);
            hive_data = (Hive_data)bf.Deserialize(file);
            file.Close();
        }
    }

}
