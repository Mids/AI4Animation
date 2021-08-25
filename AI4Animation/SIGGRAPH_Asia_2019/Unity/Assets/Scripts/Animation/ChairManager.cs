using UnityEngine;

public class ChairManager : MonoBehaviour
{
    public GameObject ChairPrefab;

    private GameObject ChairInstance;

    private readonly Vector3 Center = new Vector3(0, 0, -5);

    [SerializeField]
    [Range(2, 3)]
    private float Distance = 2.0f;

    [SerializeField]
    [Range(0, 3)]
    private int curDirection = 0;

    [SerializeField]
    [Range(0, 3)]
    private int curRotation = 0;

    public static ChairManager Instance;

    public int CurRotation
    {
        get => curRotation;
        set => curRotation = value % 4;
    }


    public int CurDirection
    {
        get => curDirection;
        set => curDirection = value % 4;
    }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        ChairInstance = Instantiate(ChairPrefab, GetPosition(), GetRotation(), transform);
        ChairInstance.name = "Chair";
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            ++CurDirection;
            CreateChair();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            ++CurRotation;
            CreateChair();
        }
    }

    public void CreateChair()
    {
        ChairInstance.transform.SetPositionAndRotation(GetPosition(), GetRotation());
    }

    private Vector3 GetPosition()
    {
        return Center + GetDirection() * Distance;
    }

    private Vector3 GetDirection()
    {
        switch (CurDirection)
        {
            case 0:
                return Vector3.forward;
            case 1:
                return Vector3.right;
            case 2:
                return Vector3.back;
            case 3:
                return Vector3.left;
        }

        return Vector3.zero;
    }

    private Quaternion GetRotation()
    {
        var randOffset = Random.Range(-0.1f, 0.1f);
        switch (CurRotation)
        {
            case 0:
                return Quaternion.Euler(0, randOffset, 0);
            case 1:
                return Quaternion.Euler(0, 90 + randOffset, 0);
            case 2:
                return Quaternion.Euler(0, 180 + randOffset, 0);
            case 3:
                return Quaternion.Euler(0, 270 + randOffset, 0);
        }

        return Quaternion.identity;
    }
}