using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
	public Obstacle ObstaclePrefab;

	public List<Obstacle> Obstacles;

	public float BoundaryRadius;

	public int BoxCount;

	public float MaxVelocity;

	// Start is called before the first frame update
	private void Start()
	{
		// for (int i = 0; i < BoxCount; i++) MakeRandomBox();
		MakeBoxTowardCharacter();
	}

	private void MakeBoxTowardCharacter()
	{
		var box = Instantiate(ObstaclePrefab, Vector3.zero, Quaternion.identity, transform);
		var velocity = Vector2.down * MaxVelocity;
		var scale = new Vector3(2, 2, 0.5f);
		box.transform.localScale = scale;
		box.transform.localPosition += new Vector3(0, scale.y / 2, 0);
		box.Velocity = new Vector3(velocity.x, 0, velocity.y);
		Obstacles.Add(box);
	}

	private void MakeRandomBox()
	{
		var position = Random.insideUnitCircle * BoundaryRadius;
		var box = Instantiate(ObstaclePrefab, new Vector3(position.x, 0, position.y), Quaternion.identity, transform);
		var velocity = Random.insideUnitCircle * MaxVelocity;
		var scale = new Vector3(Random.value, Random.value, Random.value) * 1 + Vector3.one;
		box.transform.localScale = scale;
		box.transform.localPosition += new Vector3(0, scale.y / 2, 0);
		box.Velocity = new Vector3(velocity.x, 0, velocity.y);
		Obstacles.Add(box);
	}

	// Update is called once per frame
	private void Update()
	{
		foreach (var obstacle in Obstacles.Where(obstacle => obstacle.transform.position.magnitude > BoundaryRadius))
			obstacle.Velocity *= -1;
	}
}