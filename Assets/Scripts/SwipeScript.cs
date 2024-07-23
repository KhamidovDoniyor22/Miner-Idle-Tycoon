using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeScript : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    
    public void SwipeButton(float index)
    {
        _camera.position = new Vector3(0, -9, -10);       
    }
    public void UpButton()
    {
        _camera.position = new Vector3(0, 0.65f, -10);
    }
}
