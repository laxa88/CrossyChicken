using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	public float moveSpeed;
	public GameObject originSpawner;
	public int direction;

	public void InitEnemy (GameObject spawner, int dir, float speed)
	{
		originSpawner = spawner;
		direction = dir;
		moveSpeed = speed;

		// set the starting position for the enemy, based on where the spawner is
		transform.position = originSpawner.transform.position;
	}

	void Start ()
	{
	}

	void Update ()
	{
		Vector3 newPos = transform.position;
		bool enemyDies = false;

		if (direction == 0) {
			newPos.x += Time.deltaTime * -moveSpeed; // move left
			if (newPos.x < -Global.xBound + 1f) {
				enemyDies = true; // If enemy moves out of map on RIGHT side, destroy
			}
		}
		else {
			newPos.x += Time.deltaTime * moveSpeed; // move right
			if (newPos.x > Global.xBound - 1f) {
				enemyDies = true; // If enemy moves out of map on LEFT side, destroy
			}
		}

		newPos.z = originSpawner.transform.position.z;
		if (newPos.z < Global.zBound + 1) {
			enemyDies = true; // If enemy moves out of BOTTOM of map, destroy the enemy
		}

		if (enemyDies) {
			Destroy(gameObject);
		}
		else {
			// If the enemy isn't destroyed, update movement normally
			transform.position = newPos;
		}
	}
}
