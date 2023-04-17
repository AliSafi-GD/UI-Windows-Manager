using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class UIWindowsManager : MonoBehaviour
{
    [SerializeField] private AnimationCurve asfd;
     public List<WindowItem> windows;
    public static UIWindowsManager instance;
    public Stack<WindowItem> stackPopups = new Stack<WindowItem>();
    
    public void ClosePopupUntil(string windowName)
    {
        var popup = stackPopups.FirstOrDefault(x => x.name == windowName);
        if (popup != null)
        {
            Close(null, () =>
            {
                ClosePopupUntil(windowName);
            });
        }
    }
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }
    
    #region Open Methods Without Action
    
    public void Open(WindowItem window)
    {
        Debug.Log("Open "+window.name);
        window.transform.SetAsLastSibling();
        window.Open();
        stackPopups.Push(window);
    }
    public void Open(int windowIndex)
    {
        Debug.Log("Open " +windowIndex);
        windows[windowIndex].transform.SetAsLastSibling();
        windows[windowIndex].Open();
        stackPopups.Push(windows[windowIndex]);
    }
    public T Open<T>(string windowName)
    {
        var window = windows.Find(x => x.name == windowName);
        window.transform.SetAsLastSibling();
        window.Open();
        stackPopups.Push(window);
        return window.GetComponent<T>();
    }
    public WindowItem Open(string windowName)
    {
        var window = windows.Find(x => x.name == windowName);
        window.transform.SetAsLastSibling();
        window.Open();
        stackPopups.Push(window);
        return window;
    }
    
    #endregion
    
    
    #region Open method with action
    
    public void Open(WindowItem window,Action OnBegin,Action OnEnd)
    {
        window.transform.SetAsLastSibling();
        window.Open(OnBegin,OnEnd);
        stackPopups.Push(window);
    }
    public void Open(int windowIndex,Action OnBegin,Action OnEnd)
    {
        windows[windowIndex].transform.SetAsLastSibling();
        windows[windowIndex].Open(OnBegin,OnEnd);
        stackPopups.Push(windows[windowIndex]);
    }
    public T Open<T>(string windowName,Action OnBegin,Action OnEnd)
    {
        var popup = windows.Find(x => x.name == windowName);
        popup.transform.SetAsLastSibling();
        popup.Open(OnBegin,OnEnd);
        stackPopups.Push(popup);
        return popup.GetComponent<T>();
    }
    public WindowItem Open(string windowName,Action OnBegin,Action OnEnd)
    {
        var popup = windows.Find(x => x.name == windowName);
        popup.transform.SetAsLastSibling();
        popup.Open(OnBegin,OnEnd);
        stackPopups.Push(popup);
        return popup;
    }
    
    #endregion
    
    public void Close(Action OnBegin,Action OnEnd)
    {
        if(stackPopups.Count <= 0)
        {
            Open("ExitGame");
        }
        else
        {
            var p = stackPopups.Peek();
            p.Close(OnBegin,OnEnd);
            stackPopups.Pop();
        }
        
    }
    
    public bool HasExistPopup(string popupName)=>windows.Exists(x=>x.name == popupName); 
    
    public void Close()
    {
        if(stackPopups.Count <= 0)
        {
            if (HasExistPopup("ExitGame"))
                Open("ExitGame");
        }
        else
        {
            var p = stackPopups.Peek();
            p.Close();

            var p2 = stackPopups.Pop();
            if (p2.GetComponent<IPopupCloseable>() != null)
            {
                var iClose = p2.GetComponent<IPopupCloseable>();
                iClose.OnBeginClose();
                iClose.OnEndClose();
            }
                
        }
        
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
    
    public void CloseAllPage()
    {
        Debug.Log("stack Count = "+stackPopups.Count);
        for (int i = 0; i < stackPopups.Count;)
        {
            Debug.Log(stackPopups.Peek().name);
            Close();
        }
    }
}

interface IPopupCloseable
{
    void OnBeginClose();
    void OnEndClose();
}
