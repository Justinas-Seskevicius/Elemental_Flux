using UnityEngine;

public class FieldOfView2 : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    private Mesh _mesh;
    private float _fov;
    private float _viewDistance;
    private Vector3 _origin;
    private float _startingAngle;

    private void Start() {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        _fov = 90f;
        _viewDistance = 50f;
        _origin = Vector3.zero;
    }

    private void LateUpdate() {
        int rayCount = 50;
        float angle = _startingAngle;
        float angleIncrease = _fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = _origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++) {
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(_origin, GetVectorFromAngle(angle), _viewDistance, layerMask);
            if (raycastHit2D.collider == null) {
                // No hit
                vertex = _origin + GetVectorFromAngle(angle) * _viewDistance;
            } else {
                // Hit object
                vertex = raycastHit2D.point;
            }
            vertices[vertexIndex] = vertex;

            if (i > 0) {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }


        _mesh.vertices = vertices;
        _mesh.uv = uv;
        _mesh.triangles = triangles;
        _mesh.bounds = new Bounds(_origin, Vector3.one * 1000f);
    }

    public static Vector3 GetVectorFromAngle(float angle) {
        // angle = 0 -> 360
        float angleRad = angle * (Mathf.PI/180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public static float GetAngleFromVectorFloat(Vector3 dir) {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    public static int GetAngleFromVector(Vector3 dir) {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        int angle = Mathf.RoundToInt(n);

        return angle;
    }

    public static int GetAngleFromVector180(Vector3 dir) {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        int angle = Mathf.RoundToInt(n);

        return angle;
    }

    public void SetOrigin(Vector3 origin)
    {
        this._origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection) {
        _startingAngle = GetAngleFromVectorFloat(aimDirection) + _fov / 2f;
    }

    public void SetFoV(float fov) {
        this._fov = fov;
    }

    public void SetViewDistance(float viewDistance) {
        this._viewDistance = viewDistance;
    }
    
    
}
