using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class Grid : VisualElement
{
    private MainUI _mainUI;
    
    [UxmlAttribute] public int gridSize = 5;
    
    private Node[,] _grid;
    private List<Node> _powerNodes;
    
    public List<Battery> batteries;
    
    public Grid()
    {
        AddToClassList("grid");
        CreateGrid();
        _powerNodes = new();
        batteries = new();
    }

    public Grid(MainUI mainUI)
    {
        _mainUI = mainUI;
        AddToClassList("grid");
        _powerNodes = new();
        batteries = new();
        CreateGrid();
    }

    private void CreateGrid()
    {
        _grid = new Node[gridSize, gridSize];
        
        int center = gridSize / 2;
        for (int i = 0; i < gridSize; i++)
        for (int ii = 0; ii < gridSize; ii++)
        {
            bool isPowered = i == center && (ii == 0 || ii == gridSize - 1)
                             || ii == center && (i == 0 || i == gridSize - 1);
            string name = $"{i + 1}-{ii + 1}";
            
            Node node = new Node(_mainUI, name, ii, i);
            _grid[i, ii] = node;
            Add(node);

            if (isPowered) _powerNodes.Add(node);
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

    public List<Node> GetNeighborNodes(Node node)
    {
        List<Node> neighbors = new();
        int[] dx = { -1, 0, 1, 0 };
        int[] dy = { 0, -1, 0, 1 };

        for (int i = 0; i < 4; i++)
        {
            int col = node.x + dx[i];
            int row = node.y + dy[i];
            if (col < 0 || row < 0 || gridSize <= col || gridSize <= row) continue;
            neighbors.Add(_grid[col, row]);
        }

        return neighbors;
    }

    public void AddBattery(Battery battery) => batteries.Add(battery);

    public void RemoveBattery(Battery battery)
    {
        foreach (var item in batteries)
        {
            if (item == battery)
            {
                batteries.Remove(battery);
                break;
            }
        }
    }
}