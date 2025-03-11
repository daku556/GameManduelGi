using UnityEngine;
using UnityEngine.UI;

public class InventoryGrid : MonoBehaviour
{
    public GridLayoutGroup gridLayout;
    public int rows = 7; // ���ϴ� �� ����
    public int columns = 6; // ���ϴ� �� ����

    void Start()
    {
        AdjustCellSize();
    }

    void AdjustCellSize()
    {
        RectTransform parentRect = gridLayout.GetComponent<RectTransform>();

        // �θ� �г��� ũ�� �������� (�е� ����)
        float width = parentRect.rect.width - gridLayout.padding.left - gridLayout.padding.right;
        float height = parentRect.rect.height - gridLayout.padding.top - gridLayout.padding.bottom;

        // �� ũ�� ��� (���� ���)
        float cellWidth = (width - (gridLayout.spacing.x * (columns - 1))) / columns;
        float cellHeight = (height - (gridLayout.spacing.y * (rows - 1))) / rows;

        gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
    }
}
