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


    List<RoadChunk> _destination;
    int _lastChunkIndex => _destination.Count - 1;
    Transform parent;


    public void GenerateRoad(List<BuffObjects> buffs)
    {
        _destination = new();
        parent = transform;

        if (chunks != null)
        {
            for (int i = 0; i < Count; i++)
            {
                CreateChunk(Random.Range(0, chunks.Count));
            }
            if (buffs != null && buffs.Count != 0)
            {
                genBuffObjects(buffs);
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
    void genBuffObjects(List<BuffObjects> buffs)
    {
        if (Random.Range(0, 2) == 1)
        {
            var rand = Random.Range(0, buffs.Count);
            var buff = Instantiate(buffs[rand], parent);
            
            var pos = chunks[0].transform.localPosition;
            var randPosX = Random.Range(1, 5);
            pos.y = 0.5f;
            pos.x = randPosX switch
            {
                1 => -4,
                2 => -1.5f,
                3 => 1.5f,
                4 => 4,
                _ => 4
            };

            buff.transform.localPosition = pos;
        }
    }
}
