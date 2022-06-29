using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
  public class Aggro : MonoBehaviour
  {
    public TriggerObserver triggerObserver;
    public Follow follow;

    public float cooldown;
    private bool _hasAggroTarget;

    private WaitForSeconds _switchFollowOffAfterCooldown;
    private Coroutine _aggroCoroutine;

    private void Start()
    {
      _switchFollowOffAfterCooldown = new WaitForSeconds(cooldown);
      
      triggerObserver.TriggerEnter += TriggerEnter;
      triggerObserver.TriggerExit += TriggerExit;

      follow.enabled = false;
    }

    private void OnDestroy()
    {
      triggerObserver.TriggerEnter -= TriggerEnter;
      triggerObserver.TriggerExit -= TriggerExit;
    }

    private void TriggerEnter(Collider obj)
    {
      if(_hasAggroTarget) return;
      
      StopAggroCoroutine();

      SwitchFollowOn();
    }

    private void TriggerExit(Collider obj)
    {
      if(!_hasAggroTarget) return;
      
      _aggroCoroutine = StartCoroutine(SwitchFollowOffAfterCooldown());
    }

    private void StopAggroCoroutine()
    {
      if(_aggroCoroutine == null) return;
      
      StopCoroutine(_aggroCoroutine);
      _aggroCoroutine = null;
    }

    private IEnumerator SwitchFollowOffAfterCooldown()
    {
      yield return _switchFollowOffAfterCooldown;
      
      SwitchFollowOff();
    }

    private void SwitchFollowOn()
    {
      _hasAggroTarget = true;
      follow.enabled = true;
    }

    private void SwitchFollowOff()
    {
      follow.enabled = false;
      _hasAggroTarget = false;
    }
  }
}