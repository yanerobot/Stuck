using UnityEngine;
using System.Collections.Generic;
using UnityEngine.U2D.IK;

[DefaultExecutionOrder(-1)]
public class Ragdoll2D : MonoBehaviour
{
    [SerializeField] List<RagdollPart> ragdollParts;

    [SerializeField] Collider2D mainCollider;
    [SerializeField] Animator animator;
    [SerializeField] IKManager2D ik;

    void Awake()
    {
        ragdollParts = new List<RagdollPart>();

        transform.ActOnEveryChild(
            (parent) => {
                var instance = CreateRagdollPartInstance(parent);
                if (instance != null)
                    ragdollParts.Add(instance);
            });
        ActivateRagdoll(false);
    }

    RagdollPart CreateRagdollPartInstance(Transform t)
    {
        var joint = t.GetComponent<HingeJoint2D>();
        var coll = t.GetComponent<Collider2D>();
        if (joint == null && coll == null)
        {
            return null;
        }

        return new RagdollPart(coll, joint, t);
    }
    public void ActivateRagdoll(bool enabled)
    {
        mainCollider.enabled = !enabled;
        mainCollider.attachedRigidbody.simulated = !enabled;
        animator.enabled = !enabled;
        ik.enabled = !enabled;

        foreach (var part in ragdollParts)
        {
            part.SetActiveComponents(enabled);

            if (!enabled) 
                part.Reset();
            else
                part.transform.SetParent(null);
        }
    }
}
public class RagdollPart
{
    public Transform parent;
    public Transform transform;
    public Collider2D coll;
    public HingeJoint2D joint;
    public Vector3 initialPosition;
    public Vector3 initialEuler;

    public RagdollPart(Collider2D coll, HingeJoint2D joint, Transform inititalTransform)
    {
        this.coll = coll;
        this.joint = joint;
        transform = inititalTransform;
        parent = transform.parent;
        initialPosition = inititalTransform.localPosition;
        initialEuler = inititalTransform.localEulerAngles;
    }

    public void SetActiveComponents(bool enabled)
    {
        coll.enabled = enabled;
        coll.attachedRigidbody.simulated = enabled;
        if (joint != null) joint.enabled = enabled;
    }

    public void Reset()
    {
        transform.SetParent(parent);
        transform.localPosition = initialPosition;
        transform.localEulerAngles = initialEuler;
    }
}
