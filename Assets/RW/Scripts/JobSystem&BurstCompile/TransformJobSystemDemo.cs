using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class TransformJobSystemDemo : MonoBehaviour
{
    private float myNumber = 0.5f;
    private NativeArray<float> myData;

    private JobHandle transformJobHandle;
    private TransformAccessArray transformAccessArray;


    private void OnEnable()
    {

        myData = new NativeArray<float>(1, Allocator.Persistent);

        for (int i =0;i<myData.Length;++i)
        {
            myData[i] = i;
        }
        Transform[] myTansforms = { transform };
        transformAccessArray = new TransformAccessArray(myTansforms);

    }

    private void OnDisable()
    {
        myData.Dispose();
        transformAccessArray.Dispose();
    }

    private void Update()
    {
        TransformJob transformJob = new TransformJob
        {
            number = myNumber,
            data = myData,
            deltaTime = Time.deltaTime,

        };

        transformJobHandle = transformJob.Schedule(transformAccessArray);

        JobHandle.ScheduleBatchedJobs();
        transformJobHandle.Complete();

        if(transformJobHandle.IsCompleted && transform.position.x >= Vector3.one.x * 3f)
        {
            // Debug.Log("Transform job completed");
        }

    }

}

[BurstCompile(CompileSynchronously = true)]
public struct TransformJob : IJobParallelForTransform
{
    public float number;
    public float deltaTime;

    public NativeArray<float> data;

    public void Execute(int index, TransformAccess transform)
    {
        transform.localPosition += Vector3.one * (data[index] + number) * deltaTime;
    }
}
