using System.Collections;
using System.Linq;
using UnityEngine;

public class FakeInput : MonoBehaviour
{
	// private static readonly KeyCode[] FakeKeys = {KeyCode.W, KeyCode.Q, KeyCode.E, KeyCode.LeftShift};
	private static readonly KeyCode[] FakeKeys = {KeyCode.LeftShift};

	private static readonly Vector3[] CheckPoints =
		{new Vector3(-10, 0, 0), new Vector3(10, 0, 0)}; //, new Vector3(0, 0, 1), new Vector3(0, 0, 10)};

	private int CurrentCheckPoint = 0;

	[SerializeField]
	private Transform CharacterTransform;

	public static FakeInput Instance;

	public static float AngleThreshold = 10f;

	public static float DistanceThreshold = 1f;

	private bool isChanging = false;

	private void Start()
	{
		Instance = this;
		CharacterTransform = GameObject.Find("Anubis").transform;
		transform.position = CheckPoints[CurrentCheckPoint];
	}

	private void Update()
	{
		if (Vector3.Distance(CharacterTransform.position, transform.position) < DistanceThreshold && !isChanging)
			StartCoroutine(DelayedChange());
	}

	private IEnumerator DelayedChange()
	{
		isChanging = true;
		yield return new WaitForSeconds(5f);
		isChanging = false;

		ChangeToNextCheckPoint();
	}

	private void ChangeToNextCheckPoint()
	{
		if (++CurrentCheckPoint == CheckPoints.Length) CurrentCheckPoint = 0;

		transform.position = CheckPoints[CurrentCheckPoint];
	}

	public static bool IsFakeKey(KeyCode key)
	{
		return FakeKeys.Contains(key);
	}

	public static bool ControlFakeKey(KeyCode key)
	{
		switch (key)
		{
			case KeyCode.LeftShift:
				return true;
			case KeyCode.W:
				return true;

			case KeyCode.Q:
				if (GetAngle() < -AngleThreshold)
					return true;
				break;
			case KeyCode.E:
				if (GetAngle() > AngleThreshold)
					return true;
				break;
		}

		return false;
	}

	private static float GetAngle()
	{
		var characterPosition = Instance.CharacterTransform.position;
		var characterForward = Instance.CharacterTransform.forward;
		var targetPosition = Instance.transform.position;
		var dirVector = (targetPosition - characterPosition).normalized;

		var angle = Vector3.SignedAngle(characterForward, dirVector, Vector3.up);

		Debug.DrawLine(characterPosition, 10 * (characterPosition + characterForward), Color.red);
		Debug.DrawLine(characterPosition, 10 * (characterPosition + dirVector), Color.blue);

		return angle;
	}
}