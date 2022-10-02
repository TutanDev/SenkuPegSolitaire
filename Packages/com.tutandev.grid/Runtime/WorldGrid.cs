using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldGrid : MonoBehaviour, IEnumerable<Vector3>
{
    [SerializeField] bool IsSquare = true;
    [SerializeField] int horizontalResolution;
    [SerializeField] int verticalResolution;

    [SerializeField] bool DefineUsingGaps = false;
    [SerializeField] bool IsCentered = true;
    [SerializeField] Vector2 dimensions = Vector2Int.one;
    [SerializeField] float horizontalGapLength = 1.0f;
    [SerializeField] float verticalGapLength = 1.0f;

    [SerializeField] bool drawGizmos = true;
    [SerializeField] Color gizmoColor = Color.yellow;
    [SerializeField] float gizmoSize = 0.1f;
    [SerializeField] bool showCoords = false;


    private Grid _grid;
    private List<Vector3> _positions;
    private List<Vector3> _positionsNormalized;
    private Dictionary<Vector3, Vector2Int> _positionToCoord;
    private Transform _transform; 

    private Vector2 scale = Vector2.one;

    private void OnValidate()
    {
        horizontalResolution = Mathf.Clamp(horizontalResolution, 2, int.MaxValue);
        verticalResolution = Mathf.Clamp(verticalResolution, 2, int.MaxValue);

        if (IsSquare)
        {
            verticalResolution = horizontalResolution;
            verticalGapLength = horizontalGapLength;
            dimensions.Set(dimensions.x, dimensions.x);
        }

        // Awake
        if (_grid == null)
        {
            _transform = transform;
            _grid = new Grid(horizontalResolution, verticalResolution);
            _positions = new List<Vector3>();
            _positionsNormalized = new List<Vector3>();
            _positionToCoord = new Dictionary<Vector3, Vector2Int>();
        }
        
        _grid.SetResolution(horizontalResolution, verticalResolution);
        CalculateNormalizedPositions();
    }

    private void Awake()
    {
        _transform = transform;
        _grid = new Grid(horizontalResolution, verticalResolution);
        _positions = new List<Vector3>();
        _positionsNormalized = new List<Vector3>();
        _positionToCoord = new Dictionary<Vector3, Vector2Int>();

        CalculateNormalizedPositions();
        CalculateRelativePositions();
    }

    public Vector2Int GetCoords(Vector3 pos)
    {
        Vector2Int result = -Vector2Int.one;
        _positionToCoord.TryGetValue(pos, out result);
        return result;
    }

    private void CalculateNormalizedPositions()
    {
        Vector3 pos;

        scale = DefineUsingGaps ? new Vector2(horizontalGapLength, horizontalGapLength)
                                : new Vector2(dimensions.x / (horizontalResolution - 1), dimensions.y / (verticalResolution - 1));

        float xOffset = IsCentered ? -(horizontalResolution - 1) * 0.5f : 0;
        float yOffset = IsCentered ? -(verticalResolution - 1) * 0.5f : 0;

        _positionsNormalized.Clear();
        foreach (Vector2Int coord in _grid)
        {
            pos = new Vector3((coord.x + xOffset) * scale.x, 0, (coord.y + yOffset) * scale.y);    
            _positionsNormalized.Add(pos);
        }
    }

    Vector3 MakePositionRelative(Vector3 position, Vector2Int coords)
    {
        Vector3 result = _transform.rotation * (position + _transform.position);
        _positionToCoord.Add(result, coords);
        return result;
    }

    void CalculateRelativePositions()
    {
        _positions.Clear();
        _positionToCoord.Clear();
        int i = 0;
        foreach (Vector2Int coord in _grid)
        {
            _positions.Add(MakePositionRelative(_positionsNormalized[i], coord));
            i++;
        }
    }

    public IEnumerator<Vector3> GetEnumerator()
    {
        return _positions.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos) return;

        CalculateRelativePositions();

        Gizmos.color = gizmoColor;
        foreach (var pos in _positions)
        {
            Gizmos.DrawSphere(pos, gizmoSize);
        }

    }
}
