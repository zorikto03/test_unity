using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public int Coins;
    public float RecordDistance;
    public float MaxDangerousManeuvers;
    public float MaxScore;

    public bool SetData(float distance, float dangerousManeuvers, float maxScore)
    {
        bool result = false;

        if (RecordDistance < distance)
        {
            RecordDistance = distance;
            result = true;
        }

        if (MaxDangerousManeuvers < dangerousManeuvers)
        {
            MaxDangerousManeuvers = dangerousManeuvers;
            result = true;
        }

        if (MaxScore < maxScore)
        {
            MaxScore = maxScore;
            result = true;
        }

        if (result)
        {
            LocalSave();
        }

        return result;
    }

    void LocalSave()
    {
        var json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("player", json);
    }

    public PlayerInfo LocalLoad()
    {
        if (PlayerPrefs.HasKey("player"))
        {
            string json = PlayerPrefs.GetString("player");
            return JsonUtility.FromJson<PlayerInfo>(json);
        }
        else
        {
            return new PlayerInfo();
        }
    }
    public override int GetHashCode()
    {
        return RecordDistance.GetHashCode() ^ MaxDangerousManeuvers.GetHashCode() ^ MaxScore.GetHashCode();
    }
}

