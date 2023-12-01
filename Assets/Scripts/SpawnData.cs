using System;

public class SpawnData
{
    public enum SpawnPoint { Left, Right};

    public SpawnPoint spawnPoint;
    public float spawnInterval;

    public SpawnData(SpawnPoint spawnPoint, float spawnInterval)
    {
        this.spawnPoint = spawnPoint;
        this.spawnInterval = spawnInterval;
    }
}
