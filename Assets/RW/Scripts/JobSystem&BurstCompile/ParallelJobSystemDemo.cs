using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class ParallelJobSystemDemo : MonoBehaviour
{
    private float myNumber = 0;
    private NativeArray<float> myData;

    private JobHandle parallelJobHandle;

    private void OnEnable()
    {
        myData = new NativeArray<float>(100, Allocator.TempJob);

        for (int i =0; i< myData.Length; ++i)
        {
            myData[i] = i;

        }

    }


    // Start is called before the first frame update
    void Start()
    {

        ParallelJob parallelJob = new ParallelJob
        {
            number = myNumber,
            data = myData,
        };

        parallelJobHandle = parallelJob.Schedule(myData.Length, 32);

        JobHandle.ScheduleBatchedJobs();

        parallelJobHandle.Complete();

        if(parallelJobHandle.IsCompleted)
        {
            for(int i =0;i < myData.Length;++i)
            {
                Debug.Log(parallelJob.data[i]);
            }

        }

        myData.Dispose();
    }


}

[BurstCompile(CompileSynchronously = true)]
public struct ParallelJob : IJobParallelFor
{
    public float number;

    public NativeArray<float> data;


    public void Execute(int index)
    {
        // Debug.Log("index is " + index);
        data[index] += number + 11;
    }
}
