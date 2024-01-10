using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util {

    public static float PingPong(float t) {
        float frequency = t*2f; // unity pingpong takes 2 seconds by default, we reduce that to 1 second
        return Mathf.PingPong(frequency, 1f);
    }

    public static float MarioBlink(float t) {
        return Mathf.Clamp(PingPong(t*1.2f) * 10 - 5, 0f, 1f);
    }

    public static float MarioFireBlink(float t) {
        float frequency = 1f; // seconds
        float slope = frequency * 100f;
        float activationRatio = 0.2f; // time must be active
        float levelAdjust = slope * activationRatio;

        return Mathf.Clamp(PingPong(t*frequency) * slope - levelAdjust, 0f, 1f);
    }
}
