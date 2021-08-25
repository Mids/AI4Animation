using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class FootSlidingMeasure
{
    public static readonly float heightThreshold = 0.005723238f + 0.025f;
    public static bool IsReading = true;

    public static IEnumerator Measure(Transform lToe, Transform rToe, string title)
    {
        var cumLDis = 0f;
        var cumRDis = 0f;
        var cumLFrame = 0;
        var cumRFrame = 0;
        var time = 0f;
        var lastLToePos = Vector3.zero;
        var lastRToePos = Vector3.zero;

        while (IsReading)
        {
            var lPos = lToe.position;

            if (lPos.y > heightThreshold)
            {
                lastLToePos = default;
            }
            else
            {
                if (lastLToePos != default)
                {
                    var disVector3 = lPos - lastLToePos;

                    var dis = Mathf.Sqrt(disVector3.x * disVector3.x + disVector3.z * disVector3.z);

                    cumLDis += dis;
                    cumLFrame++;

                    Debug.DrawLine(lastLToePos, lPos, Color.green, 100f);
                }

                lastLToePos = lPos;
            }

            var rPos = rToe.position;

            if (rPos.y > heightThreshold)
            {
                lastRToePos = default;
            }
            else
            {
                if (lastRToePos != default)
                {
                    var disVector3 = rPos - lastRToePos;

                    var dis = Mathf.Sqrt(disVector3.x * disVector3.x + disVector3.z * disVector3.z);

                    cumRDis += dis;
                    cumRFrame++;

                    Debug.DrawLine(lastRToePos, rPos, Color.green, 100f);
                }

                lastRToePos = rPos;
            }


            time += Time.deltaTime;
            yield return null;
        }

        var s = $"{time}\t{cumLDis / cumLFrame}\t{cumRDis / cumRFrame}";
        var sw = new StreamWriter(title + ".txt", true);
        sw.WriteLine(s);
        Debug.Log(s);

        sw.Close();

        EditorApplication.ExitPlaymode();

        EditorApplication.playModeStateChanged += Restart;
    }

    public static void Restart(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            EditorApplication.playModeStateChanged -= Restart;
            EditorApplication.EnterPlaymode();
        }
    }
}