using UnityEngine;
using System.Collections.Generic;

public class RandomSpawner : MonoBehaviour
{
    public BoxCollider2D spawnArea;            // 限制生成区域的 BoxCollider2D
    public BoxCollider2D spawnArea2;            // 限制生成区域的 BoxCollider2D
    public float minDistance = 2f;             // 新生成物体与其他物体的最小距离

    void Start()
    {
    }

    public void SpawnPrefab(string name, int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnPrefab(name);
        }
    }

    void SpawnPrefab(string name)
    {
        var pa = spawnArea2;
        if (name == "industryMan")
        {
            pa = spawnArea;
        }
        // 获取所有当前子物体的位置
        List<Vector2> occupiedPositions = new List<Vector2>();
        foreach (Transform child in transform)
        {
            occupiedPositions.Add(child.position);
        }

        // 寻找一个尽可能远的位置
        Vector2 spawnPosition = FindFarAwayPosition(occupiedPositions,pa);

        // 实例化 Prefab
        var prefab = Resources.Load<GameObject>("characters/"+name);
        Instantiate(prefab, spawnPosition, Quaternion.identity, transform);
    }

    Vector2 FindFarAwayPosition(List<Vector2> occupiedPositions ,BoxCollider2D pa)
    {
        Vector2 bestPosition = Vector2.zero;
        float maxDistance = 0f;

        // 设置边界
        Vector2 min = pa.bounds.min;
        Vector2 max = pa.bounds.max;

        // 随机尝试多个位置
        for (int i = 0; i < 10; i++)
        {
            // 随机生成一个位置
            Vector2 randomPosition = new Vector2(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y)
            );

            // 检查这个位置与现有物体的距离
            float distance = GetMinimumDistance(randomPosition, occupiedPositions);

            // 记录距离最大的点
            if (distance > maxDistance)
            {
                maxDistance = distance;
                bestPosition = randomPosition;
            }
        }

        return bestPosition;
    }

    // 计算新位置与现有物体的最小距离
    float GetMinimumDistance(Vector2 position, List<Vector2> occupiedPositions)
    {
        float minDist = float.MaxValue;

        foreach (var occupiedPosition in occupiedPositions)
        {
            float dist = Vector2.Distance(position, occupiedPosition);
            if (dist < minDist)
            {
                minDist = dist;
            }
        }

        return minDist;
    }
}
