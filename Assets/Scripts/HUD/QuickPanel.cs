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
        selectPanelAction = playerInput.actions["SelectPanel"]; // 액션 이름 확인!

        // 이벤트 리스너 등록
        selectPanelAction.performed += OnSelectPanel;
    }

    private void OnDestroy()
    {
        // 이벤트 리스너 해제 (메모리 누수 방지)
        selectPanelAction.performed -= OnSelectPanel;
    }

    private void OnSelectPanel(InputAction.CallbackContext context)
    {
        string keyPressed = context.control.name; // 입력된 키 이름 가져오기

        if (int.TryParse(keyPressed, out int numberKey)) // 숫자 입력인지 확인
        {
            int index = numberKey - 1; // 숫자키 (1~9)를 배열 인덱스(0~8)로 변환
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
