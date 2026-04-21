using UnityEngine;

public class LogicaTubos : MonoBehaviour
{
    public float spawnInterval = 1f;
    public GameObject obstaclePrefab;
    public float heightRange;

    float timer = 0f;

    void Start()
    {
        SpawnObstacle(Vector3.zero);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > spawnInterval)
        {
            SpawnObstacle(new Vector3(0, Random.Range(-heightRange, heightRange), 0));
            timer = 0f;
        }
    }

    void SpawnObstacle(Vector3 position)
    {
        GameObject obstacle = Instantiate(obstaclePrefab);
        obstacle.transform.position = position;
        Destroy(obstacle, 10f);
    }
}