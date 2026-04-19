using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public enum SpawnDirection { Vertical, Horizontal }
    public GameObject coinPrefabs;
    public int count = 1;
    public float spacing;
    public SpawnDirection dir;
    private void Start()
    {
        SpawnCoin();
    }
    public void SpawnCoin()
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos=transform.position;
            if (dir == SpawnDirection.Horizontal)
            {
                spawnPos += new Vector3(spacing*i, 0f,0f);
            }
            else
            {
                spawnPos += new Vector3(0f, spacing * i, 0f);
            }
            GameObject coin= Instantiate(coinPrefabs, spawnPos, Quaternion.identity);
            coin.transform.SetParent(transform);
        }
    }
}
