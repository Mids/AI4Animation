using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;

public class FakeInput : MonoBehaviour
{
    private static readonly KeyCode[] FakeKeys = {KeyCode.W, KeyCode.Q, KeyCode.E, KeyCode.LeftShift};
    // private static readonly KeyCode[] FakeKeys = {};

    private static readonly Vector3[] CheckPoints =
        {new Vector3(-10, 0, 0), new Vector3(10, 0, 0)}; //, new Vector3(0, 0, 1), new Vector3(0, 0, 10)};

    private int CurrentCheckPoint = 0;

    [SerializeField]
    private Transform characterTransform;

    public static FakeInput Instance;

    public static float AngleThreshold = 10f;

    public static float DistanceThreshold = 1f;

    private static bool IsChanging = false;

    public float targetDelayTime = 105f;

    private Material Mat;

    private ChairManager chairManager;

    // public static float randomTime = 0f;

    public Transform leftFoot;
    public Transform rightFoot;

    public bool isCheckingStopToRun = false;
    public bool isCheckingRunToStop = false;

    public Vector3 lastCharPos;

    public int StopToRunCount = 0;

    private void Start()
    {
        Instance = this;
        characterTransform = GameObject.Find("Anubis").transform;
        Mat = GetComponent<MeshRenderer>().material;
        chairManager = GameObject.Find("ChairManager").GetComponent<ChairManager>();
        // transform.position = GetRandomCheckPoint();

        // randomTime = Random.Range(0f, 1f);
    }

    private void Update()
    {
        if (Vector3.Distance(characterTransform.position, transform.position) < DistanceThreshold && !IsChanging)
            StartCoroutine(DelayedChange());

        // randomTime -= Time.deltaTime;
        var curPos = Instance.characterTransform.position;
        string title;
        StreamWriter sw;

        lastCharPos = curPos;
    }

    private Vector3 GetRandomCheckPoint()
    {
        var position = Random.insideUnitCircle * 20;
        return new Vector3(position.x, 0, position.y);
        return new Vector3(0, 0, -5);
    }

    private IEnumerator DelayedChange()
    {
        IsChanging = true;
        var t = 0f;
        var sw = new StreamWriter("1mTest" + ".txt", true);
        
        while (t < targetDelayTime)
        {
            t += Time.deltaTime;
            var gb = 1 - t / targetDelayTime;

            Mat.color = new Color(1, gb, gb);
            
            var s = $"{Vector3.Distance(characterTransform.position, transform.position)}\t";
            sw.Write(s);
            Debug.Log(s);


            yield return null;
        }
        
        sw.Write('\n');
        
        sw.Close();

        IsChanging = false;

        ChangeToNextCheckPoint();
        Mat.color = Color.white;
    }

    private void ChangeToNextCheckPoint()
    {
        if (++CurrentCheckPoint == CheckPoints.Length) CurrentCheckPoint = 0;

        transform.position = CheckPoints[CurrentCheckPoint];

        // transform.position = GetRandomCheckPoint();
    }

    public static bool IsFakeKey(KeyCode key)
    {
        return FakeKeys.Contains(key);
    }

    public static bool ControlFakeKey(KeyCode key)
    {
        if (Vector3.Distance(Instance.characterTransform.position, Instance.transform.position) <
            DistanceThreshold) return false;

        switch (key)
        {
            case KeyCode.LeftShift:
            case KeyCode.W:
            case KeyCode.S:
            case KeyCode.C:
                // if (randomTime > 0)
                    // return false;
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
        var characterPosition = Instance.characterTransform.position;
        var characterForward = Instance.characterTransform.forward;
        var targetPosition = Instance.transform.position;
        var dirVector = (targetPosition - characterPosition).normalized;

        var angle = Vector3.SignedAngle(characterForward, dirVector, Vector3.up);

        Debug.DrawLine(characterPosition, 10 * (characterPosition + characterForward), Color.red);
        Debug.DrawLine(characterPosition, 10 * (characterPosition + dirVector), Color.blue);

        return angle;
    }
}