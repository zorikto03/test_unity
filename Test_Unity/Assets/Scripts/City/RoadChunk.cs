using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadChunk : MonoBehaviour
{
    [SerializeField] Transform begin;
    [SerializeField] Transform end;

    public Transform Begin => begin;
    public Transform End => end;

}
