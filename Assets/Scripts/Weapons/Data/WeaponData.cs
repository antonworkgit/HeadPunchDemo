using Scripts.Interactive.Painting;
using UnityEngine;

namespace Scripts.Weapons.Data
{
    [CreateAssetMenu(menuName = "Weapons/WeaponData", fileName = "NewWeaponData")]
    public class WeaponData : ScriptableObject
    {
        public float Damage;
        public float AttackRange;
        public float Cooldown;
        public Vector2 KnockbackRange;
        public PaintParams PaintParams;

        //public Vector2 AttackAngleRange;
        //public List<Texture2D> DecalBrushes;
        //public Texture2D GetRandomDecal() => DecalBrushes.RandomElement();
    }
}