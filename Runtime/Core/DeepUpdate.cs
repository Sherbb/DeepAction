using System;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Profiling;

// Custom update organizer.
// Very helpfull when scheduling Jobs
/// <summary> Early => Schedule => Norm => Complete => Final </summary>
public class DeepUpdate : MonoBehaviour
{
    public static DeepUpdate instance;

    public static Action UpdateEarly;
    /// <summary> Used for scheduling Jobs </summary>
    public static Action UpdateSchedule;
    /// <summary> Primary Update loop. If you don't care when something happens it should be here </summary>
    public static Action UpdateNorm;// called after schedule to give time for jobs to complete
    /// <summary> Used for reckoning completed Jobs</summary>
    public static Action UpdateComplete;
    public static Action UpdateFinal;

    //TODO: properly implement the rest:
    public static Action FixedEarly;
    public static Action FixedSchedule;
    public static Action FixedNorm;
    public static Action FixedComplete;
    public static Action FixedFinal;

    public static Action LateEarly;
    public static Action LateSchedule;
    public static Action LateNorm;
    public static Action LateComplete;
    public static Action LateFinal;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        Profiler.BeginSample("Update_Early");
        UpdateEarly?.Invoke();
        Profiler.EndSample();

        Profiler.BeginSample("Update_Schedule");
        UpdateSchedule?.Invoke();
        Profiler.EndSample();

        JobHandle.ScheduleBatchedJobs();

        Profiler.BeginSample("Update_Norm");
        UpdateNorm?.Invoke();
        Profiler.EndSample();

        Profiler.BeginSample("Update_Complete");
        UpdateComplete?.Invoke();
        Profiler.EndSample();

        Profiler.BeginSample("Update_Final");
        UpdateFinal?.Invoke();
        Profiler.EndSample();
    }

    private void FixedUpdate()
    {
        FixedEarly?.Invoke();
        FixedSchedule?.Invoke();
        FixedNorm?.Invoke();
        FixedComplete?.Invoke();
        FixedFinal?.Invoke();
    }

    private void LateUpdate()
    {
        LateEarly?.Invoke();
        LateSchedule?.Invoke();
        LateNorm?.Invoke();
        LateComplete?.Invoke();
        LateFinal?.Invoke();
    }
}
