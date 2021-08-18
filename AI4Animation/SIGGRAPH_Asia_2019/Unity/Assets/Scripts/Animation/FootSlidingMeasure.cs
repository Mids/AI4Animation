using System;
using System.IO;
using UnityEngine;

public class FootSlidingMeasure : MonoBehaviour
{
	public Transform LeftFootToe;
	public Transform RightFootToe;

	public Vector3 lastLFTPos;
	public Vector3 lastRFTPos;

	public readonly float heightThreshold = 0.005723238f + 0.025f;

	public float CumulatedDis = 0f;
	public int CumulatedFrame = 0;

	private StreamWriter sw;

	// Start is called before the first frame update
	private void Start()
	{
		sw = new StreamWriter("Text Result.txt", true);
	}

	private void OnDestroy()
	{
		sw.Close();
	}

	// Update is called once per frame
	private void Update()
	{
		if (Input.anyKey)
		{
			if (Input.GetKeyDown(KeyCode.T))
			{
				CumulatedDis = 0f;
				CumulatedFrame = 0;
			}

			if (Input.GetKeyDown(KeyCode.G))
			{
				sw.WriteLineAsync($"[{ChairManager.curDirection}/{ChairManager.curRotation}] {CumulatedDis}/{CumulatedFrame} Mean Dis : {CumulatedDis / CumulatedFrame}");
				CumulatedDis = 0f;
				CumulatedFrame = 0;
			}
		}

		Measure();
		if (CumulatedFrame != 0)
			print($"Mean Dis : {CumulatedDis} / {CumulatedFrame} : {CumulatedDis / CumulatedFrame}");
	}

	private void Measure()
	{
		Measure(LeftFootToe, ref lastLFTPos);
		Measure(RightFootToe, ref lastRFTPos);
	}

	private void Measure(Transform footToe, ref Vector3 lastPos)
	{
		if (footToe.position.y > heightThreshold)
		{
			lastPos = default;
			return;
		}

		var curPos = footToe.position;


		if (lastPos != default)
		{
			var disVector3 = curPos - lastPos;

			var dis = Mathf.Sqrt(disVector3.x * disVector3.x + disVector3.z * disVector3.z);

			CumulatedDis += dis;
			CumulatedFrame++;

			// Debug
			// Debug.DrawLine(lastPos, curPos, Color.green, 100f);
		}

		lastPos = curPos;
	}
}