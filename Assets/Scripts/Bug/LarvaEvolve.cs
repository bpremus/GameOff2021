using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarvaEvolve : CoreBug
{
    // Save and Load 
    // ----------------------------
    [System.Serializable]
    public class SaveBugVariant
    {
        public float evolution_time;
        public float bug_spawn_perc;
        public float _spawn_t;
        public CoreBug.BugEvolution evolve_to;
    }

    public SaveBugVariant GetEvolvedSaveData()
    {
        SaveBugVariant data = new SaveBugVariant();
        data.evolution_time = this.evolution_time;
        data.bug_spawn_perc = this.bug_spawn_perc;
        data._spawn_t = this._spawn_t;
        data.evolve_to = this.evolve_to;
        return data;
    }

    public void SetEvolvedSaveData(SaveBugVariant data)
    {
        this.evolution_time = data.evolution_time;
        this.bug_spawn_perc = data.bug_spawn_perc;
        this._spawn_t = data._spawn_t;
        this.evolve_to = data.evolve_to;
    }

    [SerializeField] float evolution_time = 2f;
    [SerializeField] float bug_spawn_perc = 0;
    [SerializeField] ProgressBar bug_spawn_bar;

    public CoreBug.BugEvolution evolve_to;

    float _spawn_t = 0;

    protected void BugSpawner()
    {
        _spawn_t += Time.deltaTime;
        if (_spawn_t > evolution_time)
        {
            ArtPrefabsInstance.Instance.EvolveBug(this, evolve_to);
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
