using UnityEngine;

public class ChairManager : MonoBehaviour
{
	public GameObject ChairPrefab;

	private GameObject ChairInstance;

	private readonly Vector3 Center = new Vector3(0, 0, -5);

	private readonly float Distance = 2.5f;

	public static int curDirection = 0;
	public static int curRotation = 0;

	// Start is called before the first frame update
	private void Start()
	{
		CreateChair();
	}

	// Update is called once per frame
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Y))
		{
			if (ChairInstance != default)
			{
				Destroy(ChairInstance);
				ChairInstance = default;
			}

			if (++curDirection > 3) curDirection = 0;
			CreateChair();
		}
		else if (Input.GetKeyDown(KeyCode.U))
		{
			if (ChairInstance != default)
			{
				Destroy(ChairInstance);
				ChairInstance = default;
			}

			if (++curRotation > 3) curRotation = 0;
			CreateChair();
		}
	}

	private void CreateChair()
	{
		ChairInstance = Instantiate(ChairPrefab, GetPosition(), GetRotation(), transform);
		ChairInstance.name = "Chair";
	}

	private Vector3 GetPosition()
	{
		return Center + GetDirection() * Distance;
	}

	private Vector3 GetDirection()
	{
		switch (curDirection)
		{
			case 0:
				return Vector3.forward;
			case 1:
				return Vector3.left;
			case 2:
				return Vector3.back;
			case 3:
				return Vector3.right;
		}

		return Vector3.zero;
	}

	private Quaternion GetRotation()
	{
		switch (curRotation)
		{
			case 0:
				return Quaternion.identity;
			case 1:
				return Quaternion.Euler(0, 90, 0);
			case 2:
				return Quaternion.Euler(0, 180, 0);
			case 3:
				return Quaternion.Euler(0, 270, 0);
		}

		return Quaternion.identity;
	}
}