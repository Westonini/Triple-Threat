using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoveTextWithParent : MonoBehaviour
{
    private Camera mainCam;
    public TextMeshProUGUI text;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        Vector3 namePos = mainCam.WorldToScreenPoint(this.transform.position);
        text.transform.position = namePos;
    }
}
