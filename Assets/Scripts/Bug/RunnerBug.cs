using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerBug : CoreBug
{
    // Runner bugs are enemy AI bug

    public  override void OnBugReachHomeCell()
    {
        Debug.Log("OnBugReachHomeCell");
    }

    public override void OnTargetReach(HiveCell cell)
    {
        // steal something 
        Debug.Log("OnTargetReach");

        int idx = Random.Range(0, 6);
        GameObject food_wood = ArtPrefabsInstance.Instance.FoodAndWoodPrefabs[idx];
        Vector3 food_pos = new Vector3(0, 0, -5);
        GameObject g = Instantiate(food_wood, food_pos, Quaternion.identity);
        harvest_object = g;
    }

    public override void OnDestinationReach(HiveCell cell)
    {
        Debug.Log("OnDestinationReach");
        EnemyController.Instance.ReportSucessfulScout(target_cell);

        if(harvest_object != null)
        {
            Destroy(harvest_object);
            harvest_object = null;
        }
        OnLateDecay();
    }

    [SerializeField]
    private float walk_animation_adjust = 1;

    public override void SetAnimation()
    {
        // if its dead just stop all
        if (_isDead)
        {
            for (int i = 0; i < animators.Length; i++)
            {
                animators[0].speed = 0;
            }
            return;
        }

        // idle 

        if (bugAnimation == BugAnimation.idle)
        {
            animators[0].speed = 0;
            animators[0].SetInteger("State", 0);
            animators[1].SetInteger("State", 0);
            //animators[2].SetInteger("State", 0);
        }

        // walking

        if (bugAnimation == BugAnimation.walk)
        {
            animators[0].speed = move_speed * 2.5f * walk_animation_adjust;
            animators[0].SetInteger("State", 0);
            animators[1].SetInteger("State", 0);
            //animators[2].SetInteger("State", 0);
        }

        last_pos = transform.position;
    }

}
