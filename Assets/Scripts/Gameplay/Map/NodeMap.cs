using System.Collections.Generic;
using System.Linq;
using _Core;
using Gameplay.Data.Configs;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Map
{
    public class NodeMap : MonoBehaviour
    {
        [SerializeField] private PathfindingConfig _config;
        [SerializeField] private Vector2Int _mapSize;

        private Vector2[] _bypassDirections;

        public Vector2[] BypassDirections
        {
            get
            {
                if ( _bypassDirections != null)
                    return _bypassDirections;
                float[] angles = { 0, 45, 90, 135, 180, 225, 270, 315 };
                _bypassDirections = angles
                    .Select(a => a.DegreesToVector2())
                    .Select(d => d * Isometry.Scale)
                    .ToArray();
                return _bypassDirections;
            }
        }

        private Node[] _nodes;
        private Node _closestToMouseNode;
        private int _lastQuery;
        private int _nextIsland;
        private Queue<Bounds> _obstacleRecalculationQueue = new();

        public Vector2Int MapSize => _mapSize;

        private void Awake()
        {
            _nodes = new Node[_mapSize.x * _mapSize.y];

            for (int i = 0; i < _nodes.Length;i++)
            {
                int x = i % _mapSize.x;
                int y = i / _mapSize.x;

                Vector2 nodeWorldPosition = _config.MapOrigin + new Vector2(x, y) * _config.NodesWorldSpacing;
                _nodes[i] = new Node(_config, nodeWorldPosition, new Vector2Int(x, y));
            }
            Observable.EveryFixedUpdate()
                .Where(_ => _obstacleRecalculationQueue.Count > 0)
                .Subscribe(_ => RecalculateAllObstacles(_obstacleRecalculationQueue.Dequeue()));
        }

        public void Init()
        {
            RecalculateAllObstacles();
        }

        public void QueueObstacleRecalculation(Bounds bounds) => _obstacleRecalculationQueue.Enqueue(bounds);

        public bool CanPassBetween(Vector2 worldStart, Vector2 worldTarget, PathfindingAgent agent)
            => CanPassBetween(worldStart, worldTarget, agent, out _);

        public bool CanPassBetween(Vector2 worldStart, Vector2 worldTarget, PathfindingAgent agent, out RaycastHit2D hit)
        {
            LayerMask layerMask = agent.IsAir ? _config.CommonLayerMask : _config.GroundLayerMask;
            
            Vector2 direction = worldTarget - worldStart;
            float distance = direction.magnitude;

            hit = Physics2D.BoxCast(worldStart, agent.BoundingBoxSize, 0, direction, distance, layerMask);
            return hit.collider == null;
        }

        public bool TryFindPath(Vector2 worldStart, Vector2 worldTarget, out List<Vector2> path, PathfindingAgent agent)
        {
            if (CanPassBetween(worldStart, worldTarget, agent))
            {
                path = new List<Vector2> { worldTarget };
                return true;
            }
            Node startNode = GetClosestNode(worldStart);
            Node targetNode = GetClosestNode(worldTarget);

            if ( ! TryFindPath(startNode, targetNode, out path, agent))
            {
                if ( ! TryFindBypassPath(startNode, worldTarget, out path, agent))
                    return false;
                Vector2 pathLast = path.Last();
                Vector2 direction = pathLast.DirectionTo(worldTarget);
                Ray ray = new Ray(pathLast, direction);
                LayerMask mask = agent.IsAir ? _config.CommonLayerMask : _config.GroundLayerMask;
                worldTarget = Physics2D.CircleCast(pathLast, 0.1f, direction, _config.BypassDistance, mask).point;
            }
            
            path = SimplifyPath(path, worldStart, worldTarget, agent);
            return true;
        }

        private bool TryFindPath(Node startNode, Node targetNode, out List<Vector2> path, PathfindingAgent agent)
        {
            if (startNode == targetNode || ! targetNode.IsPassable(agent.IsAir) || ! startNode.IsOnTheSameIsland(targetNode))
            {
                path = new List<Vector2>();
                return false;
            }

            _lastQuery++;
            
            startNode.G = 0;
            startNode.H = GetNodeH(startNode, targetNode.MapCoordinates);
            startNode.LastQuery = _lastQuery;
            startNode.WasProcessedThisQuery = false;
            
            PriorityQueue<Node, int> nodesQueue =new();
            nodesQueue.Enqueue(startNode, startNode.Priority);

            while (nodesQueue.Count > 0)
            {
                Node currentNode = nodesQueue.Dequeue();
                if (currentNode.Equals(targetNode))
                {
                    path = GetPathFromFinalNode(currentNode, startNode).Select(n => n.WorldPosition).ToList();
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
                        if (x < 0 || x >= _mapSize.x || y < 0 || y >= _mapSize.y)
                            continue;
                        Node neightbor = _nodes[x + y * _mapSize.x];
                        if ( ! neightbor.IsPassable(agent.IsAir))
                            continue;
                        bool firstProcessing = neightbor.LastQuery < _lastQuery;
                        if (firstProcessing)
                        {
                            neightbor.LastQuery = _lastQuery;
                            neightbor.WasProcessedThisQuery = false;
                            neightbor.PreviousNode = null;
                            neightbor.H = GetNodeH(neightbor, targetNode.MapCoordinates);
                            neightbor.G = int.MaxValue;
                        }

                        if (neightbor.WasProcessedThisQuery)
                            continue;

                        bool diagonal = xOffset != 0 && yOffset != 0;
                        
                        int newG = currentNode.G;
                        newG += diagonal ? _config.DiagonalTravelCost : _config.OrtogonalTravelCost;
                        if (neightbor.ObstacleDistanceFor(agent.IsAir) < agent.Radius)
                            newG += _config.TooCloseToObstaclePenalty;

                        if (newG > neightbor.G)
                            continue;
                        neightbor.PreviousNode = currentNode;
                        neightbor.G = newG;
                        if (firstProcessing)
                            nodesQueue.Enqueue(neightbor, neightbor.Priority);
                    }
                }
                currentNode.WasProcessedThisQuery = true;
            }
            Debug.LogError($"No path was found from {startNode.WorldPosition} to {targetNode.WorldPosition}");
            FillIsland(startNode);
            FillIsland(targetNode);
            path = new List<Vector2>();
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

        private bool TryFindBypassPath(Node startNode, Vector2 worldTarget, out List<Vector2> path, PathfindingAgent agent)
        {
            int shortestPathNodes = int.MaxValue;
            path = null;
            
            foreach (Vector2 bypassDirection in BypassDirections)
            {
                Vector2 worldBypassTarget = worldTarget + bypassDirection * _config.BypassDistance;
                Node targetNBypassNode = GetClosestNode(worldBypassTarget);
                if ( ! TryFindPath(startNode, targetNBypassNode, out List<Vector2> bypassPath, agent))
                    continue;
                if (bypassPath.Count > shortestPathNodes)
                    continue;
                shortestPathNodes = bypassPath.Count;
                path = bypassPath;
            }
            return path != null;
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
            Vector2 mapCoordinates = new(relativePosition.x / _config.NodesWorldSpacing.x,   relativePosition.y / _config.NodesWorldSpacing.y);
            mapCoordinates.x = Mathf.Clamp(mapCoordinates.x, 0, _mapSize.x);
            mapCoordinates.y = Mathf.Clamp(mapCoordinates.y, 0, _mapSize.y);
            Vector2Int nodeCoordinates = new Vector2Int(Mathf.RoundToInt(mapCoordinates.x), Mathf.RoundToInt(mapCoordinates.y));
            return _nodes[nodeCoordinates.x + nodeCoordinates.y * _mapSize.x];
        }

        private List<Vector2> SimplifyPath(List<Vector2> path, Vector2 worldStart, Vector2 worldTarget, PathfindingAgent agent)
        {
            List<Vector2> result = new();
            
            for (int i = 1; i < path.Count; i++)
            {
                Vector2 previousPoint = result.Count == 0 ? worldStart : result.Last();
                Vector2 questionedPoint = path[i - 1];
                Vector2 checkingPoint = path[i];
                if ( ! CanPassBetween(previousPoint, checkingPoint, agent))
                    result.Add(questionedPoint);
            }
            result.Add(worldTarget);
            return result;
        }

        private async void RecalculateAllObstacles() =>
            RecalculateAllObstacles(Vector2Int.zero, _mapSize - new Vector2Int(1, 1), out _);

        private void RecalculateAllObstacles(Bounds bounds)
        {
            Vector2Int min = GetClosestNode(bounds.min).MapCoordinates;
            Vector2Int max = GetClosestNode(bounds.max).MapCoordinates;
            RecalculateAllObstacles(min, max, out bool includesDifferentIslands);
            if (includesDifferentIslands)
                FillIsland(GetClosestNode(bounds.center));
        }

        private void RecalculateAllObstacles(Vector2Int min,  Vector2Int max, out bool includesDifferentIslands)
        {
            includesDifferentIslands = false;
            int firstIsland = -1;
            for (int y = min.y; y <= max.y; y++)
            {
                for (int x = min.x; x <= max.x; x++)
                {
                    Node node = _nodes[x + y * _mapSize.x];
                    node.RecalculateObstacles();
                    if (firstIsland == -1)
                        firstIsland = node.Island;
                    else if (firstIsland != node.Island)
                        includesDifferentIslands = true;
                }
            }
        }

        private void FillIsland(Node startNode)
        {
            Queue<Node> queue = new();
            queue.Enqueue(startNode);

            while (queue.Count > 0)
            {
                Node currentNode = queue.Dequeue();
                currentNode.Island = _nextIsland;
                
                for (int yOffset = -1; yOffset <= 1; yOffset++)
                {
                    for (int xOffset = -1; xOffset <= 1; xOffset++)
                    {
                        if (Mathf.Abs(xOffset) == Mathf.Abs(yOffset))
                            continue;
                        int x = currentNode.MapCoordinates.x + xOffset;
                        int y = currentNode.MapCoordinates.y + yOffset;
                        Node neighbor = _nodes[x + y * _mapSize.x];
                        if (neighbor.Island == currentNode.Island || ! neighbor.IsPassableByGround)
                            continue;
                        neighbor.Island = _nextIsland;
                        queue.Enqueue(neighbor);
                    }
                }
            }
            _nextIsland++;
        }

        private void Update()
        {
            _closestToMouseNode = GetClosestNode(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        }

        private void OnDrawGizmos()
        {
            if (_nodes == null)
                return;

            for (int i = 0; i < _nodes.Length; i++)
            {
                Node node = _nodes[i];
                if (node == _closestToMouseNode)
                    Gizmos.color = Color.yellow;
                else if (node.LastQuery == _lastQuery)
                    Gizmos.color = Color.cyan;
                else if ( ! node.IsPassableByGround)
                    Gizmos.color = Color.red;
                else
                    Gizmos.color = Color.white;

                Gizmos.DrawCube(_nodes[i].WorldPosition, Vector3.one * 0.1f);
            }
        }
    }
}