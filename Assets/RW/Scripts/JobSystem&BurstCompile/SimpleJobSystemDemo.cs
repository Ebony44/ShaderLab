using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

public class SimpleJobSystemDemo : MonoBehaviour
{
    private float myNumber = 5;
    private NativeArray<float> myData;
    private JobHandle simpleJobHandle;

    private void OnEnable()
    {
        myData = new NativeArray<float>(1, Allocator.Persistent);
        myData[0] = 2;
    }
    private void OnDisable()
    {
        myData.Dispose();
    }

    private void Start()
    {

        SimpleJob simpleJob = new SimpleJob
        {
            number = myNumber,
            data = myData,
        };

        simpleJobHandle = simpleJob.Schedule();
        simpleJobHandle.Complete();

        if(simpleJobHandle.IsCompleted)
        {
            Debug.Log(simpleJob.data[0]);
        }


    }

    [BurstCompile(CompileSynchronously = true)]
    public struct SimpleJob : IJob
    {
        public float number;

        public NativeArray<float> data;

        public void Execute()
        {
            data[0] += number;
        }
    }

}
