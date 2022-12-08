using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace AI
{
    [ExecuteAlways]
    public class AIShooterWeapon : MonoBehaviour
    {
      
        private float NextTimeToFire = 0f;
        private bool CanShoot = true;
        private bool Reloading = false;
        private float CurrentAmmoInMagazine = 0f;
        
        [HideInInspector] public AISystem AISystem;

        [Group("Open ")] public weaponSettings WeaponSettings;

            [Serializable]
            public class weaponSettings
            {
                [HideInInspector] public bool Single = false;
                [Header("WEAPON SETTINGS")]
                [Space]
                [Help("Weapon Settings Part Allows You To Control The Weapon's Settings", HelpBoxMessageType.BigInfo)]
                [Space]

                public AIEnums.YesNo AlwaysEquipped = AIEnums.YesNo.No;
                [Space] [Space] [Help("Is The Weapon Is Always Equipped ?", HelpBoxMessageType.Info)]

                public AIEnums.WeaponType WeaponType = AIEnums.WeaponType.AdditiveShot;
                [Space] [Space] [Help("The Weapon's Type (Sniper Rifle Or Minigun etc..)", HelpBoxMessageType.Info)]
                
                public AIEnums.FireType FireType;
                [Space] [Space] [Help("The Weapon's Shooting Type.", HelpBoxMessageType.Info)]
                
                public float WeaponAttackDistance = 10f;
                [Space] [Space] [Help("The Attack Distance Of The AI For Start Attacking To The Target.", HelpBoxMessageType.Info)]

                [Condition("Single", false, 0f)]
                public float FireRate = 8f;
                
                [Condition("Single", true, 0f)]
                public float FireDelay = 1.5f;
                [Space] [Space] [Help("The Weapons Fire Rate / Shoot Delay Speed.", HelpBoxMessageType.Info)]

                public int WeaponAccuracy = 100;
                [Space] [Space] [Help("The Weapon's Accuracy Amount.", HelpBoxMessageType.Info)]
                
                [Header("PROJECTILE SETTINGS")]
                [Space]
                
                public GameObject ProjectileObject;
                [Space] [Space] [Help("The Weapon's Bullet Object.", HelpBoxMessageType.Info)]
                
                public float ProjectileForce = 20f;
                [Space] [Space] [Help("The Bullet Projectile's Shoot Force.", HelpBoxMessageType.Info)]
                
                public LayerMask ProjectileHitLayers = 1;
                [Space] [Space] [Help("Which Layers Can The Projectile Hit ?", HelpBoxMessageType.Info)]
                
                
                [Header("RAYCAST SETTINGS")]
                [Space]

                // public float MaxRaycastDistance = 10f;
                // [Space] [Space] [Help("The Weapon's Max Hit Distance.", HelpBoxMessageType.Info)]
                
                public AIEnums.YesNo DrawRaycastDebug = AIEnums.YesNo.No;
                [Space] [Space] [Help("Draw Raycast Line Gizmos ?", HelpBoxMessageType.Info)]
                
                public LayerMask RaycastHitLayers = 1;
                [Space] [Space] [Help("Which Layers Can The Raycast Hit ?", HelpBoxMessageType.Info)]

                [Header("SIMULATED SETTINGS")]
                [Space]
                
                public int DistanceAccuracy = 99;
                [Space] [Space] [Help("The Weapon's Accuracy Modifier According To The Target Distance.", HelpBoxMessageType.Info)]
                
                public AIEnums.YesNo DebugSimulatedAccuracy = AIEnums.YesNo.No;
                [Space] [Space] [Help("Debug The Simulated Weapon Accuracy ?", HelpBoxMessageType.Info)]

                public string WeaponName = "Gun";
            }

            [Space]
            
            [Group("Open ")] public ammoSettings AmmoSettings;

            [Serializable]
            public class ammoSettings
            {
                [Header("AMMO SETTINGS")]
                [Space]
                [Help("Ammo Settings Part Allows You To Control The Weapon Ammo's Settings", HelpBoxMessageType.BigInfo)]
                [Space]
                

                public int MinDamageAmount = 5;
                [Space] [Space] [Help("The Weapons Minimum Damage.", HelpBoxMessageType.Info)]
                
                public int MaxDamageAmount = 9;
                [Space] [Space] [Help("The Weapons Maximum Damage.", HelpBoxMessageType.Info)]
                
                public float MagazineAmmo = 30f;
                [Space] [Space] [Help("The Weapons One Magazine's Ammo Storage.", HelpBoxMessageType.Info)]
                
                public float StorageAmmo = 90f;
                [Space] [Space] [Help("The Ammo Amount Carried On The Shooter AI.", HelpBoxMessageType.Info)]

                public string AmmoTypeName = "762x39mm";
            }

            [Space]
            
            [Group("Open ")] public vfxSettings VFXSettings;

            [Serializable]
            public class vfxSettings
            {
                [Space]
                [Group("Open ")] public bulletSettings BulletSettings;

                [Serializable]
                public class bulletSettings
                {
                    [Header("BULLET SETTINGS")]
                    [Space]
                    [Help("Bullet Settings Part Allows You To Control The Weapon's Bullet VFX Settings",
                        HelpBoxMessageType.BigInfo)]
                    [Space]

                    public List<AIMeleeWeapon.impactList> ImpactEffects = new List<AIMeleeWeapon.impactList>();
                    [Space] [Space] [Help("The Impact Particle Effects To Be Played.", HelpBoxMessageType.Info)] 

                    public List<ParticleSystem> MuzzleFlashes = new List<ParticleSystem>();
                    
                }
                
                [Space]
                
                [Group("Open ")] public soundSettings SoundSettings;

                [Serializable]
                public class soundSettings
                {
                    [Header("SOUND SETTINGS")]
                    [Space]
                    [Help("Sound Settings Part Allows You To Control The Weapon's Sound Settings", HelpBoxMessageType.BigInfo)]
                    [Space]

                    public AudioClip FireSoundEffect = null;
                    [Range(0f,1f)]
                    public float FireSoundVolume = 1f;
                    [Space] [Space] [Help("The Fire Sound Effect Audio.", HelpBoxMessageType.Info)]
                    
                    public AudioClip ReloadSoundEffect = null;
                    [Range(0f,1f)]
                    public float ReloadSoundVolume = 1f;
                    [Space] [Space] [Help("The Reload Sound Effect Audio.", HelpBoxMessageType.Info)]
                    
                    public AudioClip EquipSoundEffect = null;
                    [Range(0f,1f)]
                    public float EquipSoundVolume = 1f;
                    [Space] [Space] [Help("The Equip Sound Effect Audio.", HelpBoxMessageType.Info)]
                    
                    public AudioClip UnEquipSoundEffect = null;
                    [Range(0f,1f)]
                    public float UnEquipSoundVolume = 1f;
                    [Space] [Space] [Help("The Un Equip Sound Effect Audio.", HelpBoxMessageType.Info)]
                    
                    [AI.ReadOnly] public AudioSource WeaponAudioSource;
                }
            }
            
            [Space]
            
            [Group("Open ")] public weaponObjectSettings WeaponObjectSettings;

            [Serializable]
            public class weaponObjectSettings
            {
                [Header("WEAPON OBJECT SETTINGS")]
                [Space]
                [Help("This part is for making your Weapon Object Enabled When You Equip A Weapon, You Can Ignore This Part If Your Weapon Is Equipped Always", HelpBoxMessageType.BigInfo)]
                [Space]

                public List<Renderer> HolsteredWeaponObject;
                [Space] [Space] [Help("The Weapon Renderer(s) That Will Be Enabled When The Weapon Is Holstered.", HelpBoxMessageType.Info)]
                
                public List<Renderer> MainWeaponObject;
                [Space] [Space] [Help("The Weapon Renderer(s) That Will Be Enabled When The Weapon Is Equipped.", HelpBoxMessageType.Info)]
                
     
                [AI.ReadOnly]
                public Transform Muzzle = null;    
            }
            
            [Space]

            [Group("Open ")] public ikSettings IKSettings;

            [Serializable]
            public class ikSettings
            {
                [Header("IK SETTINGS")]
                [Space]
                [Help("IK Settings Part Allows You To Control The Weapon's IK Settings", HelpBoxMessageType.BigInfo)]
                [Space]

                [AI.ReadOnly]
                public Transform LeftHandIK = null;    
                
                [Space]
                
                [AI.ReadOnly]
                public Transform RightHandIK = null;

            }
            
            [Space]
            
            [Group("Open ")] public weaponEvents WeaponEvents;

            [Serializable]
            public class weaponEvents
            {
                [Header("WEAPON EVENTS")]
                [Space]
                [Help("Weapon Events Part Allows You To Control The Weapon's Events", HelpBoxMessageType.BigInfo)]
                [Space]

                public UnityEvent OnFire = new UnityEvent();
                [Space] [Space] [Help("The Firing Event.", HelpBoxMessageType.Info)]

                public UnityEvent OnReload = new UnityEvent();
                [Space] [Space] [Help("The Reloading Event.", HelpBoxMessageType.Info)]
                
                public UnityEvent OnReloadEnd = new UnityEvent();
                [Space] [Space] [Help("The Reload End Event.", HelpBoxMessageType.Info)]

                public UnityEvent OnEquip = new UnityEvent();
                [Space] [Space] [Help("The Weapon Equip Event.", HelpBoxMessageType.Info)]
                
                public UnityEvent OnUnEquip = new UnityEvent();
                [Space] [Space] [Help("The Weapon Un Equip Event.", HelpBoxMessageType.Info)]

                public AIEnums.YesNo DisableEvents = AIEnums.YesNo.No;
            }


            private bool NotEquipped = false;
            
        public void OnDeath()
        {
            transform.SetParent(null);

            if(GetComponent<AudioSource>() != null)
                GetComponent<AudioSource>().enabled = false;
            
            if (GetComponent<Rigidbody>() == null)
            {
                gameObject.AddComponent<Rigidbody>().isKinematic = false;
            }
            else
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
            }
            if (GetComponent<Collider>() == null)
            {
                if (gameObject.GetComponent<MeshFilter>() != null)
                {
                    gameObject.AddComponent<MeshCollider>().convex = true;

                    if (gameObject.GetComponent<MeshCollider>().sharedMesh == null)
                    {
                        gameObject.GetComponent<MeshCollider>().sharedMesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
                    }
                }
                else if (gameObject.GetComponent<SkinnedMeshRenderer>() != null)
                {
                    gameObject.AddComponent<MeshCollider>().convex = true;

                    if (gameObject.GetComponent<MeshCollider>().sharedMesh == null)
                    {
                        gameObject.GetComponent<MeshCollider>().sharedMesh =
                            gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
                    }
                }
                else
                    gameObject.AddComponent<BoxCollider>();
            }
            else if(GetComponent<Collider>() != null)
            {
                GetComponent<Collider>().enabled = true;
            }
            if(AISystem.General.DestroyAIOnDeath == AIEnums.YesNo.Yes)
                Destroy(gameObject, 1.3f + AISystem.General.DestroyDelay);
        }

        public void OnValidate()
        {
            if(Application.isPlaying)
                return;

            if (WeaponSettings == null)
                WeaponSettings = new weaponSettings();
            
            if(AISystem != null &&  AISystem.Settings != null && AISystem.Settings.Attack != null && WeaponSettings != null)
                AISystem.Settings.Attack.AttackDistance = WeaponSettings.WeaponAttackDistance;
        }
        
        public void LateUpdate()
        {
        
            if(Application.isPlaying)
                return;
            
            if (AISystem == null)
            {
                Transform parentt = transform.parent;

                while (parentt.GetComponent<AISystem>() == null)
                {
                    parentt = parentt.parent;
                }

                AISystem = parentt.GetComponent<AISystem>();
            }
            
            AISystem.IsWeapon = true;
            AISystem.IsShooter = true;
            WeaponSettings.Single = WeaponSettings.WeaponType == AIEnums.WeaponType.SingleShot;
            AISystem.SingleShot = WeaponSettings.WeaponType == AIEnums.WeaponType.SingleShot;
            if(VFXSettings == null)
                VFXSettings = new vfxSettings();
            
            if(WeaponSettings == null)
                WeaponSettings = new weaponSettings();
            
            if(IKSettings == null)
                IKSettings = new ikSettings();

            
            if (WeaponSettings.ProjectileObject == null)
            {
                WeaponSettings.ProjectileObject = Resources.Load<GameObject>("Prefabs/Example Bullet");
            }
            
            if (AISystem.Anim == null)
            {
                AISystem.Anim = AISystem.GetComponent<Animator>();
            }
            
            if (VFXSettings.SoundSettings.WeaponAudioSource == null)
            {
                if (GetComponent<AudioSource>() != null)
                {
                    VFXSettings.SoundSettings.WeaponAudioSource = GetComponent<AudioSource>();
                }
                else
                {
                    VFXSettings.SoundSettings.WeaponAudioSource = gameObject.AddComponent<AudioSource>();
                }

                VFXSettings.SoundSettings.WeaponAudioSource.playOnAwake = false;
            }

            if (WeaponObjectSettings.Muzzle == null)
            {
                if(transform.Find("W Muzzle") != null)
                {
                    WeaponObjectSettings.Muzzle = transform.Find("W Muzzle");
                }
                else
                {
                    WeaponObjectSettings.Muzzle = new GameObject("W Muzzle").transform;
                    WeaponObjectSettings.Muzzle.transform.SetParent(transform);
                    WeaponObjectSettings.Muzzle.localPosition = Vector3.zero;
                    WeaponObjectSettings.Muzzle.localEulerAngles = Vector3.zero;
                }
            }
            
            if (IKSettings.RightHandIK == null || !IKSettings.RightHandIK.gameObject.activeInHierarchy)
            {
                if(transform.Find("Right Hand IK") != null)
                {
                    IKSettings.RightHandIK = transform.Find("Right Hand IK");
                }
                else
                {
                    IKSettings.RightHandIK = new GameObject("Right Hand IK").transform;
                    IKSettings.RightHandIK.transform.SetParent(AISystem.Anim.GetBoneTransform(HumanBodyBones.RightHand));
                    IKSettings.RightHandIK.localPosition = Vector3.zero;
                    IKSettings.RightHandIK.localEulerAngles = Vector3.zero;
                    IKSettings.RightHandIK.SetParent(transform);
                }
            }
               
            if (IKSettings.LeftHandIK == null|| !IKSettings.LeftHandIK.gameObject.activeInHierarchy)
            {
                if(IKSettings.RightHandIK.transform.Find("Left Hand IK") != null)
                {
                    IKSettings.LeftHandIK = IKSettings.RightHandIK.transform.Find("Left Hand IK");
                }
                else
                {
                    IKSettings.LeftHandIK = new GameObject("Left Hand IK").transform;
                    IKSettings.LeftHandIK.SetParent(IKSettings.RightHandIK);
                    IKSettings.LeftHandIK.localPosition = Vector3.zero;
                    IKSettings.LeftHandIK.localEulerAngles = Vector3.zero;
                }
            }
            
            if (WeaponSettings.WeaponAccuracy < 1)
            {
                WeaponSettings.WeaponAccuracy = 1;
            }
            if (WeaponSettings.WeaponAccuracy > 100)
            {
                WeaponSettings.WeaponAccuracy = 100;
            }

            if (WeaponObjectSettings.Muzzle != null)
            {
                if(WeaponObjectSettings.Muzzle.gameObject.GetComponent<MuzzleGizmos>() == null)
                    WeaponObjectSettings.Muzzle.gameObject.AddComponent<MuzzleGizmos>();
            }
        }

        private void OnWeaponStateChanged(bool enable, bool first = false)
        {
            NotEquipped = !enable;
            
            foreach (var renderer in WeaponObjectSettings.MainWeaponObject)
            {
                renderer.enabled = enable;
            }
            
            foreach (var renderer in WeaponObjectSettings.HolsteredWeaponObject)
            {
                renderer.enabled = !enable;
            }

            if (enable)
            {
                if (WeaponSettings.Single)
                {
                    SingleCanFire = false;
                    Invoke("EndSingleDelay", Random.Range(AISystem.Settings.Attack.MinAttackDelay, AISystem.Settings.Attack.MaxAttackDelay + 1f));
                }
            }
        }
        private void Equipped()
        {
            WeaponEvents.OnEquip.Invoke();
            PlaySound(WeaponAudioType.Equip);
        }
        
        private void UnEquipped()
        {
            WeaponEvents.OnUnEquip.Invoke();
            PlaySound(WeaponAudioType.UnEquip);
        }

        private void PlaySound(WeaponAudioType weaponAudioType)
        {
            if (weaponAudioType == WeaponAudioType.Fire && VFXSettings.SoundSettings.FireSoundEffect != null)
            {
                VFXSettings.SoundSettings.WeaponAudioSource.volume = VFXSettings.SoundSettings.FireSoundVolume;
                VFXSettings.SoundSettings.WeaponAudioSource.PlayOneShot(VFXSettings.SoundSettings.FireSoundEffect);
                AIManager.SoundDetection(AIEnums.SoundType.ShootSound, 20, AISystem.transform);
            }
            
            if (weaponAudioType == WeaponAudioType.Reload && VFXSettings.SoundSettings.ReloadSoundEffect != null)
            {
                VFXSettings.SoundSettings.WeaponAudioSource.volume = VFXSettings.SoundSettings.ReloadSoundVolume;
                VFXSettings.SoundSettings.WeaponAudioSource.PlayOneShot(VFXSettings.SoundSettings.ReloadSoundEffect);
            }
            
            if (weaponAudioType == WeaponAudioType.Equip && VFXSettings.SoundSettings.EquipSoundEffect != null)
            {
                VFXSettings.SoundSettings.WeaponAudioSource.volume = VFXSettings.SoundSettings.EquipSoundVolume;
                VFXSettings.SoundSettings.WeaponAudioSource.PlayOneShot(VFXSettings.SoundSettings.EquipSoundEffect);
            }
            
            if (weaponAudioType == WeaponAudioType.UnEquip && VFXSettings.SoundSettings.UnEquipSoundEffect != null)
            {
                VFXSettings.SoundSettings.WeaponAudioSource.volume = VFXSettings.SoundSettings.UnEquipSoundVolume;
                VFXSettings.SoundSettings.WeaponAudioSource.PlayOneShot(VFXSettings.SoundSettings.UnEquipSoundEffect);
            }
        }
        private void Update()
        {
            
            if(!Application.isPlaying)
                return;

            if(NotEquipped)
                return;
            
            if(AISystem == null || AISystem != null && AISystem.Debug.AICurrentStateDebug == AIEnums.AICurrentState.Death)
                return;
        
            if(AISystem.Target == null || AISystem.SoundSearching)
                return;
            
            if(AISystem.Frozen || AISystem.StopAIBehaviour)
                return;

            if (AISystem.Debug.AICurrentStateDebug == AIEnums.AICurrentState.BackingUp)
            {
                if (AISystem.TargetDistance <= AISystem.Settings.Movement.TooCloseDistance / 2.55f &&
                    AISystem.TargetDistance > 0.1f)
                {
                    
                }
                else
                {
                    return;
                }
            }
            
            if (CurrentAmmoInMagazine <= 0 && AmmoSettings.StorageAmmo <= 0)
            {
                AmmoSettings.StorageAmmo = 0;
                AISystem.DontGoCombat = true;
                AISystem.General.AIConfidence = AIEnums.AIConfidence.Coward;
                AISystem.StopAttack();
            }

            if (AISystem.Reloading && WeaponSettings.WeaponType == AIEnums.WeaponType.SingleShot)
                NextTimeToFire = Random.Range(2, 5);
            
      
            if (CanShoot && CurrentAmmoInMagazine > 0)
            {
                if (AISystem.InverseKinematics.UseInverseKinematics == AIEnums.YesNo.Yes &&
                    AISystem.InverseKinematics.UseAimIK == AIEnums.YesNo.Yes &&
                    AISystem.BodyWeight <= 0.35f)
                {
                    
                }
                else
                {
                    if (WeaponSettings.WeaponType == AIEnums.WeaponType.SingleShot && SingleCanFire)
                    {
                        if (Time.time >= NextTimeToFire)
                        {
                            NextTimeToFire = Time.time + 1f / WeaponSettings.FireRate;
                            AISystem.StartAttack();
                            Shoot();   
                        }
                    }
                    else
                    {
                        if (Time.time >= NextTimeToFire)
                        {
                            NextTimeToFire = Time.time + 1f / WeaponSettings.FireRate;
                            if(AISystem.CanFire)
                                Shoot(); 
                        }
                    }
                }
            }
            

            if (CurrentAmmoInMagazine <= 0 && !Reloading && AmmoSettings.StorageAmmo > 0)
            {
                Reloading = true;
                Reload();
            }
            else if(CurrentAmmoInMagazine <= 0 && !Reloading)
            {
                AmmoSettings.StorageAmmo = 0;
                AISystem.DontGoCombat = true;
                AISystem.General.AIConfidence = AIEnums.AIConfidence.Coward;
                AISystem.StopAttack();
                Reloading = false;
                AISystem.Anim.ResetTrigger("Reload");
                return;
            }
            
        }

        private void Reload()
        {
            if (AmmoSettings.StorageAmmo <= 0)
            {
                AmmoSettings.StorageAmmo = 0;
                AISystem.DontGoCombat = true;
                AISystem.General.AIConfidence = AIEnums.AIConfidence.Coward;
                AISystem.StopAttack();
                Reloading = false;
                AISystem.Anim.ResetTrigger("Reload");
                return;
            }
          
            WeaponEvents.OnReload.Invoke();
            PlaySound(WeaponAudioType.Reload);
            AISystem.Reload();
        }

       [HideInInspector] public bool SingleCanFire = false;
        public void ReloadEndStart()
        {
            Invoke("ReloadEnd", 0f);
        }

        public void ReloadEnd()
        {
            
            if (AmmoSettings.StorageAmmo <= 0)
            {
                AmmoSettings.StorageAmmo = 0;
                AISystem.DontGoCombat = true;
                AISystem.General.AIConfidence = AIEnums.AIConfidence.Coward;
                AISystem.StopAttack();
                Reloading = false;
                return;
            }
            
            if(AmmoSettings.StorageAmmo >= AmmoSettings.MagazineAmmo)
               CurrentAmmoInMagazine = AmmoSettings.MagazineAmmo;
            else
                CurrentAmmoInMagazine = AmmoSettings.StorageAmmo;
            
            AmmoSettings.StorageAmmo -= AmmoSettings.MagazineAmmo;

            if (AmmoSettings.StorageAmmo <= 0 && CurrentAmmoInMagazine <= 0)
            {
                AmmoSettings.StorageAmmo = 0;
                AISystem.DontGoCombat = true;
                AISystem.General.AIConfidence = AIEnums.AIConfidence.Coward;
                AISystem.StopAttack();
            }
            
            Reloading = false;
            WeaponEvents.OnReloadEnd.Invoke();
            
            if (WeaponSettings.Single)
            {
                Invoke("EndSingleDelay", WeaponSettings.FireDelay);
            }
        }

        public void EndSingleDelay()
        {
            SingleCanFire = true;
        }

        private void Shoot()
        {

            if(!AISystem.Anim.GetBool("Equipped") || !AISystem.Anim.GetBool("Attack"))
                return;

            if (WeaponSettings.FireType == AIEnums.FireType.Raycast &&
                AISystem.InverseKinematics.UseInverseKinematics == AIEnums.YesNo.Yes &&
                AISystem.InverseKinematics.UseAimIK == AIEnums.YesNo.Yes &&
                AISystem.TargetDistance >= 12)
            {
                ReturnRaycast = true;
                WeaponSettings.FireType = AIEnums.FireType.Simulated;
            }
            if(WeaponSettings.FireType == AIEnums.FireType.Simulated &&
                    AISystem.InverseKinematics.UseInverseKinematics == AIEnums.YesNo.Yes &&
                    AISystem.InverseKinematics.UseAimIK == AIEnums.YesNo.Yes && ReturnRaycast &&
                    AISystem.TargetDistance < 12)
            {
                ReturnRaycast = false;
                WeaponSettings.FireType = AIEnums.FireType.Raycast;
            }

            SingleCanFire = false;
            float temporaryaccuracy = (100 - WeaponSettings.WeaponAccuracy) / 50f;
            temporaryaccuracy *= 0.85f;
        
            if (WeaponSettings.FireType == AIEnums.FireType.Projectile)
            {
                Rigidbody ProjectileBullet = Instantiate(WeaponSettings.ProjectileObject,
                    WeaponObjectSettings.Muzzle.position,
                    Quaternion.LookRotation(WeaponObjectSettings.Muzzle.forward)).GetComponent<Rigidbody>();

                ProjectileBullet.velocity = transform.TransformDirection(new Vector3(Random.Range(-temporaryaccuracy, temporaryaccuracy),ProjectileBullet.transform.position.y,WeaponSettings.ProjectileForce));

                AIProjectile proj = ProjectileBullet.GetComponent<AIProjectile>();

                if (proj != null)
                {
                    proj.weapon = this;
                
                    proj.ProjectileHitLayers = WeaponSettings.ProjectileHitLayers;
                
                    proj.damage = Random.Range(AmmoSettings.MinDamageAmount,
                        AmmoSettings.MaxDamageAmount + 1);

                    proj.damager = AISystem.gameObject;   
                }
                else
                {
                    AIRocketProjectile rocketproj = ProjectileBullet.GetComponent<AIRocketProjectile>();
                    
                    rocketproj.weapon = this;

                    rocketproj.damage = Random.Range(AmmoSettings.MinDamageAmount,
                        AmmoSettings.MaxDamageAmount + 1);

                    rocketproj.damager = AISystem.gameObject;   
                }

            }
            else if(WeaponSettings.FireType == AIEnums.FireType.Raycast)
            {
                RaycastHit hit;

                if (WeaponSettings.DrawRaycastDebug == AIEnums.YesNo.Yes)
                {
                    Debug.DrawRay(WeaponObjectSettings.Muzzle.transform.position, WeaponObjectSettings.Muzzle.transform.forward * 20 + new Vector3(Random.Range(-temporaryaccuracy, temporaryaccuracy), 0, 0), Color.green, 5);
                }
                
                if (Physics.Raycast(WeaponObjectSettings.Muzzle.transform.position, WeaponObjectSettings.Muzzle.transform.forward + new Vector3(Random.Range(-temporaryaccuracy, temporaryaccuracy), 0 ,0), out hit,
                    AISystem.Settings.Attack.AttackDistance, WeaponSettings.RaycastHitLayers, QueryTriggerInteraction.Ignore))
                {

                    if(hit.transform == AISystem.transform)
                        return;
                    
                  
                    float damage = Random.Range(AmmoSettings.MinDamageAmount, AmmoSettings.MaxDamageAmount + 1);
                    if (hit.transform.GetComponent<AIPlayerReference>() != null)
                    {
                        hit.transform.GetComponent<AIPlayerReference>().TakeDamage(damage, AttackerType.AI, AISystem.gameObject);
                    }
        
                    if (hit.transform.GetComponent<AIDamageable>() != null && hit.transform.root.gameObject != AISystem.gameObject)
                    {
                        hit.transform.GetComponent<AIDamageable>().TakeDamage(damage, AttackerType.AI, AISystem.gameObject); 
                        AISystem.AIEvents.OnDealDamage.Invoke(damage);
                    }
                }
            }
            else
            {
                float TargetDistance = AISystem.TargetDistance;
                float baseHitChance = (( WeaponSettings.WeaponAttackDistance - TargetDistance)  / WeaponSettings.WeaponAttackDistance)  * 100;
                float situationModfier = (WeaponSettings.WeaponAccuracy + WeaponSettings.DistanceAccuracy) / 2f;
                float hitChance = (baseHitChance  + situationModfier) / 2f;
                
                


                if (WeaponSettings.DebugSimulatedAccuracy == AIEnums.YesNo.Yes)
                {
                    Debug.Log("[Simulated Weapon Accuracy]: " + hitChance);
                }

                if (Random.Range(0, 101) <= hitChance)
                {
                    float damage = Random.Range(AmmoSettings.MinDamageAmount, AmmoSettings.MaxDamageAmount + 1);
                    if (AISystem.CurrentAITarget != null)
                    {
                        AISystem.CurrentAITarget.TakeDamage(damage, AttackerType.AI, AISystem.gameObject);
                    }
        
                    if (AISystem.CurrentPlayerRef != null)
                    {
                        AISystem.CurrentPlayerRef.TakeDamage(damage, AttackerType.AI, AISystem.gameObject);
                    }
                }
            }
           

            foreach (var par in VFXSettings.BulletSettings.MuzzleFlashes)
            {
                par.Play();
            }
            WeaponEvents.OnFire.Invoke();
            PlaySound(WeaponAudioType.Fire);

            CurrentAmmoInMagazine--;
            
            if(WeaponSettings.WeaponType == AIEnums.WeaponType.SingleShot)
                Invoke("StopAttack", 0.2f);
            
            if (WeaponSettings.Single)
            {
                Invoke("EndSingleDelay", WeaponSettings.FireDelay);
            }
        }

        private bool ReturnRaycast = false;
        public void StopAttack()
        {
            AISystem.StopAttack();
        }
        private void Awake()
        {
            if(!Application.isPlaying)
                return;
            
            if (AISystem == null)
            {
                Transform parentt = transform.parent;

                while (parentt.GetComponent<AISystem>() == null)
                {
                    parentt = parentt.parent;
                }

                AISystem = parentt.GetComponent<AISystem>();
            }

            AISystem.muzzle = WeaponObjectSettings.Muzzle.gameObject;
            AISystem.IsWeapon = true;
            AISystem.IsShooter = true;
            if(AISystem.IsEquipping == false)
            {
                WeaponSettings.AlwaysEquipped = AIEnums.YesNo.Yes;
            }

            AISystem.AlwaysEquipped = WeaponSettings.AlwaysEquipped == AIEnums.YesNo.Yes;
        }

        private void OnApplicationQuit()
        {
            AISystem.IsWeapon = false;
            AISystem.IsShooter = false;
        }

        [HideInInspector] public bool StartWithError;
        [HideInInspector] public string Reasons;
        private bool CheckComponents()
        {
            //Check The AI

            bool Failed = false;
            string Reason = String.Empty;
        
        
            if (StartWithError)
            {
                Failed = true;
                Reason = Reasons;
            }
            else if(Reasons != String.Empty)
            {
                UnityEngine.Debug.LogError(Reasons);
            }

            if (Failed)
            {
                UnityEngine.Debug.LogError(Reason);
                gameObject.SetActive(false);
                return false;
            }

            return true;

        }
        
        public void Start()
        {

            if(!Application.isPlaying)
                return;
            
            if (!gameObject.activeInHierarchy)
                return;
            
            
            
            CurrentAmmoInMagazine = AmmoSettings.MagazineAmmo;
            if (WeaponObjectSettings.Muzzle == null)
            {
                if(transform.Find("W Muzzle") != null)
                {
                    WeaponObjectSettings.Muzzle = transform.Find("W Muzzle");
                }
                else
                {
                    WeaponObjectSettings.Muzzle = new GameObject("W Muzzle").transform;
                    WeaponObjectSettings.Muzzle.transform.SetParent(transform);
                }
            }
            
            if (IKSettings.RightHandIK == null)
            {
                if(transform.Find("Right Hand IK") != null)
                {
                    IKSettings.RightHandIK = transform.Find("Right Hand IK");
                }
                else
                {
                    IKSettings.RightHandIK = new GameObject("Right Hand IK").transform;
                    IKSettings.RightHandIK.transform.SetParent(transform);
                }
            }
            
            if (IKSettings.LeftHandIK == null)
            {
                if(transform.Find("Left Hand IK") != null)
                {
                    IKSettings.LeftHandIK = transform.Find("Left Hand IK");
                }
                else
                {
                    IKSettings.LeftHandIK = new GameObject("Left Hand IK").transform;
                    IKSettings.LeftHandIK.transform.SetParent(IKSettings.RightHandIK);
                }
            }
            

            AISystem.AimTransform = WeaponObjectSettings.Muzzle;
            AISystem.InverseKinematics.LeftHandIK = IKSettings.LeftHandIK;
            AISystem.InverseKinematics.RightHandIK = IKSettings.RightHandIK;
         

            if (WeaponSettings.AlwaysEquipped == AIEnums.YesNo.Yes)
            {
                if (AISystem.InverseKinematics.UseInverseKinematics == AIEnums.YesNo.Yes &&
                    AISystem.InverseKinematics.UseHandIK == AIEnums.YesNo.Yes)
                {
                    AISystem.state = 1f;
                    AISystem.EnableIK();
                    AISystem.elapsedTime = AISystem.InverseKinematics.HandIKSmooth;
                }
            }
            AISystem.AIEvents.OnDeath.AddListener(OnDeath);
            AISystem.PrivateReloadEnd.AddListener(ReloadEndStart);
            
            AISystem.OnEquipped.AddListener(Equipped);
            AISystem.OnUnEquipped.AddListener(UnEquipped);
            
            AISystem.WeaponStateEvent.AddListener(OnWeaponStateChanged);

            AISystem.Settings.Attack.AttackDistance = WeaponSettings.WeaponAttackDistance;
            
            //Set Weapon Renderers

           OnWeaponStateChanged(WeaponSettings.AlwaysEquipped == AIEnums.YesNo.Yes);
        }
    }

