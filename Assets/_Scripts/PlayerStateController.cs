using System;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    [Serializable]
    public struct PlayerState
    {
        public Vector3 position;
        public Quaternion rotation;
        public int score;
        public String timeTaken;
        public int health;
       

        public PlayerState(Vector3 position, Quaternion rotation, int score, String timeTaken, int health)
        {
            this.position = position;
            this.rotation = rotation;
            this.score = score;
            this.timeTaken = timeTaken;
            this.health = health;
        }
    }

    public PlayerState ToRecord()
    {
        Vector3 pos = transform.position;
        Quaternion rotate = transform.rotation;
        return new PlayerState(pos, rotate, GameStatsController.Score, GameStatsController.TimeTaken, GameStatsController.Health);
    }


}
