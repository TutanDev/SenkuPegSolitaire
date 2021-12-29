using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector2IntEnumerator : IEnumerator<Vector2Int>
{
    private readonly List<Vector2Int> _coords;
    private int _index = -1;
    public Vector2IntEnumerator(List<Vector2Int> coords)
    {
        _coords = coords;
    }

    public Vector2Int Current => _coords[_index];

    object IEnumerator.Current => Current;


    public bool MoveNext()
    {
        _index++;
        return _index < _coords.Count;
    }

    public void Reset()
    {
        _index = -1;
    }

    public void Dispose() { }
}
