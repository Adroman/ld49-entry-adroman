using System;
using System.Collections.Generic;
using UnityEngine;

public class Presentation : MonoBehaviour
{
    public GameObject[] ObjectsToPresent;
    public MenuManager Menu;
    
    private readonly Queue<GameObject> _queue = new Queue<GameObject>();
    private GameObject _currentUi;
    
    public void OnEnable()
    {
        _queue.Clear();
        foreach(var ui in ObjectsToPresent) _queue.Enqueue(ui);
        
        NextUi();
    }


    public void Update()
    {
        if (Input.anyKeyDown)
        {
            NextUi();
        }
    }

    private void NextUi()
    {
        if (_currentUi != null)
        {
            // hide previous
            _currentUi.SetActive(false);
        }

        if (_queue.Count > 0)
        {
            // show next
            _currentUi = _queue.Dequeue();
            _currentUi.SetActive(true);
        }
        else
        {
            // no next - disable self
            Menu.GoToTitle();
            gameObject.SetActive(false);
        }
    }
}