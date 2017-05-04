using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    [SerializeField]
    private string _name;
    [SerializeField]
    private Mesh _model;
    [SerializeField]
    private WeaponType _weaponType;
    [SerializeField]
    private WeaponAOEType _weaponAOEType;

    public string Name {
        get { return _name; }
    }
    public Mesh Model {
        get { return _model; }
    }
    public WeaponType WeaponType {
        get { return _weaponType; }
    }
    public WeaponAOEType WeaponAOEType {
        get { return _weaponAOEType; }
    }
}
