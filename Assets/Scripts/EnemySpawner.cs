using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject frogPrefab;
    [SerializeField] private GameObject eaglePrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField, Range(0f, 1f)] private float eagleChance = 0.35f;
    [SerializeField] private int maxAlive = 6;
    [SerializeField] private float despawnDistance = 28f;
    [SerializeField] private float eagleHeightOffset = 1.6f;
    [SerializeField] private float spawnXOffset = 1f;

    private float timer;
    private readonly List<GameObject> alive = new List<GameObject>();

    private void Awake()
    {
        timer = spawnInterval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Spawn();
            timer = spawnInterval;
        }

        for (int i = alive.Count - 1; i >= 0; i--)
        {
            if (alive[i] == null)
            {
                alive.RemoveAt(i);
            }
            else if (Vector3.Distance(transform.position, alive[i].transform.position) > despawnDistance)
            {
                Destroy(alive[i]);
                alive.RemoveAt(i);
            }
        }
    }

    private void Spawn()
    {
        if (alive.Count >= maxAlive) return;

        bool eagle = Random.value < eagleChance;
        GameObject prefab = eagle ? eaglePrefab : frogPrefab;
        if (prefab == null) return;

        Vector3 pos = transform.position;
        pos.x += Random.Range(-spawnXOffset, spawnXOffset);
        if (eagle) pos.y += eagleHeightOffset;

        alive.Add(Instantiate(prefab, pos, Quaternion.identity));
    }
}