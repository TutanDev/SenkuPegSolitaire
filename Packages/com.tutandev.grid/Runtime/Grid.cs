using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : IEnumerable<Vector2Int>
{
    #region *** IEnumerable ***
    private List<Vector2Int> _coords;
    public IEnumerator<Vector2Int> GetEnumerator()
    {
        return new Vector2IntEnumerator(_coords);
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    #endregion

    public Grid()
    {
        _coords = new List<Vector2Int>();
    }

    public Grid(int horizontalRes, int verticalRes)
    {
        _coords = new List<Vector2Int>();
        for (int j = 0; j < verticalRes; j++)
        {
            for (int i = 0; i < horizontalRes; i++)
            {
                _coords.Add(new Vector2Int(i, j));
            }
        }
    }

    public void SetResolution(int horizontalRes, int verticalRes)
    {
        _coords.Clear();
        for (int j = 0; j < verticalRes; j++)
        {
            for (int i = 0; i < horizontalRes; i++)
            {
                _coords.Add(new Vector2Int(i, j));
            }
        }
    }
}