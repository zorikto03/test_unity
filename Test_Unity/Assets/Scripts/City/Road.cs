using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoadType
{
    Road,
    Bridge,
    Alley
}

public class Road : MonoBehaviour
{
    [SerializeField] List<RoadChunk> chunks;
    [SerializeField] int Count;
    [SerializeField] RoadType type;

    public Transform Begin => _destination[0].Begin;
    public Transform End => _destination[_lastChunkIndex].End;
    public RoadType Type => type;


    List<RoadChunk> _destination = new();
    int _lastChunkIndex => _destination.Count - 1;
    Transform parent;


    public void GenerateRoad()
    {
        parent = transform;

        if (chunks != null)
        {
            for (int i = 0; i < Count; i++)
            {
                CreateChunk(Random.Range(0, chunks.Count - 1));
            }
        }
    }

    void CreateChunk(int index)
    {
        var chunk = Instantiate(chunks[index], parent);
        if (_destination.Count == 0)
        {
            chunk.transform.localPosition = Vector3.zero;
        }
        else
        {
            chunk.transform.localPosition = _destination[_lastChunkIndex].End.position - chunk.Begin.localPosition;
        }
        _destination.Add(chunk);
    }

}
