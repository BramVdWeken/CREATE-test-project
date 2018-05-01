using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTargetAxis : MonoBehaviour
{

    public bool UseDistanceTargetOverwrite = true; //makes it use look at target from a certain distance from target
    public float DistanceForTargetOverwrite = 2;

    public bool LookAtAxis = true;
    public bool LookAtTarget = false;

    private Transform Target;
    private Transform OverwriteTarget;
    private Vector3 Axis = new Vector3(0, 0, 1);

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(0, 0, 0);

        if (LookAtAxis)
        {
            //rotate image to the set axis
            transform.rotation = Quaternion.AngleAxis(0, Axis);
        }
        else if (LookAtTarget)
        {
            //rotate image to the set target
            transform.LookAt(Target);
            transform.Rotate(0, 90, 0, Space.Self);
        }

        if (UseDistanceTargetOverwrite)
        {
            //will make the image close to the camera be rotated towards the camera
            if ((transform.position - OverwriteTarget.position).magnitude < DistanceForTargetOverwrite)
            {
                transform.LookAt(OverwriteTarget);
            }
        }

    }

    public void SetTarget(Transform target)
    {
        Target = target;
    }

    public void SetOverwriteTarget(Transform target)
    {
        OverwriteTarget = target;
    }

    public void SetLookAtAxis(Vector3 axis)
    {
        Axis = axis;
    }
}
