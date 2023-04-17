using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowItem : MonoBehaviour
{
    [SerializeField] private bool needAnim = true;
    public Animator animator;
    [SerializeField] private GameObject root;
    public bool isOpen;
    private Action OnBeginOpen;
    private Action OnEndOpen;
    private Action OnBeginClose;
    private Action OnEndClose;

    private void Awake()
    {
        if (needAnim)
            animator = GetComponent<Animator>();
        if (root == null)
            root = transform.GetChild(0).gameObject;
    }

    public void Close(Action OnBeginClose = null, Action OnEndClose = null)
    {
        this.OnBeginClose = OnBeginClose;
        this.OnEndClose = OnEndClose;
        isOpen = false;
        // Debug.Log("close");
        if (needAnim)
            animator.SetBool("isOpen", false);
        else
            root.SetActive(false);
    }

    public void Open(Action OnBeginOpen = null, Action OnEndOpen = null)
    {
        this.OnBeginOpen = OnBeginOpen;
        this.OnEndOpen = OnEndOpen;
        isOpen = true;
        // Debug.Log("open");
        if (needAnim)
            animator.SetBool("isOpen", true);
        else 
            root.SetActive(true);
    }

    public void BeginOpen()
    {
        OnBeginOpen?.Invoke();
        OnBeginOpen = null;
    }

    public void Endpen()
    {
        OnEndOpen?.Invoke();
        OnEndOpen = null;
    }

    public void BeginClose()
    {
        OnBeginClose?.Invoke();
        OnBeginClose = null;
    }

    public void EndClose()
    {
        OnEndClose?.Invoke();
        OnEndClose = null;
    }
}