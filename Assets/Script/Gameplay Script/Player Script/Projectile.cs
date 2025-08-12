using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float endOffsetDistance;   // khoảng cách giữa vật thể và mục tiêu đc xem là đánh trúng
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 offsetPos = new(); // offset so với chân (vị trí đứng) của mục tiêu

    [SerializeField] private Transform target;
    private Transform startPoint;
    private Vector3 currentPos;

    private ITarget targetInterface;

    private void Awake()
    {
        targetInterface = transform.parent.GetComponentInParent<ITarget>();
        startPoint = transform.parent;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        currentPos = transform.position;
        target = targetInterface.Target;
    }

    private void Update()
    {
        // Di chuyển và luôn hướng mặt về mục tiêu
        Vector3 direction = target.position + offsetPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.SetPositionAndRotation(currentPos = Vector3.MoveTowards(currentPos, target.position + offsetPos, moveSpeed * Time.deltaTime), Quaternion.Euler(0, 0, angle));

        // Chạm mục tiêu
        if (Vector3.Distance(transform.position, target.position) <= (endOffsetDistance + 0.03f))
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        gameObject.SetActive(false);
        transform.position = startPoint.position;

        targetInterface.HitTarget(target);
    }
}
