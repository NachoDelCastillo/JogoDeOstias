using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCard : MonoBehaviour
{
    public CharacterData characterData;

    [SerializeField] Transform gfx;

    int pointersInsideThis = 0;

    // Devuelve true si la carta personaje esta ya grande
    bool big;

    private void Awake()
    {

    }

    public void PointerEnter()
    {
        // Si estaba sin ningun pointer y entra uno, hacerlo grande
        if (pointersInsideThis == 0)
            GoBig();

        pointersInsideThis++;
    }

    public void PointerExit()
    {
        pointersInsideThis--;

        // Si ya no hay pointers dentro de esto, hacerlo pequeño
        if (pointersInsideThis == 0)
            GoSmall();
    }

    void GoBig()
    {
        big = true;
        gfx.transform.DOKill();
        gfx.transform.DOScale(1.2f, 1).SetEase(Ease.OutElastic);
    }

    void GoSmall()
    {   
        big = false;
        gfx.transform.DOKill();
        gfx.transform.DOScale(1, 1).SetEase(Ease.OutElastic);
    }
}
