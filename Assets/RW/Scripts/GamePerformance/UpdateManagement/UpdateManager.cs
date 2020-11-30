using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{

    public enum eUpdateMode { BucketA, BucketB, Always }
    public static UpdateManager Instance { get; private set; }
    private readonly HashSet<IBatchUpdate> slicedUpdateBehavioursBucketA = new HashSet<IBatchUpdate>();
    private readonly HashSet<IBatchUpdate> slicedUpdateBehavioursBucketB = new HashSet<IBatchUpdate>();
    
    private bool bIsCurrentBucketA;

    public void RegisterSlicedUpdate(IBatchUpdate slicedUpdateBehaviour, eUpdateMode updateMode)
    {

        if(updateMode == eUpdateMode.Always)
        {
            slicedUpdateBehavioursBucketA.Add(slicedUpdateBehaviour);
            slicedUpdateBehavioursBucketB.Add(slicedUpdateBehaviour);
        }
        else
        {
            var targetUpdateFunctions = updateMode == eUpdateMode.BucketA ? slicedUpdateBehavioursBucketA : slicedUpdateBehavioursBucketB;
            targetUpdateFunctions.Add(slicedUpdateBehaviour);
        }

    }

    public void DeregisterSlicedUpdate(IBatchUpdate slicedUpdateBehavior)
    {
        slicedUpdateBehavioursBucketA.Remove(slicedUpdateBehavior);
        slicedUpdateBehavioursBucketB.Remove(slicedUpdateBehavior);
    }


    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var targetUpdateFunctions = bIsCurrentBucketA ? slicedUpdateBehavioursBucketA : slicedUpdateBehavioursBucketB;
        foreach(var slicedUpdateBehaviour in targetUpdateFunctions )
        {
            slicedUpdateBehaviour.BatchUpdate();
        }
        bIsCurrentBucketA = !bIsCurrentBucketA;
    }
}



// test purpose
//
public class Logic_GrabACoffeeSlow_Sliced_6 : MonoBehaviour, IBatchUpdate
{
    public void BatchUpdate()
    {
        SlowWork();
    }

    private void Start()
    {
        UpdateManager.Instance.RegisterSlicedUpdate(this, UpdateManager.eUpdateMode.BucketB);
    }
    private void OnDestroy()
    {
        UpdateManager.Instance.DeregisterSlicedUpdate(this);
    }
    private void SlowWork()
    {
        const int time = 6000;
        Thread.Sleep(time);
    }



}
