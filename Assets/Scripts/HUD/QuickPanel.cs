using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuickPanel : MonoBehaviour
{
    public Transform quickPanelSelect;
    public Transform[] quickPanels;
    private int selectIndex = 0;

    private PlayerInput playerInput;
    private InputAction selectPanelAction;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        selectPanelAction = playerInput.actions["SelectPanel"]; // �׼� �̸� Ȯ��!

        // �̺�Ʈ ������ ���
        selectPanelAction.performed += OnSelectPanel;
    }

    private void OnDestroy()
    {
        // �̺�Ʈ ������ ���� (�޸� ���� ����)
        selectPanelAction.performed -= OnSelectPanel;
    }

    private void OnSelectPanel(InputAction.CallbackContext context)
    {
        string keyPressed = context.control.name; // �Էµ� Ű �̸� ��������

        if (int.TryParse(keyPressed, out int numberKey)) // ���� �Է����� Ȯ��
        {
            int index = numberKey - 1; // ����Ű (1~9)�� �迭 �ε���(0~8)�� ��ȯ
            SelectPanel(index);
        }
    }

    private void SelectPanel(int index)
    {
        if (index < 0 || index >= quickPanels.Length) return;

        selectIndex = index;
        Transform selectedPanel = quickPanels[selectIndex];

        quickPanelSelect.position = selectedPanel.position;
    }
}
