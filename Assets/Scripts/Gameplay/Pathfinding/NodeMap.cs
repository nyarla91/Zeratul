using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Gameplay.Data.Configs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Pathfinding
{
    public class NodeMap : MonoBehaviour
    {
        [SerializeField] private PathfindingConfig _config;
        [SerializeField] private Vector2Int _mapSize;

        private Node[,] _nodes;

        private Node _closestToMouseNode;

        private int _lastQuery = -1;

        public Vector2Int MapSize => _mapSize;

        private void Awake()
        {
            _nodes = new Node[_mapSize.x, _mapSize.y];

            for (int y = 0; y < _nodes.GetLength(1); y++)
            {
                for (int x = 0; x < _nodes.GetLength(0); x++)
                {
                    Vector2 nodeWorldPosition = _config.MapOrigin + new Vector2(x, y) * _config.NodesWorldSpacing;
                    _nodes[x, y] = new Node(nodeWorldPosition, new Vector2Int(x, y));
                }
            }
            RecalculateAllObstacles();
        }

        public bool IsPointPassable(Vector2 worldPoint, bool isAgentAir)
        {
            Node node = GetClosestNode(worldPoint);
            return node.IsPassable(isAgentAir);
        }
        
        public bool TryFindPath(Vector2 worldStart, Vector2 worldTarget, out INodeWorld[] path, bool isAgentAir)
        {
            Node startNode = GetClosestNode(worldStart);
            Node targetNode = GetClosestNode(worldTarget);
            return TryFindPath(startNode, targetNode, out path, isAgentAir);
        }
        
        private bool TryFindPath(Node startNode, Node targetNode, out INodeWorld[] path, bool isAgentAir)
        {
            if (startNode == targetNode || ! targetNode.IsPassable(isAgentAir))
            {
                path = Array.Empty<INodeWorld>();
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
                    path = GetPathFromFinalNode(currentNode, startNode);
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
                        if ( ! adjacentNode.IsPassable(isAgentAir))
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
                        newG += diagonal ? _config.DiagonalTravelCost : _config.OrtogonalTravelCost;

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
            int minF = int.MaxValue;
            int minH = int.MaxValue;
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

        private Node[] GetPathFromFinalNode(Node finalNode, Node originNode)
        {
            List<Node> result = new();
            Node currentNode = finalNode;
            while (currentNode != null)
            {
                if (currentNode == originNode || result.Contains(currentNode))
                {
                    result.Insert(0,  currentNode);
                    break;
                }
                Node nextNode = result.FirstOrDefault();
                if (nextNode != null && currentNode.PreviousNode != null)
                {
                    Vector2 deltaPrevious = currentNode.PreviousNode.MapCoordinates - currentNode.MapCoordinates;
                    Vector2 deltaNext = currentNode.MapCoordinates - nextNode.MapCoordinates;
                    if (Vector2.Angle(deltaNext, deltaPrevious) < 5)
                    {
                        currentNode = currentNode.PreviousNode;
                        continue;
                    }
                }
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
            
            return diagonalSteps * _config.DiagonalTravelCost + ortogonalSteps * _config.OrtogonalTravelCost;
            
        }

        private Node GetClosestNode(Vector2 worldPosition)
        {
            Vector2 relativePosition = worldPosition - _config.MapOrigin;
            Vector2 mapCoordinates = new Vector2(relativePosition.x / _config.NodesWorldSpacing.x,   relativePosition.y / _config.NodesWorldSpacing.y);
            mapCoordinates.x = Mathf.Clamp(mapCoordinates.x, 0, _nodes.GetLength(0) - 1);
            mapCoordinates.y = Mathf.Clamp(mapCoordinates.y, 0, _nodes.GetLength(1) - 1);
            Vector2Int nodeCoordinates = new Vector2Int(Mathf.RoundToInt(mapCoordinates.x), Mathf.RoundToInt(mapCoordinates.y));
            return _nodes[nodeCoordinates.x, nodeCoordinates.y];
        }

        private void RecalculateAllObstacles()
        {
            for (int y = 0; y < _nodes.GetLength(1); y++)
            {
                for (int x = 0; x < _nodes.GetLength(0); x++)
                {
                    _nodes[x, y].RecalculateObstacles();
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
                    else
                        continue;

                    Gizmos.DrawCube(_nodes[x, y].WorldPosition, Vector3.one * 0.1f);
                }
            }
/*
            for (int y = 0; y < _nodes.GetLength(1); y++)
            {
                for (int x = 0; x < _nodes.GetLength(0); x++)
                {
                    if (_nodes[x, y].PreviousNode == null)
                        continue;
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(_nodes[x,  y].WorldPosition, _nodes[x, y].PreviousNode.WorldPosition);
                }
            }*/
        }
    }
}