using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Any object with this script will be set as a child of the current character when the character gets swapped.
public class SetAsChildOfCurrentCharacter : MonoBehaviour
{
    private void OnEnable()
    {
        SwapCharacters._finsihedCharacterSwap += SetAsChild;
        SwapCharacters._startedCharacterSwap += RemoveParent;
    }

    private void OnDisable()
    {
        SwapCharacters._finsihedCharacterSwap -= SetAsChild;
        SwapCharacters._startedCharacterSwap -= RemoveParent;
    }

    private void Start()
    {
        SetAsChild();
    }

    public void SetAsChild()
    {
        gameObject.transform.SetParent(SwapCharacters.currentCharacter.transform);

        /*if (this != null)   //If this script isn't null, set this object as a child of the current character.
        {
            gameObject.transform.SetParent(SwapCharacters.currentCharacter.transform);
        }
        else               //Otherwise unsubscribe this.
        {
            SwapCharacters._characterSwapped -= SetAsChild;
        }*/
    }

    public void RemoveParent()
    {
        gameObject.transform.SetParent(null);
    }
}
