using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RagdollOnOffSwitch : MonoBehaviour
{
    [SerializeField] private GameObject _mainOBJ;
    private Collider _mainCol;
    private Rigidbody _mainRb;
    private Rigidbody[] _rigibodyList;
    private Collider[] _colList;
    private Animator _animator;
    public UnityEvent OnRagdoll;
    public void Start()
    {
        _mainCol = gameObject.GetComponent<Collider>();
        _mainRb = gameObject.GetComponent<Rigidbody>();
        _animator = gameObject.GetComponent<Animator>();
        GetRagdollParts();
        RagdollOff();
    }
    public void GetRagdollParts()
    {
        _rigibodyList = _mainOBJ.GetComponentsInChildren<Rigidbody>();
        _colList= _mainOBJ.GetComponentsInChildren<Collider>();

    }
    public void RagdollOn()
    {
        _mainCol.enabled = false;
        _mainRb.isKinematic = true;
        _animator.enabled = false;
        foreach (Rigidbody rb in _rigibodyList)
        {
            rb.isKinematic = false;
        }
        foreach (Collider col in _colList)
        {
            col.enabled = true;
        }

        
    }
    public void RagdollOff()
    {
        _mainCol.enabled = true;
        _mainRb.isKinematic = false;
        _animator.enabled = true;
        foreach (Rigidbody rb in _rigibodyList)
        {
            rb.isKinematic = true;
        }
        foreach (Collider col in _colList)
        {
            col.enabled = false;
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
     if(collision.gameObject.CompareTag("Projectile"))
        {
            RagdollOn();
        }
    }
}
