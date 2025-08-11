using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition2D : MonoBehaviour
{
    [Header("External References")]
    [SerializeField] Camera SceneCamera;
    [SerializeField] GameObject MousePointerObject;
    [SerializeField] GridManager gridManager;
    [SerializeField] SelectedTileIndicator TileIndicatorPrefab;

    //Internal Variables
    GameObject MousePointer;
    SelectedTileIndicator TileIndicator;

    private void Awake()
    {
        //MousePointer = Instantiate(MousePointerObject, new Vector3(0f,0f,0f), Quaternion.identity);
        //MousePointer.transform.parent = transform;

        MousePointer = Instantiate(MousePointerObject, transform);
        TileIndicator = Instantiate(TileIndicatorPrefab, transform);
        TileIndicator.gameObject.SetActive(false);
    }

    void Start()
    {

    }

    void Update()
    {
        Vector3 mouseWorldPosition = SceneCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        //Vector3Int GridPosition = grid.WorldToCell(mouseWorldPosition);
        Vector3Int GridPosition = gridManager.Grid.WorldToCell(mouseWorldPosition);
        //MousePointer.transform.position = mouseWorldPosition;
        //MousePointer.transform.position = grid.CellToWorld(GridPosition) + (grid.cellSize / 2);
        MousePointer.transform.position = gridManager.Grid.CellToWorld(GridPosition) + (gridManager.Grid.cellSize / 2);
    }



    public bool IsMousePointerOverGameGrid()
    {
        return gridManager.IsOverGameGrid(MousePointer.transform.position);
    }

    public void SetSelectedTile()
    {
        TileIndicator.gameObject.SetActive(true);
        //Vector3 pos = MousePointer.transform.position;
        TileIndicator.transform.position = MousePointer.transform.position;
    }

    public Vector3 GetMousePointerPosition()
    {
        return MousePointer.transform.position;
    }
}
