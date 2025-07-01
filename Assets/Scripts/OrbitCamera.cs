using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public Vector3Int GridSize = new Vector3Int(10, 10, 10);
    public float Speed = 1f;
    public float PaddingMultiplier = 2.5f;

    private Vector3 _target;
    private float _distance;

    void Start()
    {
        _target = new Vector3(
            GridSize.x / 2f,
            GridSize.y / 2f,
            GridSize.z / 2f
        );

        
    }

    void LateUpdate()
    {
        float maxDimension = Mathf.Max(GridSize.x, GridSize.y, GridSize.z);
        float distance = maxDimension * PaddingMultiplier;

        float angle = Time.time * Speed;
        float x = Mathf.Cos(angle) * distance;
        float z = Mathf.Sin(angle) * distance;

        Vector3 cameraPos = new Vector3(x, _target.y, z) + _target;
        transform.position = cameraPos;
        transform.LookAt(_target);
    }
}
