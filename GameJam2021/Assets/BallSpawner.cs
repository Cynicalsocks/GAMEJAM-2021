using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    const float SPAWN_INTERVAL = 30f;

    [SerializeField] float leftSide = 10;
    [SerializeField] float rightSide = 10;
    [SerializeField] float topSide = 10;
    [SerializeField] float bottomSide = 10;

    [SerializeField] GameObject ball_prefab;

    float timeSinceLastSpawn = SPAWN_INTERVAL;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(timeSinceLastSpawn > SPAWN_INTERVAL)
        {
            SpawnBall();

            timeSinceLastSpawn = 0f;
        }
        else
        {
            timeSinceLastSpawn += Time.deltaTime;
        }
    }

    void SpawnBall()
    {
        Vector2 spawnPosition = new Vector2(Random.Range(leftSide, rightSide), Random.Range(bottomSide, topSide));
        spawnPosition = Vector2.MoveTowards(spawnPosition, Vector2.zero, 20);

        GameObject obj = Instantiate(ball_prefab, spawnPosition, Quaternion.identity, null);
        Ball newBall = obj.GetComponent<Ball>();
        newBall.size = Ball_Size.gigantic;
        newBall.color = (Ball_Color)Random.Range(0,6);
        newBall.OnSpawn(Vector2.zero);
    }

    public bool IsPointInRT(Vector2 point, float leftSide, float rightSide, float topSide, float bottomSide)
    {
        // Check to see if the point is in the calculated bounds
        if (point.x >= leftSide &&
            point.x <= rightSide &&
            point.y >= bottomSide &&
            point.y <= topSide)
        {
            return true;
        }
        return false;
    }
}
