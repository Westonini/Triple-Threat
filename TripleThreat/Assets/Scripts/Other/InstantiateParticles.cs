using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateParticles : MonoBehaviour
{
    public static void InstantiateParticle(Transform positionToInstantiate, GameObject particle, float timeTillDestroyed, GameObject parentObject = null)
    {
        //Offset the positionToInstantiate on the y-axis
        //Transform offsetPos = transform;
        //offsetPos.position = new Vector3(positionToInstantiate.transform.position.x, positionToInstantiate.transform.position.y + 0.9f, positionToInstantiate.transform.position.z);

        //Create an instance of the particle to be instantiated
        GameObject particleInstance;
        particleInstance = Instantiate(particle, positionToInstantiate.position, positionToInstantiate.rotation) as GameObject;

        //If the parentObject parameter isn't null, set this particle as a child of the parent.
        if (parentObject != null)
        {
            particleInstance.transform.parent = parentObject.transform;
        }

        //Destroy the instantiated bloodSplatter after the value in timeTillDestroyed
        Destroy(particleInstance.gameObject, timeTillDestroyed);
    }
}
