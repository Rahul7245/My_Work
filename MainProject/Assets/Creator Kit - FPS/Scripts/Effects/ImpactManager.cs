using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// This handle impacts on object from the raycast of the weapon. It will create a pool of the prefabs for performance
/// optimisation.
/// </summary>
public class ImpactManager : MonoBehaviour
{
    [System.Serializable]
    public class ImpactSetting
    {
        public ParticleSystem ParticlePrefab;
        public AudioClip ImpactSound;
        public Material TargetMaterial;
       
    }

    static public ImpactManager Instance { get; protected set; }

    public ImpactSetting DefaultSettings;
    public ImpactSetting[] ImpactSettings;
    public GameObject PointsCanvas;
    public GameObject ClipsizeText;

    Dictionary<Material, ImpactSetting> m_SettingLookup = new Dictionary<Material,ImpactSetting>();

    Vector3 m_position;
    Vector3 m_normal; 
    Material m_material = null;
    public int m_points = 0;
    // Start is called before the first frame update

    ShotEvent shotEvent = new ShotEvent();
    SwitchCameraEvent switchCameraEvent = new SwitchCameraEvent();
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        PointsCanvas.SetActive(false);
        // PoolSystem.Instance.InitPool(DefaultSettings.ParticlePrefab, 3);
        foreach (var impactSettings in ImpactSettings)
        {
            PoolSystem.Instance.InitPool(impactSettings.ParticlePrefab, 1);
            m_SettingLookup.Add(impactSettings.TargetMaterial, impactSettings);
        }
        EventManager.AddShotInvoker(this);
        EventManager.AddCameraSwitchInvoker(this);
    }
    public void ImpactData(Vector3 position, Vector3 normal, Material material = null) {
         m_position=position;
      //  print("ImpactData m_position" + m_position);
         m_normal=normal;
         m_material =material;
        PlayImpact();
    }
    public Vector3 GetImpactPosition()
    {
        return m_position;
    }
    public Vector3 GetImpactNormal()
    {
     //   print("m_normalwe" + m_normal);
        return m_normal;
    }
    public void PlayImpact()
    {
      //  print("PlayImpact m_position" + m_position);
        ImpactSetting setting = null;
        if (m_material == null || !m_SettingLookup.TryGetValue(m_material, out setting))
        {
            setting = ImpactSettings[0];
        }
        
        var sys =  PoolSystem.Instance.GetInstance<ParticleSystem>(setting.ParticlePrefab);
        sys.gameObject.transform.position = m_position;
        sys.gameObject.transform.forward = m_normal;

        sys.gameObject.SetActive(true);
        sys.Play();

        var source = WorldAudioPool.GetWorldSFXSource();

        source.transform.position = m_position;
        source.pitch = Random.Range(0.8f, 1.1f);
        source.PlayOneShot(setting.ImpactSound);
    }
   public void AddShotEventListener(UnityAction<int> listener) {
        print("AddShotEventListener");
        shotEvent.AddListener(listener);
    }
    public void AddSwitchListener(UnityAction<bool> listener)
    {
        switchCameraEvent.AddListener(listener);
    }
    public void InvokeTheEvent(int points) {
        PointsCanvas.SetActive(true);
        m_points = points;
        PointsCanvas.GetComponentInChildren<Text>().text ="You Shot "+points;
        Controller.Instance.DisplayCursor(true);
    }
    public void OkButtonClicked() {
        PointsCanvas.SetActive(false);
        switchCameraEvent.Invoke(false);
        shotEvent.Invoke(m_points);
        m_points = 0;
        
    }

}
