using System.Collections.Generic;

//Use to store data in memory safely.
public class SafeMemory
{
    private static Dictionary<string, string> memory = new Dictionary<string, string>();

    public static void Set(string key, string data)
    {
        memory[key] = B64X.Encode(data);
    }

    public static string Get(string key)
    {
        return B64X.Decode(memory[key]);
    }

    public static int GetInt(string key)
    {
        return System.Int32.Parse(Get(key));
    }

    public static float GetFloat(string key)
    {
        return float.Parse(Get(key));
    }
}
