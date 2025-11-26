using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Pathfinding
{
    public class NodeMap : MonoBehaviour
    {
        [SerializeField] private Vector2 _mapOrigin;
        [SerializeField] private Vector2 _nodesWorldSpacing;
        [SerializeField] private Vector2Int _mapSize;
        [Space]
        [SerializeField] private int _ortogonalTravelCost;
        [SerializeField] private int _diagonalTravelCost;

        private Node[,] _nodes;

        private Node _closestToMouseNode;

        private int _lastQuery = -1;
        
        private void Awake()
        {
            _nodes = new Node[_mapSize.x, _mapSize.y];

            for (int y = 0; y < _nodes.GetLength(1); y++)
            {
                for (int x = 0; x < _nodes.GetLength(0); x++)
                {
                    Vector2 nodeWorldPosition = _mapOrigin + new Vector2(x, y) * _nodesWorldSpacing;
                    _nodes[x, y] = new Node(nodeWorldPosition, new Vector2Int(x, y));
                }
            }
            CalculateAllTravelCosts();
        }

        public bool IsPointPassable(Vector2 worldPoint)
        {
            Node node = GetClosestNode(worldPoint);
            return node.Passable;
        }

        public bool TryFindPath(Vector2 worldStart, Vector2 worldTarget, out INodeWorld[] path)
        {
            Node startNode = GetClosestNode(worldStart);
            Node targetNode = GetClosestNode(worldTarget);
            return TryFindPath(startNode, targetNode, out path);
        }
        
        private bool TryFindPath(Node startNode, Node targetNode, out INodeWorld[] path)
        {
            if (startNode == targetNode || ! targetNode.Passable)
            {
                path = Array.Empty<Node>();
                return false;
            }

            _lastQuery++;
            List<Node> pendingNodes = new(){startNode};
            
            startNode.G = 0;
            startNode.H = GetNodeH(startNode, targetNode.MapCoordinates);
            startNode.LastQuery = _lastQuery;
            startNode.WasProcessedThisQuery = false;

            while (pendingNodes.Count > 0)
            {
                Node currentNode = GetBestPendingNode(pendingNodes, out int currentNodeIndex);
                if (currentNode.Equals(targetNode))
                {
                    path = GetPathFromFinalNode(currentNode);
                    return true;
                }
                
                for (int yOffset = -1; yOffset <= 1; yOffset++)
                {
                    for (int xOffset = -1; xOffset <= 1; xOffset++)
                    {
                        if (xOffset == 0 && yOffset == 0)
                            continue;
                        int x = currentNode.MapCoordinates.x + xOffset;
                        int y = currentNode.MapCoordinates.y + yOffset;
                        if (x < 0 || x >= _nodes.GetLength(0) || y < 0 || y >= _nodes.GetLength(1))
                            continue;
                        Node adjacentNode = _nodes[x, y];
                        if ( ! adjacentNode.Passable)
                            continue;
                        if (adjacentNode.LastQuery < _lastQuery)
                        {
                            adjacentNode.LastQuery = _lastQuery;
                            adjacentNode.WasProcessedThisQuery = false;
                            adjacentNode.PreviousNode = null;
                            adjacentNode.H = GetNodeH(adjacentNode, targetNode.MapCoordinates);
                            adjacentNode.G = int.MaxValue;
                            pendingNodes.Add(adjacentNode);
                        }
                        if (adjacentNode.WasProcessedThisQuery)
                            continue;

                        bool diagonal = xOffset != 0 && yOffset != 0;
                        
                        int newG = currentNode.G;
                        newG += diagonal ? _diagonalTravelCost : _ortogonalTravelCost;

                        if (newG > adjacentNode.G)
                            continue;
                        adjacentNode.PreviousNode = currentNode;
                        adjacentNode.G = newG;
                    }
                }
                pendingNodes.RemoveAt(currentNodeIndex);
                currentNode.WasProcessedThisQuery = true;
            }
            Debug.LogError($"No path was found from {startNode.WorldPosition} to {targetNode.WorldPosition}");
            path = Array.Empty<Node>();
            return false;
        }

        private static Node GetBestPendingNode(List<Node> pendingNodes, out int index)
        {
            Node result = null;
            index = 0;
            int minF = Int32.MaxValue;
            int minH = Int32.MaxValue;
            for (int i = 0; i < pendingNodes.Count; i++)
            {
                Node pendingNode = pendingNodes[i];
                if (pendingNode.F < minF)
                {
                    index = i;
                    result = pendingNode;
                    minH = pendingNode.H;
                    minF = pendingNode.F;
                }
                else if (pendingNode.F == minF && pendingNode.H < minH)
                {
                    index = i;
                    result = pendingNode;
                    minH = pendingNode.H;
                }
            }
            return result;
        }

        private Node[] GetPathFromFinalNode(Node finalNode)
        {
            List<Node> result = new();
            Node currentNode = finalNode;
            while (currentNode != null)
            {
                if (result.Contains(currentNode))
                    break;
                result.Insert(0, currentNode);
                currentNode = currentNode.PreviousNode;
            }

            return result.ToArray();
        }

        private int GetNodeH(Node node, Vector2Int target)
        {
            Vector2Int difference = new(Mathf.Abs(node.MapCoordinates.x - target.x), Mathf.Abs(node.MapCoordinates.y - target.y));
            
            int diagonalSteps = Mathf.Min(difference.x, difference.y);
            int ortogonalSteps = Mathf.Max(difference.x, difference.y) - diagonalSteps;
            
            return diagonalSteps * _diagonalTravelCost + ortogonalSteps * _ortogonalTravelCost;
            
        }

        private Node GetClosestNode(Vector2 worldPosition)
        {
            Vector2 relativePosition = worldPosition - _mapOrigin;
            Vector2 mapCoordinates = new Vector2(relativePosition.x / _nodesWorldSpacing.x,   relativePosition.y / _nodesWorldSpacing.y);
            mapCoordinates.x = Mathf.Clamp(mapCoordinates.x, 0, _nodes.GetLength(0) - 1);
            mapCoordinates.y = Mathf.Clamp(mapCoordinates.y, 0, _nodes.GetLength(1) - 1);
            Vector2Int nodeCoordinates = new Vector2Int(Mathf.RoundToInt(mapCoordinates.x), Mathf.RoundToInt(mapCoordinates.y));
            return _nodes[nodeCoordinates.x, nodeCoordinates.y];
        }

        private void CalculateAllTravelCosts()
        {
            for (int y = 0; y < _nodes.GetLength(1); y++)
            {
                for (int x = 0; x < _nodes.GetLength(0); x++)
                {
                    _nodes[x, y].CalculateTravelCost();
                }
            }
        }

        private void Update()
        {
            _closestToMouseNode = GetClosestNode(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        }

        private void OnDrawGizmos()
        {
            if (_nodes == null)
                return;

            for (int y = 0; y < _nodes.GetLength(1); y++)
            {
                for (int x = 0; x < _nodes.GetLength(0); x++)
                {
                    Node node = _nodes[x, y];
                    if (node == _closestToMouseNode)
                        Gizmos.color = Color.yellow;
                    else if ( ! node.Passable)
                        Gizmos.color = Color.black;
                    else
                        continue;

                    Gizmos.DrawCube(_nodes[x, y].WorldPosition, Vector3.one * 0.1f);
                }
            }

            for (int y = 0; y < _nodes.GetLength(1); y++)
            {
                for (int x = 0; x < _nodes.GetLength(0); x++)
                {
                    if (_nodes[x, y].PreviousNode == null)
                        continue;
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(_nodes[x,  y].WorldPosition, _nodes[x, y].PreviousNode.WorldPosition);
                }
            }
        }
    }
}