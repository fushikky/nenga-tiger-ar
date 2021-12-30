using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Anim {
    Idle,
    Kite,
    Jog,
    Dance,
    Jump,
    Hanetsuki,
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    Anim[] anims;

    [SerializeField]
    int index = 0;

    [SerializeField]
    GameObject kiteObj;
    Anim currentAnim;

    void Awake() {
        HideAllObj();
        currentAnim = anims[index];
    }

    void HideAllObj() {
        kiteObj.SetActive(false);
    }
    void ShowAnim() {
        HideAllObj();
        switch(currentAnim) {
            case Anim.Kite:
                kiteObj.SetActive(true);
                break;
        }
    }

   public void Next() {
        NextIndex();
        animator.Play(anims[index].ToString());
        ShowAnim();
    }

    public void Prev() {
        PreavIndex();
        animator.Play(anims[index].ToString());
        ShowAnim();
    }

    void NextIndex() {
        if (anims.Length -1 < index ) {
            index = 0;
        } else {
            index++;
        }
        currentAnim = anims[index];
    }

    void PreavIndex() {
        if ( index < 1) {
            index = anims.Length;
        } else {
            index--;
        }
        currentAnim = anims[index];
    }
}
