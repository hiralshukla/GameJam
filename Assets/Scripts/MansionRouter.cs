using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using System;

public class MansionRouter : MonoBehaviour
{
    [Header("Special Rooms")]
    public string startRoomId = "Start";
    public string ballroomRoomId = "Ballroom";

    string[] randomizeRooms = { "Kitchen", "Basement", "Lounge", "GameRoom", "Theater", "Conservatory", "Patio" };
    string[] stableRooms = {};
    public int[,] gameMap = new int[3, 3];

    private Dictionary<(string, Direction), string> doorMap = new Dictionary<(string, Direction), string>();
    private Dictionary<string, Vector2Int> roomPositions = new Dictionary<string, Vector2Int>();
    private HashSet<string> anchoredRooms = new HashSet<string>();
    private List<string> indexToRoomId = new List<string>();
    private Dictionary<string, int> roomIdToIndex = new Dictionary<string, int>();
    private readonly Vector2Int startPos = new Vector2Int(2, 1);    // bottom middle
    private readonly Vector2Int ballroomPos = new Vector2Int(0, 2); // top right

    [Header("Debug")]
    public bool debugLogs = true;
    public bool logDoorMap = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeNewRun();
        DebugPrintGrid("After InitializeNewRun()");
        DebugPrintAllDoorMap();
        DebugPrintDoorMapForRoom(startRoomId);

    }

    public string GetDestinationRoomId(string currentRoomId, Direction exitDirection)
    {
        var key = (currentRoomId, exitDirection);

        if (!doorMap.TryGetValue(key, out var dest))
        {
            Debug.LogWarning($"[MansionRouter] No mapping for {currentRoomId} {exitDirection}. Falling back to {startRoomId}.");
            dest = startRoomId;
        }

        if (debugLogs)
            Debug.Log($"[MansionRouter] Route: {currentRoomId} exit {exitDirection} -> {dest}");

        return dest;
    }

    public void Reshuffle()
    {
        Debug.Log("[MansionRouter] Reshuffling mansion layout...");
        ShuffleMovableRooms();
        RebuildDoorMapFromGrid();

        DebugPrintGrid("After Reshuffle()");
        DebugPrintAllDoorMap();
        DebugPrintDoorMapForRoom(startRoomId);
    }

    public void SetRoomAnchored(string roomId, bool anchored)
    {
        if (anchored) anchoredRooms.Add(roomId);
        else anchoredRooms.Remove(roomId);
    }

    public void ResetMansion()
    {
        anchoredRooms.Clear();
        InitializeNewRun();
    }

    public bool TryGetRoomPosition(string roomId, out Vector2Int pos) =>
        roomPositions.TryGetValue(roomId, out pos);

    private void InitializeNewRun()
    {
        BuildRoomIndexTables();
        ClearGrid();

        // Place fixed rooms
        PlaceRoomAt(startRoomId, startPos.x, startPos.y);
        PlaceRoomAt(ballroomRoomId, ballroomPos.x, ballroomPos.y);

        // Place stable rooms first (if any)
        foreach (var stable in stableRooms)
        {
            if (string.IsNullOrWhiteSpace(stable)) continue;
            PlaceRoomInFirstEmptyCell(stable);
        }

        // Place randomized rooms into remaining empty cells
        var rooms = new List<string>(randomizeRooms);
        ShuffleList(rooms);

        foreach (var r in rooms)
            PlaceRoomInFirstEmptyCell(r);

        RebuildDoorMapFromGrid();
    }

    private void BuildRoomIndexTables()
    {
        indexToRoomId.Clear();
        roomIdToIndex.Clear();

        // Ensure start + ballroom exist even if arrays change
        AddRoomIdIfMissing(startRoomId);
        AddRoomIdIfMissing(ballroomRoomId);

        foreach (var s in stableRooms) AddRoomIdIfMissing(s);
        foreach (var r in randomizeRooms) AddRoomIdIfMissing(r);

        void AddRoomIdIfMissing(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return;
            if (roomIdToIndex.ContainsKey(id)) return;

            roomIdToIndex[id] = indexToRoomId.Count;
            indexToRoomId.Add(id);
        }
    }

    private void ClearGrid()
    {
        roomPositions.Clear();

        for (int row = 0; row < gameMap.GetLength(0); row++)
            for (int col = 0; col < gameMap.GetLength(1); col++)
                gameMap[row, col] = -1;
    }

    private void PlaceRoomAt(string roomId, int row, int col)
    {
        if (!roomIdToIndex.TryGetValue(roomId, out int idx))
        {
            Debug.LogError($"[MansionRouter] Unknown roomId '{roomId}'. Add it to randomizeRooms/stableRooms.");
            return;
        }

        gameMap[row, col] = idx;
        roomPositions[roomId] = new Vector2Int(row, col);
    }

    private void PlaceRoomInFirstEmptyCell(string roomId)
    {
        for (int row = 0; row < gameMap.GetLength(0); row++)
        {
            for (int col = 0; col < gameMap.GetLength(1); col++)
            {
                if (gameMap[row, col] == -1)
                {
                    PlaceRoomAt(roomId, row, col);
                    return;
                }
            }
        }

        Debug.LogWarning($"[MansionRouter] No empty cell to place '{roomId}'. Grid too small?");
    }

    private string RoomIdAt(int row, int col)
    {
        int idx = gameMap[row, col];
        if (idx < 0 || idx >= indexToRoomId.Count) return null;
        return indexToRoomId[idx];
    }

    private void ShuffleMovableRooms()
    {
        // Fixed room IDs = start + ballroom + stable + anchored
        var fixedIds = new HashSet<string>(anchoredRooms);
        fixedIds.Add(startRoomId);
        fixedIds.Add(ballroomRoomId);
        foreach (var s in stableRooms) if (!string.IsNullOrWhiteSpace(s)) fixedIds.Add(s);

        // Collect fixed cells & movable cells
        var fixedCells = new HashSet<Vector2Int>();
        roomPositions.Clear(); // we will rebuild positions after moving

        // First scan grid, record where fixed rooms currently are, and gather movable rooms/cells
        var movableRooms = new List<string>();
        var movableCells = new List<Vector2Int>();

        for (int row = 0; row < gameMap.GetLength(0); row++)
        {
            for (int col = 0; col < gameMap.GetLength(1); col++)
            {
                var id = RoomIdAt(row, col);
                if (id == null) continue;

                if (fixedIds.Contains(id))
                    fixedCells.Add(new Vector2Int(row, col));
                else
                    movableRooms.Add(id);
            }
        }

        for (int row = 0; row < gameMap.GetLength(0); row++)
        {
            for (int col = 0; col < gameMap.GetLength(1); col++)
            {
                var cell = new Vector2Int(row, col);
                if (!fixedCells.Contains(cell))
                    movableCells.Add(cell);
            }
        }

        // Clear all non-fixed cells
        foreach (var cell in movableCells)
            gameMap[cell.x, cell.y] = -1;

        // Shuffle movable rooms and re-place them
        ShuffleList(movableRooms);

        int count = Mathf.Min(movableRooms.Count, movableCells.Count);
        for (int i = 0; i < count; i++)
            PlaceRoomAt(movableRooms[i], movableCells[i].x, movableCells[i].y);

        // Re-place fixed room positions (scan grid to find them OR place them deterministically)
        // We place deterministically to guarantee Start/Ballroom stay in their designed spots:
        PlaceRoomAt(startRoomId, startPos.x, startPos.y);
        PlaceRoomAt(ballroomRoomId, ballroomPos.x, ballroomPos.y);

        // Stable rooms: if you want them fixed to their original cell, you can store their cell.
        // For now we simply ensure they exist somewhere (if missing due to grid clear), place them:
        foreach (var s in stableRooms)
        {
            if (string.IsNullOrWhiteSpace(s)) continue;
            if (!roomPositions.ContainsKey(s))
                PlaceRoomInFirstEmptyCell(s);
        }

        // Anchored rooms: ensure they still exist (they should, since we treated them as fixedIds)
        // Rebuild roomPositions for any fixed room already on the grid:
        RebuildRoomPositionsFromGrid();
    }

    private void RebuildRoomPositionsFromGrid()
    {
        roomPositions.Clear();
        for (int row = 0; row < gameMap.GetLength(0); row++)
        {
            for (int col = 0; col < gameMap.GetLength(1); col++)
            {
                var id = RoomIdAt(row, col);
                if (id != null)
                    roomPositions[id] = new Vector2Int(row, col);
            }
        }
    }

    private void RebuildDoorMapFromGrid()
    {
        doorMap.Clear();
        RebuildRoomPositionsFromGrid();

        int rows = gameMap.GetLength(0);
        int cols = gameMap.GetLength(1);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                var fromId = RoomIdAt(row, col);
                if (fromId == null) continue;

                foreach (Direction dir in Enum.GetValues(typeof(Direction)))
                {
                    var (nr, nc) = Neighbor(row, col, dir);

                    string toId = null;
                    if (nr >= 0 && nr < rows && nc >= 0 && nc < cols)
                        toId = RoomIdAt(nr, nc);

                    // Fallback to Start if out of bounds or empty cell
                    if (string.IsNullOrEmpty(toId))
                        toId = startRoomId;

                    doorMap[(fromId, dir)] = toId;
                }
            }
        }
    }

    private (int nr, int nc) Neighbor(int row, int col, Direction dir)
    {
        return dir switch
        {
            Direction.N => (row - 1, col),
            Direction.E => (row, col + 1),
            Direction.S => (row + 1, col),
            Direction.W => (row, col - 1),
            _ => (row, col)
        };
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }




    private void DebugPrintGrid(string label)
    {
        if (!debugLogs) return;

        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"[MansionRouter] {label}");
        sb.AppendLine("Grid (row 0 = top):");

        int rows = gameMap.GetLength(0);
        int cols = gameMap.GetLength(1);

        for (int r = 0; r < rows; r++)
        {
            sb.Append("| ");
            for (int c = 0; c < cols; c++)
            {
                string id = RoomIdAt(r, c);
                sb.Append(string.IsNullOrEmpty(id) ? "----" : id);
                sb.Append(" | ");
            }
            sb.AppendLine();
        }

        Debug.Log(sb.ToString());
    }

    private void DebugPrintDoorMapForRoom(string roomId)
    {
        if (!debugLogs) return;

        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"[MansionRouter] Door routes from '{roomId}':");

        foreach (Direction dir in System.Enum.GetValues(typeof(Direction)))
        {
            var key = (roomId, dir);
            if (doorMap.TryGetValue(key, out var dest))
                sb.AppendLine($"  {dir} -> {dest}");
            else
                sb.AppendLine($"  {dir} -> (missing)");
        }

        Debug.Log(sb.ToString());
    }

    private void DebugPrintAllDoorMap()
    {
        if (!debugLogs || !logDoorMap) return;

        var sb = new System.Text.StringBuilder();
        sb.AppendLine("[MansionRouter] Full doorMap:");

        foreach (var kvp in doorMap)
            sb.AppendLine($"  ({kvp.Key.Item1}, {kvp.Key.Item2}) -> {kvp.Value}");

        Debug.Log(sb.ToString());
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Reshuffle();

        if (Input.GetKeyDown(KeyCode.T))
            ResetMansion();
    }

}


