using System.Collections;
using UnityEngine;
using VContainer;

[DisallowMultipleComponent]
public sealed class PowerDrainRunner : MonoBehaviour
{
    private PowerController powerController;
    private int drainAmount;
    private float drainIntervalSeconds;
    private Coroutine drainCoroutine;
    private bool isInitialized;

    [Inject]
    public void Construct(
        PowerController powerController,
        int drainAmount,
        float drainIntervalSeconds)
    {
        this.powerController = powerController;
        this.drainAmount = drainAmount;
        this.drainIntervalSeconds = drainIntervalSeconds;
        isInitialized = true;

        StartDrainIfPossible();
    }

    private void OnEnable()
    {
        StartDrainIfPossible();
    }

    private void OnDisable()
    {
        StopDrain();
    }

    private void StartDrainIfPossible()
    {
        if (!isInitialized || drainCoroutine != null)
        {
            return;
        }

        drainCoroutine = StartCoroutine(DrainPowerOverTime());
    }

    private void StopDrain()
    {
        if (drainCoroutine == null)
        {
            return;
        }

        StopCoroutine(drainCoroutine);
        drainCoroutine = null;
    }

    private IEnumerator DrainPowerOverTime()
    {
        WaitForSeconds waitForDrainInterval = new WaitForSeconds(drainIntervalSeconds);

        while (true)
        {
            yield return waitForDrainInterval;
            powerController.ReducePower(drainAmount);
        }
    }
}
