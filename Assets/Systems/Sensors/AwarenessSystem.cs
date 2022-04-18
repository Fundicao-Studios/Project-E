using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackedTarget
{
    public DetectableTarget Detectable;
    public Vector3 RawPosition;

    public float LastSensedTime = -1f;
    public float Awareness; // 0     = não está ciente (vai ser culled);
                            // 0-1   = uma pequena ideia (sem local definido)
                            // 1-2   = alvo provável (local)
                            // 2     = totalmente detetado

    public bool UpdateAwareness(DetectableTarget target, Vector3 position, float awareness, float minAwareness)
    {
        var oldAwareness = Awareness;

        if (target != null)
            Detectable      = target;
        RawPosition     = position;
        LastSensedTime  = Time.time;
        Awareness       = Mathf.Clamp(Mathf.Max(Awareness, minAwareness) + awareness, 0f, 2f);

        if (oldAwareness < 2f && Awareness >= 2f)
            return true;
        if (oldAwareness < 1f && Awareness >= 1f)
            return true;
        if (oldAwareness <= 0f && Awareness >= 0f)
            return true;

        return false;
    }

    public bool DecayAwareness(float decayTime, float amount)
    {
        // detetado muito recentemente - sem mudanças
        if ((Time.time - LastSensedTime) < decayTime)
            return false;

        var oldAwareness = Awareness;

        Awareness -= amount;

        if (oldAwareness >= 2f && Awareness < 2f)
            return true;
        if (oldAwareness >= 1f && Awareness < 1f)
            return true;
        return Awareness <= 0f;
    }
}

[RequireComponent(typeof(EnemyAI))]
public class AwarenessSystem : MonoBehaviour
{
    [SerializeField] AnimationCurve VisionSensitivity;
    [SerializeField] float VisionMinimumAwareness = 1f;
    [SerializeField] float VisionAwarenessBuildRate = 10f;

    [SerializeField] float HearingMinimumAwareness = 0f;
    [SerializeField] float HearingAwarenessBuildRate = 0.5f;

    [SerializeField] float ProximityMinimumAwareness = 0f;
    [SerializeField] float ProximityAwarenessBuildRate = 1f;
    
    [SerializeField] float AwarenessDecayDelay = 0.1f;
    [SerializeField] float AwarenessDecayRate = 0.1f;

    Dictionary<GameObject, TrackedTarget> Targets = new Dictionary<GameObject, TrackedTarget>();
    EnemyAI LinkedAI;

    public Dictionary<GameObject, TrackedTarget> ActiveTargets => Targets;

    // Start is called before the first frame update
    void Start()
    {
        LinkedAI = GetComponent<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        List<GameObject> toCleanup = new List<GameObject>();
        foreach(var targetGO in Targets.Keys)
        {
            if (Targets[targetGO].DecayAwareness(AwarenessDecayDelay, AwarenessDecayRate * Time.deltaTime))
            {
                if (Targets[targetGO].Awareness <= 0f)
                {
                    LinkedAI.OnFullyLost();
                    toCleanup.Add(targetGO);
                }
                else
                {
                    if (Targets[targetGO].Awareness >= 1f)
                        LinkedAI.OnLostDetect(targetGO);
                    else
                        LinkedAI.OnLostSuspicion();
                }
            }
        }

        // limpar os alvos que já não são detetados
        foreach(var target in toCleanup)
            Targets.Remove(target);
    }

    void UpdateAwareness(GameObject targetGO, DetectableTarget target, Vector3 position, float awareness, float minAwareness)
    {
        // não está na lista dos alvos
        if (!Targets.ContainsKey(targetGO))
            Targets[targetGO] = new TrackedTarget();

        // atualizar a awareness do alvo
        if (Targets[targetGO].UpdateAwareness(target, position, awareness, minAwareness))
        {
            if (Targets[targetGO].Awareness >= 2f)
                LinkedAI.OnFullyDetected(targetGO);
            else if (Targets[targetGO].Awareness >= 1f)
                LinkedAI.OnDetected(targetGO);
            else if (Targets[targetGO].Awareness >= 0f)
                LinkedAI.OnSuspicious();
        }
    }

    public void ReportCanSee(DetectableTarget seen)
    {
        // determina onde está o alvo dentro do campo de visão
        var vectorToTarget = (seen.transform.position - LinkedAI.EyeLocation).normalized;
        var dotProduct = Vector3.Dot(vectorToTarget, LinkedAI.EyeDirection);

        // determina a contribuição da awareness
        var awareness = VisionSensitivity.Evaluate(dotProduct) * VisionAwarenessBuildRate * Time.deltaTime;

        UpdateAwareness(seen.gameObject, seen, seen.transform.position, awareness, VisionMinimumAwareness);
    }

    public void ReportCanHear(GameObject source, Vector3 location, EHeardSoundCategory category, float intensity)
    {
        var awareness = intensity * HearingAwarenessBuildRate * Time.deltaTime;

        UpdateAwareness(source, null, location, awareness, HearingMinimumAwareness);
    }

    public void ReportInProximity(DetectableTarget target)
    {
        var awareness = ProximityAwarenessBuildRate * Time.deltaTime;

        UpdateAwareness(target.gameObject, target, target.transform.position, awareness, ProximityMinimumAwareness);
    }
}