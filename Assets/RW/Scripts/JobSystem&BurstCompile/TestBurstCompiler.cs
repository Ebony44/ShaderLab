using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

public class TestBurstCompiler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TestBurstCompile();
    }


    public void TestBurstCompile()
    {
        //using (NativeArray<float> inputArray = new NativeArray<float>(10, Allocator.Persistent))
        //{
        //    using (NativeArray<float> outputArray = new NativeArray<float>(1, Allocator.Persistent))
        //    {
        //        for (int i =0;i < inputArray.Length;++i)
        //        {
        //            inputArray[i] = 1.0f * i;
        //        }

        //    }
        //}

        NativeArray<float> inputArray = new NativeArray<float>(10, Allocator.Persistent);
        NativeArray<float> outputArray = new NativeArray<float>(1, Allocator.Persistent);

        var job = new MyJob
        {
            Input = inputArray,
            Output = outputArray,

        };

        for (int i = 0; i < inputArray.Length; ++i)
        {
            inputArray[i] = 1.0f * i;
        }
        job.Schedule().Complete();
        Debug.Log("The result of the sum is: " + outputArray[0]);
        inputArray.Dispose();
        outputArray.Dispose();

    }

    [BurstCompile(CompileSynchronously = true)]
    private struct MyJob : IJob
    {
        public void Execute()
        {
            float result = 0.0f;
            for (int i = 0; i < Input.Length; ++i)
            {
                result += Input[i];
            }
            Output[0] = result;

        }

        [ReadOnly]
        public NativeArray<float> Input;
        public NativeArray<float> Output;



    }
    [BurstCompile(CompileSynchronously = true)]
    private struct TestSimpleJob : IJob
    {
        public float number;
        public NativeArray<float> data;


        public void Execute()
        {
            data[0] += number;
        }
    }

}
