using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateParticles : MonoBehaviour
{
    //This object is used to hold instantiated objects so that the hiearchy doesn't get cluttered.
    public static GameObject instantiatedObjects;

    private void Start()
    {
        instantiatedObjects = GameObject.Find("Instantiated Objects");
    }

    public static void InstantiateParticle(Transform positionToInstantiate, GameObject particle, float timeTillDestroyed, float yOffset, GameObject parentObject = null)
    {
        //Create an instance of the particle to be instantiated (possibly with the y-axis offset)
        GameObject particleInstance;
        particleInstance = Instantiate(particle, new Vector3(positionToInstantiate.transform.position.x, positionToInstantiate.transform.position.y + yOffset, positionToInstantiate.transform.position.z), positionToInstantiate.rotation) as GameObject;

        //If the parentObject parameter isn't null, set this particle as a child of the parent.
        if (parentObject != null)
        {
            particleInstance.transform.SetParent(parentObject.transform);
        }
        //Otherwise set it as a child of the Instantiated Objects gameobject.
        else
        {
            particleInstance.transform.SetParent(instantiatedObjects.transform);
        }

        //Destroy the instantiated bloodSplatter after the value in timeTillDestroyed
        Destroy(particleInstance.gameObject, timeTillDestroyed);
    }
}
