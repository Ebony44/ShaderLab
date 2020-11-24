using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class SimpleDependencyDemo : MonoBehaviour
{

    private float myNumber = 5;
    private NativeArray<float> myData;
    private JobHandle simpleJobHandle;
    private JobHandle parallelJobHandle;
    private JobHandle transformJobHandle;

    private TransformAccessArray transformAccessArray;

    private void OnEnable()
    {
        myData = new NativeArray<float>(40, Allocator.Persistent);

        for(int i =0;i<myData.Length;++i)
        {
            myData[i] = i;
        }

        Transform[] myTransforms = { transform };
        transformAccessArray = new TransformAccessArray(myTransforms);

    }


    // Start is called before the first frame update
    void Start()
    {
        SimpleJob simpleJob = new SimpleJob
        {
            number = myNumber,
            data = myData,
        };

        ParallelJob parallelJob = new ParallelJob
        {
            number = myNumber,
            data = myData,
        };

        TransformJob transformJob = new TransformJob
        {
            number = myNumber,
            data = myData,
            deltaTime = Time.deltaTime,
        };

        simpleJobHandle     = simpleJob.Schedule();
        parallelJobHandle = parallelJob.Schedule(myData.Length, 32, simpleJobHandle);
        transformJobHandle = transformJob.Schedule(transformAccessArray, parallelJobHandle);

        // dependency like a->b->c...

        JobHandle.ScheduleBatchedJobs();

        simpleJobHandle.Complete();
        parallelJobHandle.Complete();
        transformJobHandle.Complete();

        if(simpleJobHandle.IsCompleted)
        {
            Debug.Log("simple job result " + simpleJob.data[0]);
        }
        
        if (parallelJobHandle.IsCompleted)
        {
            for(int i =0;i<myData.Length;++i)
            {
                Debug.Log("parallel job result " + parallelJob.data[i]);
            }
        }
        myData.Dispose();
        transformAccessArray.Dispose();


    }

}

