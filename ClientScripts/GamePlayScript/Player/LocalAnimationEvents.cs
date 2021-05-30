using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalAnimationEvents : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private AxeControl axeControl;
    [SerializeField]
    private PickaxeControl pickaxeControl;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void AxeHit()
    {
        axeControl.isAxeAttack = true;
        Invoke("CloseAxeAttack", 1);
    }
    private void CloseAxeAttack()
    {
        axeControl.isAxeAttack = false;
    }
    public void PickaxeHit()
    {
        pickaxeControl.isPickaxeAttack = true;
        Invoke("ClosePickaxeAttack", 1);
    }
    private void ClosePickaxeAttack()
    {
        pickaxeControl.isPickaxeAttack = false;
    }
}
