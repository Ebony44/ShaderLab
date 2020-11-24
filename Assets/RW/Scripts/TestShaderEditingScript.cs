using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class TestShaderEditingScript : MonoBehaviour
{

    public Transform targetPos;
    
    // Start is called before the first frame update
    void Start()
    {
        Vector4 tempVector4 = new Vector4(targetPos.position.x,
            targetPos.position.y,
            targetPos.position.z,
            1.0f);
        


        gameObject.GetComponent<Renderer>().sharedMaterial.SetVector("_TargetVector", tempVector4);

        Vector4 tempRelativeVector4 = new Vector4(targetPos.position.x,
            targetPos.position.y,
            targetPos.position.z,
            1.0f);

        gameObject.GetComponent<Renderer>().sharedMaterial.SetVector("_TargetVectorRelative", tempRelativeVector4);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
