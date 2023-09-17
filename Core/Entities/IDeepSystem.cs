using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

namespace DeepAction
{

    //TODO DELETE THIS.
    //leaving it rn incase I have more ideas idk.

    public interface IDeepSystem<T> where T : DeepBehavior
    {
        IJobParallelFor CreateJobForAll(HashSet<T> behaviors);

        void RegisterWithSystem(T behavior)
        {
            DeepSystem<T>.behaviors.Add(behavior);
        }
        void DeregisterWithSystem(T behavior)
        {
            DeepSystem<T>.behaviors.Remove(behavior);
        }
    }

    public static partial class DeepSystem<T> where T : DeepBehavior
    {
        public static List<T> behaviors = new List<T>();

    }

    public abstract class DeepSystemasdf<T> where T : DeepBehavior
    {
        private static HashSet<T> behaviors = new HashSet<T>();

        private static List<JobHandle> jobHandles = new List<JobHandle>();
        private static List<Tuple<T, Action<T>>> jobsWithCallback = new List<Tuple<T, Action<T>>>();
        private static bool hasJobsToSchedule;
        private static JobHandle _batchHandle;
        private static NativeArray<JobHandle> nativeHandles;

        public static void ScheduleJob(JobHandle jobHandle, T job, Action<T> callback)
        {
            jobHandles.Add(jobHandle);
            jobsWithCallback.Add(new Tuple<T, Action<T>>(job, callback));

            SetHasJobs();
        }

        private static void SetHasJobs()
        {
            if (hasJobsToSchedule)
            {
                return;
            }
            DeepUpdate.UpdateComplete += Complete;
            hasJobsToSchedule = true;
        }

        private static void BatchScehdule()
        {
            nativeHandles = new NativeArray<JobHandle>(jobHandles.Count, Allocator.TempJob);
            for (int i = 0; i < jobHandles.Count; i++)
            {
                nativeHandles[i] = jobHandles[i];
            }
            var dependency = JobHandle.CombineDependencies(nativeHandles);
        }

        private static void Complete()
        {
            _batchHandle.Complete();
            foreach (Tuple<T, Action<T>> job in jobsWithCallback)
            {
                job.Item2.Invoke(job.Item1);
            }

            jobHandles.Clear();
            jobsWithCallback.Clear();
            nativeHandles.Dispose();

            DeepUpdate.UpdateComplete -= Complete;
            hasJobsToSchedule = false;
        }
    }
}