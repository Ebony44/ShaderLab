using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardLab : MonoBehaviour
{
    public void TestDiscardFeature()
    {
        var cosmicRay = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(cosmicRay, out _))
        {
            Debug.Log("");
        }
    }

    (int numHits, RaycastHit[] raycastHits) TestDiscardGetEnemiesInSight()
    {
        var _tmpRayCastHits = new RaycastHit[3];
        int numHits = Physics.RaycastNonAlloc(transform.position, transform.forward, _tmpRayCastHits);
        _ = Physics.RaycastNonAlloc(transform.position, transform.forward, _tmpRayCastHits);
        return (numHits, _tmpRayCastHits);
    }

    private void Start()
    {
        (var enemiesInSight, _) = TestDiscardGetEnemiesInSight();
    }

    string hitpointsString = string.Empty;
    Dictionary<int, GameObject> inventory = new Dictionary<int, GameObject>();
    // Inventory inventory = new Inventory();

    public void TestOutParam()
    {
        int hitpoints;

        int slotId = 1;
        

        if (int.TryParse(hitpointsString, out hitpoints))
        {
            Debug.Log($"You're {hitpoints} hitpoints away from table flipping");
        }

        GameObject item;
        if (inventory.TryGetValue(slotId, out item))
        {
            Debug.Log($"Looted {item.name}");
        }



    }
    public void TestOutParamSecond()
    {
        if (int.TryParse(hitpointsString, out int hitpoints))
        {
            Debug.Log($"You're {hitpoints} hitpoints away from table flipping");
        }

        int slotId = 1;
        if (inventory.TryGetValue(slotId, out GameObject item))
        {
            Debug.Log($"Looted {item.name}");
        }

    }

    //public class Inventory
    //{
    //    public int slotId;
        
    //}

}
