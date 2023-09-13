using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] private TMP_Text _firstName;
    [SerializeField] private TMP_Text _lastName;
    [SerializeField] private TMP_Text _role;

    public void ChangeFirstName(string name)
    {
        _firstName.text = "";
        _firstName.text = name;
    }

    public void ChangeLastName(string name)
    {
        _lastName.text = "";
        _lastName.text = name;
    }

    public void ChangeRole(string role)
    {
        _role.text = "";
        _role.text = role;
    }

    public void StartPlayTestCredits()
    {

    }
}
