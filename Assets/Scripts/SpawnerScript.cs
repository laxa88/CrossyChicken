using UnityEngine;
using System.Collections;

public class SpawnerScript : MonoBehaviour {

	public GameObject enemyPrefab;
	public float sideTileCount = 5f; // how many tiles to LEFT or RIGHT can player move?
	public float minSpawnInterval = 0.2f;
	public float maxSpawnInterval = 1.0f;
	public float minEnemySpeed = 1.0f;
	public float maxEnemySpeed = 3.0f;
	float enemySpeed = 0f;
	float spawnCooldown = 0f;
	GameObject guideTile;
	int enemyDirection = -1;

	public void InitSpawner (GameObject tileToFollow, int spawnedEnemyDirection)
	{
		// We will use this tile as reference point for the spawner to update position.z
		guideTile = tileToFollow;
		enemyDirection = spawnedEnemyDirection;

		// If enemy is going LEFT (0), then spawner must be on the RIGHT side of the map
		// If enemy is going RIGHT (1), then spawner must be on the LEFT side of the map
		Vector3 tilePosition = guideTile.transform.position;

		if (enemyDirection == 0) {
			tilePosition.x = Global.tileDistance * sideTileCount;
		}
		else {
			tilePosition.x = -Global.tileDistance * sideTileCount;
		}

		// don't forget to update the spawner position
		transform.position = tilePosition;

		// When creating a new spawner, it will set the enemy's initial speed.
		// All enemies spawned will have the same speed for this row, same as crossy road.
		enemySpeed = Random.Range(minEnemySpeed, maxEnemySpeed);
	}

	void Update ()
	{
		Vector3 newPos = transform.position;
		newPos.z = guideTile.transform.position.z; // only update the z-movement
		newPos.y = Global.tileHeight; // remember to put it above the ground
		transform.position = newPos;

		// If the spawner moved too far below, destroy the spawner, because we
		// don't need it anymore. We also add 1f for extra space to delete the
		// spawner before the tile is recycled to the front.
		if (newPos.z <= -Global.zBound + 1f) {

			Destroy(gameObject);

		}
		else {

			// countdown for enemy spawn
			spawnCooldown -= Time.deltaTime;

			// If the spawner isn't destroyed, then try to spawn enemies
			if (spawnCooldown <= -1f) {

				GameObject enemy = GameObject.Instantiate(enemyPrefab);
				enemy.GetComponent<EnemyScript>().InitEnemy(gameObject, enemyDirection, enemySpeed);

				// If enemy is facing LEFT (0), flip the enemy 180 degrees
				// If enemy is facing RIGHT (1), do nothing
				if (enemyDirection == 0)
				{
					enemy.transform.Rotate(new Vector3(0, 180f, 0));
				}

				// set next cooldown
				spawnCooldown = Random.Range(minSpawnInterval, maxSpawnInterval);

			}

		}

	}
}
