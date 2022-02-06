using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;

    bool _pressed = false;
    public void OnUpdate()
    {
        //if (EventSystem.current.IsPointerOverGameObject()) //UI가 클릭되었는지 체크
          //  return;

        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();

        if (Input.GetKeyUp(KeyCode.K))
            KeyAction.Invoke();

        if (MouseAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                //Define.MouseEvent.Press가 event로 OnMouseClicked의 인자로 전달 된다.
                MouseAction.Invoke(Define.MouseEvent.Press); // 눌렸다 (MouseAction에 등록된 OnMouseClicked에 인자가 전달된다.)
                _pressed = true;
            }
            else
            {
                if (_pressed)
                    MouseAction.Invoke(Define.MouseEvent.Click); // 눌렀다 떼짐
                _pressed = false;
            }
        }
    }
}
