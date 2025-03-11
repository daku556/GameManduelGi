using UnityEngine;
using UnityEngine.UI;

public class InventoryGrid : MonoBehaviour
{
    public GridLayoutGroup gridLayout;
    public int rows = 7; // 원하는 행 개수
    public int columns = 6; // 원하는 열 개수

    void Start()
    {
        AdjustCellSize();
    }

    void AdjustCellSize()
    {
        RectTransform parentRect = gridLayout.GetComponent<RectTransform>();

        // 부모 패널의 크기 가져오기 (패딩 포함)
        float width = parentRect.rect.width - gridLayout.padding.left - gridLayout.padding.right;
        float height = parentRect.rect.height - gridLayout.padding.top - gridLayout.padding.bottom;

        // 셀 크기 계산 (간격 고려)
        float cellWidth = (width - (gridLayout.spacing.x * (columns - 1))) / columns;
        float cellHeight = (height - (gridLayout.spacing.y * (rows - 1))) / rows;

        gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
    }
}
