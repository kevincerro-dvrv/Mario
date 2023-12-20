using UnityEngine;

public class Util
{
    // Start is called before the first frame update
    public static float PingPong(float t)
    {
        return Mathf.PingPong(t * 2f, 1f);
    }

    public static float MarioBlink(float t)
    {
        return Mathf.Clamp(PingPong(t * 1.2f) * 10 - 5, 0f, 1f);
    }
}
