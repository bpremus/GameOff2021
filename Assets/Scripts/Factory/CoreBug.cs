using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoreBug : BugMovement
{
    [HideInInspector]
    public Queue<HiveCell> path = new Queue<HiveCell>();
    [SerializeField]
    public HiveCell current_cell = null;
    [HideInInspector]
    public HiveCell destination_cell = null;
    [HideInInspector]
    public HiveCell target_cell = null;
    [SerializeField]
    public HiveCell asigned_cell = null;
    [SerializeField]
    public HiveCell underlaying_cell = null;

    public enum BugEvolution { drone, super_drone, warrior, claw, range, cc_bug };
    public BugEvolution bug_evolution = BugEvolution.drone;

    // stats 
    [Header("Bug stats")]
    public float health = 10f;
    public float damage = 1f;
    public float extra_damage = 0;
    public float interraction_range = 1f;
    public int   decayOnDeadTimer = 20;
    public int   bug_base_level = 1;

    // bug perfo
    public int   bug_kill_count = 0;
    public int   bug_task_count = 0;

    public void LevelUp() { 
        
        bug_base_level++;
        float m1 = bug_base_level * 0.03f;
        float m2 = bug_base_level * 0.01f;
        float m3 = bug_base_level * 0.003f;
        BoostSpeed (m3);
        BoostHealth(m1);
        BoostDamage(m2);

        OnLevelUp();
    }

    public void OnLevelUp()
    {
        Debug.Log("LEVEL UP");
    }

    protected void BoostSpeed(float speed_multiply)
    {
        this.move_speed += speed_multiply;
        if (this.move_speed > 4) this.move_speed = 4;
    }
    protected void BoostHealth(float health_multiply)
    {
        this.health += health_multiply;
        if (this.health > 50) this.health = 50;
    }
    protected void BoostDamage(float dmg_multiply)
    {
        this.damage += dmg_multiply;
    }
    protected void BoostRange(float range_multiply)
    {
        this.interraction_range += range_multiply;
    }

    public void AIBoost(float speed, float health)
    {
        BoostSpeed(speed);
        BoostHealth(health);
    }

    // offset to camera above cells 
    // may not be needed once tiles are replaced with mesh
    protected Vector3 z_offset = new Vector3(0, 0, -0.1f);

    public enum BugTask { none, fight, salvage, harvesting }
    public BugTask bugTask = BugTask.none;

    public enum Bug_action { idle, traveling, gathering, fighting, salvaging, returning, dead };
    [SerializeField]
    protected Bug_action bug_action = Bug_action.idle;

    public Bug_action GetAction { get => bug_action; }

    public int coalition = 0;

    public virtual void SiegeBug(bool siege) { }
    public virtual bool GetSiegeState() { return false; }

    public float GetMoveSpeed() { return move_speed; }

    public void OnBugLoad(string name, float health, float damage, float speed, int kill_count, int task_count, int base_level, bool sieged)
    {
        this.name       = name;
        this.health     = health;
        this.damage     = damage;
        this.move_speed     = speed;
        this.bug_kill_count = kill_count;
        this.bug_task_count = task_count;
        this.SiegeBug(sieged);
    }

    //  moving to dead bug
    protected CoreBug salvagedBug;
    public CoreBug salvage_object
    {
        set { salvagedBug = value; }
        get { return salvagedBug; }
    }

    protected GameObject harvestedObject;
    public GameObject harvest_object
    {
        set { harvestedObject = value; }
        get { return harvestedObject; }
    }

    public virtual void SetAction(Bug_action action)
    {
        bug_action = action;
    }
    public virtual void NextAction()
    {
        if (bug_action == CoreBug.Bug_action.idle)
            SetAction(Bug_action.traveling);
        else if (bug_action == CoreBug.Bug_action.traveling)
            SetAction(Bug_action.gathering);
        else if (bug_action == CoreBug.Bug_action.gathering)
            SetAction(Bug_action.returning);
        else if (bug_action == CoreBug.Bug_action.returning)
            SetAction(Bug_action.idle);
    }

    public virtual void AssignToAroom(HiveCell cell)
    {
        cell.AssignDrone(this);
    }

    public override void SetAnimation()
    {
        base.SetAnimation();
    }

    public virtual void OnInteract(CoreBug otherBug)
    {
     
        // Debug.Log("we got interraced by " + otherBug.name);

        // override me with appropriate behaviour 
        if (GetState() == BugAnimation.dead)
        {
            Debug.Log("someone is interracting with a dead enemy");
            Destroy(this.gameObject);
            return;
        }

        // if other is enemy
        OnTakeDamage(otherBug);
    }

    public virtual void OnTakeDamage(CoreBug otherBug)
    {
        health -= otherBug.damage;
        if (health <= 0) LateDie();
    }

    public virtual void OnDestinationReach()
    {
        //  Debug.Log("destination reached");

        if (ai_task != null)
        {
            ai_task.OnDestinationReach();
        }
    }

    public void OnAIEndDestroy()
    {

        if (harvest_object)
            Destroy(harvest_object);

        OnLateDecay();
    }

    public virtual void OnLateDie()
    {
        // Debug.Log("died");
        FlipBug();

        if (_isDead == true) return;

        if (harvest_object)
            Destroy(harvest_object);

        // enemy bug report where they died 
        if (coalition > 0)
        {
            underlaying_cell.dCost++; // deadly cell
        }

        // else 
        bugAnimation = BugAnimation.dead;
        Invoke("OnLateDecay", decayOnDeadTimer);
        _isDead = true;

        OnBugIsDead(); // will be called once 
    }

    public bool IsDead()
    {
        return _isDead;
    }

    public virtual void OnBugIsDead()
    { 
        // you can override this one instead 

    }
 
    public void OnLateDecay()
    {
        Destroy(this.gameObject);
    }


    public virtual void OnTargetReach()
    {
      //  Debug.Log("target reached");

        if (ai_task != null)
        {
            ai_task.OnTargetReach();
        }

    }
    protected EnemyController.AITask ai_task;

    public void SetAITask(EnemyController.AITask task)
    {
        ai_task = task;
    }

    public override void OnWalk()
    {
        base.OnWalk();

        // we can cary somethign if we have something 
        if (harvest_object)
            harvest_object.transform.position = transform.position + transform.up * 0.5f;


       // if (salvage_object)
       //     salvage_object.transform.position = transform.position + transform.up * 0.5f;
       //

    }

    public void CurrentPositon(HiveCell position)
    {
        current_cell = position;
        transform.position = current_cell.transform.position + z_offset;
        this.target = transform.position;
    }

    public void StopPath()
    {
        path.Clear();
    }

    public void GoToAndBack(HiveCell start, HiveCell destination, float wait_timer = 0)
    {
        // if (destination_cell == start) return;

        path.Clear();
        target_cell = destination;
        destination_cell = start;

        List<HiveCell> move_path = AiController.GetPath(start, destination, coalition); // use different path for enemy 
        for (int i = 0; i < move_path.Count; i++)
        {
            path.Enqueue(move_path[i]);
        }

        path.Enqueue(destination);
        move_path.Reverse();

        for (int i = 0; i < move_path.Count; i++)
        {
            path.Enqueue(move_path[i]);
        }

        this.target = current_cell.transform.position + z_offset;
    }

    public void GoToAndIdle(HiveCell destination)
    {
        GoTo(destination);
    }

    public void GoTo(HiveCell destination)
    {
        if (destination_cell == destination) return;

        path.Clear();
        target_cell = destination;
        destination_cell = destination;

        path.Enqueue(current_cell);

        List<HiveCell> move_path = AiController.GetPath(current_cell, destination);
        for (int i = 1; i < move_path.Count; i++)
        {
            path.Enqueue(move_path[i]);
        }

        this.target = current_cell.transform.position + z_offset;
    }

    float _last_dist = 0;
    public override void WalkPath()
    {
        // dbg
        underlaying_cell = GetUndelayingCell();

        if (path.Count > 0)
        {
            HiveCell c = path.Peek();

            Vector3 next_cell_pos = c.transform.position;

            // move bugs outside
            if (c.cell_Type == CellMesh.Cell_type.outside)
            {
                // offset to bottom
                next_cell_pos += new Vector3(0, -1f, 0);
            }

            // inside 
            if (c.cell_Type == CellMesh.Cell_type.corridor)
            {
                // slight offset in direction 

                float ang = Vector3.SignedAngle(transform.forward, Vector3.up, z_offset);
                if (ang > 0)
                {
                    next_cell_pos += new Vector3(-0.1f, -0.1f, 0);
                }
                else
                {
                    next_cell_pos += new Vector3(0.1f, 0.1f, 0);
                }
            }

            float t = Vector3.Distance(next_cell_pos + z_offset, transform.position);
            if (t <= stop_distance || _last_dist == t)
            {
                current_cell = path.Dequeue();
                if (path.Count > 0)
                {
                    if (target_cell == underlaying_cell)
                    {
                        OnTargetReach();
                    }
                }
                else
                {
                    current_cell = target_cell;
                    target = current_cell.transform.position + z_offset;
                    OnDestinationReach();
                }
                return;
            }

            _last_dist = t;
            this.target = next_cell_pos + z_offset;
        }
        else
        {
            if (destination_cell != null)
                this.target = destination_cell.transform.position + z_offset;
        }
    }


    public void LateDie()
    {
        // unregister bug 
        OnLateDie();
            

        // place dead bug 
    }

    [SerializeField]
    Collider[] hitColliders;

    public virtual void OnCombatStop()
    {

    }

    public override void OnWalkStart()
    {
       
    }

    public override void OnIdleStart()
    {
     
    }

    public virtual void StopInteracitonWithEnemy()
    {
     //  if (bugAnimation == BugAnimation.attack)
     //  {
     //      bugAnimation = BugAnimation.idle;
     //      OnCombatStop();
     //  }
    }

    public virtual void InteractWithEnemy(CoreBug otherBug)
    {
        // single enemy
    }

    public virtual void InteractWithEnemies(List<CoreBug> othrBugs)
    {
       // multiple enemies at the same time
    }


    float _interact_t = 0;
    public override void SetTimers()
    {
        _interact_t += Time.deltaTime;
    }

    [SerializeField]
    protected int splash_dmg = 1;
    public override void DetectEnemy()
    {
        if (bugTask == BugTask.none) return;

        int layerId = 7; //bugs
        int layerMask = 1 << layerId;

        hitColliders = Physics.OverlapSphere(transform.position, interraction_range, layerMask);
        hitColliders = hitColliders.OrderBy((d) => (d.transform.position -
        transform.position).sqrMagnitude).ToArray();

        List<CoreBug> bugs_to_interract = new List<CoreBug>();

        int cnt = 0;
        foreach (var hitCollider in hitColliders)
        {
            CoreBug cb = hitCollider.GetComponent<CoreBug>();
            if (cb)
            {
                // we do not interract with ourself and we dont do anything if we are idle
                if (cb == this) continue;
                

                if (bugTask == BugTask.fight)
                {
                    // enemy is dead 
                    if (cb._isDead == true) continue;
                    if (cb.coalition == coalition) continue;
                    
                    bug_action = Bug_action.fighting;
                    InteractWithEnemy(cb);
                    bugs_to_interract.Add(cb);
                    cnt++;

                }
            }
        }

        if (cnt > 0)
            InteractWithEnemies(bugs_to_interract);

        // if we are here we stop
        if (bug_action == Bug_action.fighting && cnt == 0)
        {
            bug_action = Bug_action.idle;
            StopInteracitonWithEnemy();
        }      
    }

    private MaterialPropertyBlock _block;
    private Renderer[] _renderer;

    protected virtual void Awake()
    {
        _block = new MaterialPropertyBlock();
        _renderer = GetComponentsInChildren<Renderer>();
    }

    public void SetBugColor(float r, float g, float b)
    {
        Color bug_col = new Color(r, g, b);
        _block.SetColor("_baseColor", bug_col);
        for (int i = 0; i < _renderer.Length; i++)
        {
            _renderer[i].SetPropertyBlock(_block);
        }
    }

}
