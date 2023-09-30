using System;
using UnityEngine;

[Serializable]
public class CameraFovs
{
    [SerializeField] private float startingStateFov;
    [SerializeField] private float defaultFov;

    public float StartingStateFov => startingStateFov;
    public float DefaultFov => defaultFov;
}