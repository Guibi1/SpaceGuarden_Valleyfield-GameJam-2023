using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SimpleRoutines
{
    public static IEnumerator WaitForEndOfFrame(System.Action callback = null)
    {
        yield return new WaitForEndOfFrame();

        callback?.Invoke();
    }

    public static IEnumerator WaitForNextFrame(System.Action callback = null)
    {
        yield return null;

        callback?.Invoke();
    }

    public static IEnumerator WaitForFrames(int framesToWait, System.Action callback = null)
    {
        int counter = 0;

        while (counter < framesToWait)
        {
            ++counter;
            yield return null;
        }

        callback?.Invoke();
    }

    public static IEnumerator WaitTime(float time, System.Action callback = null)
    {
        float timer = 0.0f;

        while (timer < time)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        callback?.Invoke();
    }

    public static IEnumerator WaitWhile(System.Func<bool> condition, System.Action callback = null)
    {
        while (condition.Invoke() == true)
        {
            yield return null;
        }

        callback?.Invoke();
    }

    public static IEnumerator WaitUntil(System.Func<bool> condition, System.Action callback = null)
    {
        while (condition.Invoke() == false)
        {
            yield return null;
        }

        callback?.Invoke();
    }
    
    public static IEnumerator CustomCurveColorLerpCoroutine(Color startColor, Color endColor, float duration, AnimationCurve curve, System.Action<Color> lerpAction,  System.Action callback = null)
    {
        if (curve == null)
        {
            //curve = GameDataManager.Instance.LerpCurve;
            Debug.LogWarning("Custom curve have not been initialized. Using default curve.");
        }
        
        float timer = 0.0f;
        while (timer < duration)
        {
            Color currentColor = Color.Lerp(startColor, endColor, curve.Evaluate(timer / duration));
            lerpAction(currentColor);
            timer += Time.deltaTime;
            yield return null;
        }

        lerpAction(endColor);
        callback?.Invoke();
    }
    
    
    
    public static IEnumerator CustomCurveLerpCoroutine(System.Action<float> lerpAction, float startValue, float endValue, float duration, AnimationCurve curve, System.Action callback = null, bool isEndSnap = true)
    {
        if (curve == null)
        {
            //curve = GameDataManager.Instance.LerpCurve;
            Debug.LogWarning("Custom curve have not been initialized. Using default curve.");
        }
        
        float timer = 0.0f;
        while (timer < duration)
        {
            float currentValue = Mathf.Lerp(startValue, endValue, curve.Evaluate(timer / duration));
            lerpAction(currentValue);
            timer += Time.deltaTime;
            yield return null;
        }

        if (isEndSnap) lerpAction(endValue);
        
        callback?.Invoke();
    }

    public static IEnumerator LerpCoroutine(float startValue, float endValue, float duration, System.Action<float> lerpAction,  System.Action callback = null)
    {
        float timer = 0.0f;
        while (timer < duration)
        {
            float currentValue = Mathf.Lerp(startValue, endValue, timer / duration);
            lerpAction(currentValue);
            timer += Time.deltaTime;
            yield return null;
        }

        lerpAction(endValue);
        callback?.Invoke();
    }
    
    public static IEnumerator UnscaledLerpCoroutine(float startValue, float endValue, float duration, System.Action<float> lerpAction,  System.Action callback = null)
    {
        float timer = 0.0f;
        while (timer < duration)
        {
            float currentValue = Mathf.Lerp(startValue, endValue, timer / duration);
            lerpAction(currentValue);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        lerpAction(endValue);
        callback?.Invoke();
    }
    
    public static IEnumerator UnscaledCurvedLerpCoroutine(float startValue, float endValue, float duration, AnimationCurve curve, System.Action<float> lerpAction,  System.Action callback = null)
    {
        float timer = 0.0f;
        while (timer < duration)
        {
            float currentValue = Mathf.Lerp(startValue, endValue, curve.Evaluate(timer / duration));
            lerpAction(currentValue);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        lerpAction(endValue);
        callback?.Invoke();
    }
    
    public static IEnumerator LerpCoroutine(Vector3 startValue, Vector3 endValue, float duration, System.Action<Vector3> lerpAction,  System.Action callback = null)
    {
        float timer = 0.0f;
        while (timer < duration)
        {
            float x = Mathf.Lerp(startValue.x, endValue.x, timer / duration);
            float y = Mathf.Lerp(startValue.y, endValue.y, timer / duration);
            float z = Mathf.Lerp(startValue.z, endValue.z, timer / duration);
            lerpAction(new Vector3(x,y,z));
            timer += Time.deltaTime;
            yield return null;
        }

        lerpAction(endValue);
        callback?.Invoke();
    }
    
    public static IEnumerator CustomCurveLerpCoroutine(Vector3 startValue, Vector3 endValue, float duration, AnimationCurve curve,System.Action<Vector3> lerpAction,  System.Action callback = null)
    {
        float timer = 0.0f;
        while (timer < duration)
        {
            float x = Mathf.Lerp(startValue.x, endValue.x, curve.Evaluate(timer / duration));
            float y = Mathf.Lerp(startValue.y, endValue.y, curve.Evaluate(timer / duration));
            float z = Mathf.Lerp(startValue.z, endValue.z, curve.Evaluate(timer / duration));
            lerpAction(new Vector3(x,y,z));
            timer += Time.deltaTime;
            yield return null;
        }

        lerpAction(endValue);
        callback?.Invoke();
    }
}
