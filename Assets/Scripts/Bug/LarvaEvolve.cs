using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarvaEvolve : CoreBug
{
    [SerializeField] float evolution_time = 2f;

    [SerializeField] ProgressBar bug_spawn_bar;
    [SerializeField] float bug_spawn_perc = 0;

    public int prefab_index = 0;

    float _spawn_t = 0;
    protected void BugSpawner()
    {
        _spawn_t += Time.deltaTime;
        if (_spawn_t > evolution_time)
        {
            ArtPrefabsInstance.Instance.EvolveBug(this, prefab_index);
        }
        else
        {
            // progress bar;
            bug_spawn_perc = _spawn_t / evolution_time;
            bug_spawn_bar.SetProgress(bug_spawn_perc);
        }
    }

    public override void SetAnimation()
    {
        BugSpawner();
    }

}
