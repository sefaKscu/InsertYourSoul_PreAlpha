using InsertYourSoul.AbilitySystem;
using System.Threading.Tasks;
using UnityEngine;

namespace InsertYourSoul
{
    [CreateAssetMenu(menuName = "Insert Your Soul/Ability System/Ability/Strike Ability", fileName = "new_StrikeAbility")]
    public class StrikeAbility : Ability
    {
        public override void Activate(AbilityReferences _references)
        {
            base.Activate(_references);
            CastSpell(_references, (int)(activeSeconds * 1000f));
        }

        public async void CastSpell(AbilityReferences _references, int _castTimeMiliSeconds)
        {
            await Task.Delay(_castTimeMiliSeconds);
            // Instantiate Projectile
            LightningStrikePrototype l = Instantiate(AbilityPrefab, _references.AimData.TargetPosition, _references.ProjectileExit.rotation).GetComponent<LightningStrikePrototype>();
            l.Initialize(_references);
        }
    }
}
