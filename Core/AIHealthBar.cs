using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AI
{
    public class AIHealthBar : MonoBehaviour
    {
        public AIEnums.HealthBarDisplay HealthBarDisplay;
        
        [HideInInspector] public AISystem system;

        [HideInInspector] public Image HealthBar;

        [HideInInspector] public Text NameText;

         public string AIName = "AI";
        
         public Color HealthBarColor = Color.red;

         public Color EnemyNameColor = Color.red;
       
         public Color FriendlyNameColor = Color.green;

        [HideInInspector] public CanvasGroup CG;
        private void Awake()
        {
            if (system == null) 
            {
                if (transform.parent != null)
                {
                    if (transform.parent.GetComponent<AISystem>() != null)
                    {
                        system = transform.parent.GetComponent<AISystem>();
                    }   
                }
            }
            
            if(system == null)
                return;
            
            system.AIHealthBar = this;
            
            if(CG == null)
                CG = transform.GetChild(0).GetComponent<CanvasGroup>();
            
            CG.alpha = 0;
            
            if (HealthBarDisplay == AIEnums.HealthBarDisplay.OnlyHealth)
            {
                NameText.enabled = false;
                HealthBar.transform.parent.gameObject.SetActive(true);
            }
            
            if (HealthBarDisplay == AIEnums.HealthBarDisplay.OnlyName)
            {
                NameText.enabled = true;
                HealthBar.transform.parent.gameObject.SetActive(false);
            }
            
            if (HealthBarDisplay == AIEnums.HealthBarDisplay.HealthAndName)
            {
                NameText.enabled = true;
                HealthBar.transform.parent.gameObject.SetActive(true);
            }

            HealthBar.color = HealthBarColor;
        }

        public void OnValidate()
        {
            if(Application.isPlaying)
                return;

            if (system == null) 
            {
                if (transform.parent != null)
                {
                    if (transform.parent.GetComponent<AISystem>() != null)
                    {
                        system = transform.parent.GetComponent<AISystem>();
                    }   
                }
            }
            
            if(system == null)
                return;
            
            system.AIHealthBar = this;
            
            if (HealthBarDisplay == AIEnums.HealthBarDisplay.OnlyHealth)
            {
                NameText.enabled = false;
                HealthBar.transform.parent.gameObject.SetActive(true);
            }
            
            if (HealthBarDisplay == AIEnums.HealthBarDisplay.OnlyName)
            {
                NameText.enabled = true;
                HealthBar.transform.parent.gameObject.SetActive(false);
            }
            
            if (HealthBarDisplay == AIEnums.HealthBarDisplay.HealthAndName)
            {
                NameText.enabled = true;
                HealthBar.transform.parent.gameObject.SetActive(true);
            }

            if (system.Stats == null)
            {
                system.Stats = new AISystem.stats();
            }

            if (CG == null)
            {
                CG = transform.GetChild(0).GetComponent<CanvasGroup>();
            }
            
            HealthBar.fillAmount = system.Stats.StartHealth;

            NameText.text = AIName;
        }

       
        private void FixedUpdate()
        {

            if (system.General.AIType == AIEnums.AIType.Enemy)
            {
                NameText.color = EnemyNameColor;
            }
            else
            {
                NameText.color = FriendlyNameColor;
            }
           
            HealthBar.fillAmount = system.Health / system.Stats.StartHealth;

            NameText.text = AIName;

            if (system.Target == null && CG.alpha > 0.9f)
            {
                FadeOut();
            }
            
            if (system.Target != null && CG.alpha < 0.1f)
            {
                FadeIn();
            }

            if (system.Target != null)
            {
                transform.LookAt(transform.parent.position + system.Target.transform.rotation * Vector3.forward,
                    system.Target.transform.rotation * Vector3.up);
            }
          
        }
        
        public void FadeOut()
        {
            if (gameObject.activeSelf)
            {
                StartCoroutine(FadeTo(0f, 1f));
            }
        }
        
        public void FadeIn()
        {
            if (gameObject.activeSelf)
            {
                StartCoroutine(FadeTo(1f, 1f));
            }
        }
        
        IEnumerator FadeTo(float aValue, float aTime)
        {
            float alpha = CG.alpha;
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
            {
                if (aValue.Equals(0f))
                {
                    if (CG.alpha <= 0.05f)
                    {
                        CG.alpha = 0;
                        break;
                    }
                }
                if (aValue.Equals(1f))
                {
                    if (CG.alpha >= 0.95f)
                    {
                        CG.alpha = 1;
                        break;
                    }
                }
                
                CG.alpha = Mathf.Lerp(alpha, aValue, t);
                yield return null;
            }
        }
    }
    
}