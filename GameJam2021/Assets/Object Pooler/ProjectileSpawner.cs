using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    #region Singleton
    public static ProjectileSpawner Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    ObjectPooler objPooler;

    void Start()
    {
        objPooler = ObjectPooler.Instance;
    }


    public void SpawnProjectile(string tag, Vector2 pos, Quaternion rot, Vector2 dir, float force = 0.5f)
    {
        ParticleInfo info = new ParticleInfo
        {
            tag = tag,
            pos = pos,
            rot = rot,
            dir = dir,
            force = force
        };
        GameObject projectile = objPooler.SpawnFromPool<Projectile>(info);

        //spawn particle effects?
    }
}
