using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This script will always move text to be where the parent object is
public class MoveTextWithParent : MonoBehaviour
{
    private Camera mainCam;
    public TextMeshProUGUI text;

    void Start()
    {
        //Get the camera tagged "CameraMain"
        mainCam = Camera.main;
    }

    void Update()
    {
        //Set text transform to the transform of the object this script is attached to
        Vector3 namePos = mainCam.WorldToScreenPoint(this.transform.position);
        text.transform.position = namePos;
    }
}
