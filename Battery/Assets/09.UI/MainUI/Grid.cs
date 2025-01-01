using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class Grid : VisualElement
{
    [UxmlAttribute] public int gridSize = 5;
    
    private Node[,] _grid;
    public List<Battery> batteries;
    
    public Grid()
    {
        AddToClassList("grid");
        CreateGrid();
        batteries = new();
    }

    private void CreateGrid()
    {
        _grid = new Node[gridSize, gridSize];
        
        int center = gridSize / 2;
        for (int i = 0; i < gridSize; i++)
        for (int ii = 0; ii < gridSize; ii++)
        {
            bool isCore = i == center && ii == center;
            string name = $"{i + 1}-{ii + 1}";
            
            Node node = new Node(name, ii, i, isCore);
            _grid[i, ii] = node;
            Add(node);
        }
    }

    public Node GetNode(Vector2 pos)
    {
        for (int i = 0; i < gridSize; i++)
        for (int ii = 0; ii < gridSize; ii++)
        {
            if (_grid[i, ii].worldBound.Contains(pos))
                return _grid[i, ii];
        }

        return null;
    }

    public Node GetNode(int x, int y)
    {
        return _grid[y, x];
    }
}