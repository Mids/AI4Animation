using System.Collections;
using System.IO;
using UnityEngine;

public static class FootSlidingMeasure
{
    public static readonly float heightThreshold = 0.005723238f + 0.025f;

    public static IEnumerator Measure(Transform toe, string title)
    {
        var sw = new StreamWriter(title + ".txt");

        var cumDis = 0f;
        var cumFrame = 0;
        var time = 0f;
        var lastPos = Vector3.zero;

        while (time <= 8f)
        {
            var curPos = toe.position;

            if (curPos.y > heightThreshold)
            {
                lastPos = default;
            }
            else
            {
                if (lastPos != default)
                {
                    var disVector3 = curPos - lastPos;

                    var dis = Mathf.Sqrt(disVector3.x * disVector3.x + disVector3.z * disVector3.z);

                    cumDis += dis;
                    cumFrame++;

                    Debug.DrawLine(lastPos, curPos, Color.green, 100f);
                }

                lastPos = curPos;
            }


            time += Time.deltaTime;
            yield return null;
        }

        var s = $"[{title}] {cumDis}/{cumFrame} Mean Dis : {cumDis / cumFrame}";
        sw.WriteLineAsync(s);
        Debug.Log(s);

        sw.Close();
    }
}