using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoadChunkType
{
    Road,
    Bridge,
    Alley
}

public class RoadChunk : MonoBehaviour
{
    [SerializeField] Transform begin;
    [SerializeField] Transform end;
    [SerializeField] RoadChunkType type;

    public Transform Begin => begin;
    public Transform End => end;
    public RoadChunkType Type => type;

}
