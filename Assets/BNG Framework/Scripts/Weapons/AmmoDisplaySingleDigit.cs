using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BNG {
    public class AmmoDisplaySingleDigit : MonoBehaviour {

        public RaycastWeapon Weapon;
        public Text AmmoLabel;
        public GameObject indicatorLight;
        public Material thereIsBullets;
        public Material noBullets;

        private MeshRenderer meshRenderer;

        private void Start()
        {
            meshRenderer = indicatorLight.GetComponent<MeshRenderer>();

        }

        private void Update()
        {
            ChangeIndicatorMaterial();
        }

        void OnGUI() {
            
            AmmoLabel.text = Weapon.GetBulletCount().ToString();
        }
        
        public void ChangeIndicatorMaterial()
        {
            if (Weapon.GetBulletCount() > 0)
            {
                meshRenderer.material = thereIsBullets;
            }
            else
            {
                meshRenderer.material = noBullets;
            }
        }
    }
}

