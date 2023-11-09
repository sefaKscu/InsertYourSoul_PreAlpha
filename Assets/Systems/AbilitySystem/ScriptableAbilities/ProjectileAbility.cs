using System.Threading.Tasks;
using UnityEngine;

namespace InsertYourSoul.AbilitySystem
{
    [CreateAssetMenu(menuName = "Insert Your Soul/Ability System/Ability/Projectile Ability", fileName = "new_ProjectileAbility")]
    public class ProjectileAbility : Ability
    {

        public override void Activate(AbilityReferences _references)
        {
            base.Activate(_references);
            CastSpell(_references, (int)(activeSeconds*1000f));
        }

        public async void CastSpell(AbilityReferences _references, int _castTimeMiliSeconds)
        {
            await Task.Delay(_castTimeMiliSeconds);
            // Instantiate Projectile
            ProjectileScriptPrototype p = Instantiate(AbilityPrefab, _references.ProjectileExit.position, _references.ProjectileExit.rotation).GetComponent<ProjectileScriptPrototype>();
            p.Initialize(_references);
        }
    }
}
