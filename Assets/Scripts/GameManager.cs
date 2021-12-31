using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum Anim {
    Idle,
    Kite,
    Jog,
    Dance,
    Jump,
    Hanetsuki,
    Sitting,
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    Text camPosText;
    [SerializeField]
    Text originText;

    [SerializeField]
    Transform origin;
    [SerializeField]
    Anim[] anims;

    [SerializeField]
    int index = 0;

    [SerializeField]
    GameObject kiteObj;
    [SerializeField]
    GameObject wing;

    [SerializeField]
    GameObject hagoita;
    [SerializeField]
    GameObject kotatsu;

    [SerializeField]
    GameObject onpu;

    Anim currentAnim;
    float elapsedTime = 0f;

    [SerializeField]
    float duration = 1000;

    void Awake() {
        HideAllObj();
        currentAnim = anims[index];
        animator.Play(currentAnim.ToString());
        ShowAnim();
    }

    void HideAllObj() {
        kiteObj.SetActive(false);
        wing.SetActive(false);
        hagoita.SetActive(false);
        kotatsu.SetActive(false);
        onpu.SetActive(false);
    }

    void ShowAnim() {
        HideAllObj();
        switch(currentAnim) {
            case Anim.Kite:
                kiteObj.SetActive(true);
                break;
            case Anim.Hanetsuki:
                wing.SetActive(true);
                hagoita.SetActive(true);
                break;
            case Anim.Sitting:
                kotatsu.SetActive(true);
                onpu.SetActive(true);
                break;
        }
    }

    void Update() {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > duration) {
            elapsedTime = 0;
            Next();
        }
        var camPos = Camera.main.transform.position;
        var camScale =Camera.main.transform.localScale;
        var originPos = origin.position;
        var originScale = origin.localScale;
        camPosText.text = "camPos:" + camPos.x.ToString("F1") + "," + camPos.y.ToString("F1") + "," + camPos.z.ToString("F1") + ",scale:" + camScale.x;;
        originText.text = "originPos:" + originPos.x.ToString("F1") + "," + originPos.y.ToString("F1") + "," + originPos.z.ToString("F1") + ",scale:" + originScale.x;
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
        if (anims.Length -1 <= index ) {
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
