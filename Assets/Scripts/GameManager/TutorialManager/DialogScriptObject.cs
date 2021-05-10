using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DialogData", menuName = "ScriptableObjects/KlippyDialog", order = 2)]
public class DialogScriptObject : ScriptableObject
{
    public Sprite klippyImage;
    public string klippyDialog;
    public bool holdPlayer;

    [Header("Arrow")]
    public bool arrowActive;
    public Sprite arrowImage;
    public Vector3 arrowPosition;
    public Quaternion arrowRotation;
}