#if UNITY_EDITOR
    
[CustomEditor(typeof(AIShooterWeapon))]
public class AIShooterWeaponEditor : Editor
{
    private static readonly string[] _dontIncludeMe = new string[]{"m_Script"};
    
    public AIShooterWeapon ShooterWeapon;
    
    private void OnEnable()
    {
        ShooterWeapon = (AIShooterWeapon)target;
    }

    private bool IgnoreLayerWarning;
    private bool IgnoreStorageWarning;
    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical();
        var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
        style.fontSize = 13;
        EditorGUILayout.LabelField(new GUIContent(Resources.Load("LogoBrain") as Texture), style, GUILayout.ExpandWidth(true), GUILayout.Height(43));
        EditorGUILayout.LabelField("Universal AI Shooter Weapon", style, GUILayout.ExpandWidth(true));
            
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        
         if (!Application.isPlaying)
        {
            #region Error Checker

            ShooterWeapon.OnValidate();
            if (ShooterWeapon.AISystem == null)
            {
                EditorGUILayout.Space();
                GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                EditorGUILayout.HelpBox("The 'AISystem' component of the weapon's parent is null, make sure that this weapon is under the AI object!", MessageType.Error);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();

                ShooterWeapon.Reasons =
                    "The 'AISystem' component of the weapon's parent is null, make sure that this weapon is under the AI object, disabling the Object: ' " +
                    ShooterWeapon.transform.root.gameObject.name + " ' !";
                ShooterWeapon.StartWithError = true;
            }
            else if(ShooterWeapon.WeaponSettings.ProjectileObject == null && ShooterWeapon.WeaponSettings.FireType == AIEnums.FireType.Projectile)
            {
                EditorGUILayout.Space();
                GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                EditorGUILayout.HelpBox("The 'Projectile Object' of the Weapon is null, make sure you assign one!", MessageType.Error);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();

                ShooterWeapon.Reasons =
                    "The 'Projectile Object' of the Weapon is null, make sure you assign one, disabling the AI: ' " +
                    ShooterWeapon.AISystem.gameObject.name + " ' !";
                ShooterWeapon.StartWithError = true;
            }
            else if(ShooterWeapon.WeaponSettings.FireRate <= 0)
            {
                ShooterWeapon.WeaponSettings.FireRate = 0;
                EditorGUILayout.Space();
                GUI.backgroundColor = new Color(10f, 10f, 0.0f, 0.25f);
                EditorGUILayout.HelpBox("The 'Fire Rate' of the Weapon is zero, make sure to increase it!", MessageType.Warning);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();

                ShooterWeapon.Reasons = String.Empty;
                ShooterWeapon.StartWithError = false;
            }
            else if(ShooterWeapon.WeaponSettings.WeaponAttackDistance <= ShooterWeapon.AISystem.Settings.Movement.TooCloseDistance)
            {
                EditorGUILayout.Space();
                GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                EditorGUILayout.HelpBox("The 'Attack Distance' of the Weapon is smaller than AI 'Too Close Distance', make sure to increase it!", MessageType.Error);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();

                ShooterWeapon.Reasons =
                    "The 'Attack Distance' of the Weapon is smaller than AI 'Too Close Distance', make sure to increase it, disabling the AI: ' " +
                    ShooterWeapon.AISystem.gameObject.name + " ' !";
                ShooterWeapon.StartWithError = true;
            }
            else if(ShooterWeapon.AmmoSettings.MagazineAmmo <= 0)
            {
                EditorGUILayout.Space();
                GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                EditorGUILayout.HelpBox("The 'Magazine Ammo' of the Weapon is zero, make sure to increase it!", MessageType.Error);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();

                ShooterWeapon.Reasons =
                    "The 'Magazine Ammo' of the Weapon is zero, make sure to increase it, disabling the AI: ' " +
                    ShooterWeapon.AISystem.gameObject.name + " ' !";
                ShooterWeapon.StartWithError = true;
            }
            else if(ShooterWeapon.AmmoSettings.StorageAmmo <= 0 && !IgnoreStorageWarning)
            {
                EditorGUILayout.Space();
                GUI.backgroundColor = new Color(10f, 10f, 0.0f, 0.25f);
                EditorGUILayout.HelpBox("The 'Storage Ammo' of the Weapon is zero, your AI won't be able to reload!", MessageType.Warning);
                GUI.backgroundColor = Color.white;

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
 
                if (GUILayout.Button("Ignore Warning", GUILayout.Width(130),GUILayout.Height(20)))
                {
                    IgnoreStorageWarning = true;
                }
                
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                
                EditorGUILayout.Space();
                
                ShooterWeapon.Reasons = String.Empty;
                ShooterWeapon.StartWithError = false;
            }
            else if(ShooterWeapon.WeaponSettings.FireType == AIEnums.FireType.Projectile && ShooterWeapon.WeaponSettings.ProjectileObject != null && Physics.GetIgnoreLayerCollision(ShooterWeapon.WeaponSettings.ProjectileObject.gameObject.layer, ShooterWeapon.AISystem.gameObject.layer) && !IgnoreLayerWarning)
            {
                EditorGUILayout.Space();
                GUI.backgroundColor = new Color(10f, 10f, 0.0f, 0.25f);
                EditorGUILayout.HelpBox("The 'Projectile Layer' and 'AI Layer' can't collide with each other, please change it in order to damage enemies!", MessageType.Warning);
                GUI.backgroundColor = Color.white;
                
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
 
                if (GUILayout.Button("Ignore Warning", GUILayout.Width(130),GUILayout.Height(20)))
                {
                    IgnoreLayerWarning = true;
                }
                
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                
                EditorGUILayout.Space();
                
                ShooterWeapon.Reasons = String.Empty;
                ShooterWeapon.StartWithError = false;
            }
            #endregion
        }
            
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, _dontIncludeMe);
            
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
}