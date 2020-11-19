using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderDebugger : MonoBehaviour
{

    private Material material;
    private ComputeBuffer buffer;
    private Vector4[] element;
    private string label;
    private Renderer renderer;

    [SerializeField] private int bufferLength = 0;

    [SerializeField] private bool bBufferDisplayed = false;


    void Load()
    {
        buffer = new ComputeBuffer(1, 16, ComputeBufferType.Default);
        element = new Vector4[1];
        label = string.Empty;
        renderer = GetComponent<Renderer>();
        material = renderer.material;
        bufferLength = material.GetInt("_Length");

    }

    // Start is called before the first frame update
    void Start()
    {
        Load();
    }

    // Update is called once per frame
    void Update()
    {
        if(!bBufferDisplayed)
        {

            

            Graphics.ClearRandomWriteTargets();
            material.SetPass(0);
            material.SetBuffer("buffer", buffer);


            Graphics.SetRandomWriteTarget(1, buffer, false);
            
            buffer.GetData(element);
            label = (element != null & renderer.isVisible) ? element[0].ToString("F3") : string.Empty;

            if (!label.Equals(string.Empty))
            {
                Debug.Log(label);
                bBufferDisplayed = true;
            }

            //Graphics.ClearRandomWriteTargets();
            //var buffer2 = new ComputeBuffer(1, 16, ComputeBufferType.Default);
            //material.SetPass(0);
            //material.SetBuffer("buffer2", buffer2);


            //Graphics.SetRandomWriteTarget(1, buffer2, false);

            //buffer2.GetData(element);
            //label = (element != null & renderer.isVisible) ? element[0].ToString("F3") : string.Empty;

            //if (!label.Equals(string.Empty))
            //{
            //    Debug.Log(label);
            //    bBufferDisplayed = true;
            //}


            //for(int i = 0; i < bufferLength; ++i)
            //{
            //    // Graphics.SetRandomWriteTarget(1, buffer, false);
            //    Graphics.SetRandomWriteTarget( i + 1, buffer, false);
            //    buffer.GetData(element);
            //    label = (element != null & renderer.isVisible) ? element[0].ToString("F3") : string.Empty;

            //    if (!label.Equals(string.Empty))
            //    {
            //        Debug.Log(label);
            //        bBufferDisplayed = true;
            //    }
            //}



        }

    }
}
