using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField]
    HiveCell start_cell;

    [SerializeField]
    HiveCell target_cell;

    int coalition = 1;
    int day_attack_number = 1;
    int night_attack_number = 10;

    GameController game_controller;

    public void Start()
    {
        game_controller = GameController.Instance;
    }

    [SerializeField]
    float spawn_timer = 0;

    public void OnDayAttacks()
    {
        SetAttack();

        spawn_timer += Time.deltaTime;
        if (spawn_timer > 2)
        {
            spawn_timer = 0;
        }
        else
            return;

        SpawnBug();
    }

    public void OnNightAttacks()
    {
        SetAttack();

        spawn_timer += Time.deltaTime;
        if (spawn_timer > 2)
        {
            spawn_timer = 0;
        }
        else
            return;

        SpawnBug();
    }


    public void StageAttack()
    {
        // player condition have to be met 
        // have a queen
        // have a harvester 
        // have one combat unit
        // cycle will start

        // day scout 
        // ------------------------------
        if (game_controller.ISDayCycle())
        {
            OnDayAttacks();
        }
        else
        {
            OnNightAttacks();
        }
    }

  



    public void SetAttack()
    {
        HiveGenerator hc = FindObjectOfType<HiveGenerator>();
        if (hc)
        {
            if (hc.cells.Count > 0)
            {
                int[] size = hc.GetSize();
                start_cell = hc.cells[size[0] -1][size[1] -1];
                target_cell = hc.hive_cell;
            }
        }
    }


    public void Update()
    {
        StageAttack();
    }

    public void SpawnBug()
    {
        GameObject bug_prefab = ArtPrefabsInstance.Instance.BugsPrefabs[0];

        CoreBug cb = Instantiate(bug_prefab, start_cell.transform.position, start_cell.transform.rotation).GetComponent<CoreBug>();
       if (cb != null)
       {
            cb.CurrentPositon(start_cell);
            cb.GoToAndBack(start_cell,target_cell);
            cb.tag = "Enemy";
            cb.coalition = coalition; // same as tag 

            // cb.stop_and_fight = false;
            //cb.GoTo(target_cell);
        }
    }
}

