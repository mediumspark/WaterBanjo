using UnityEngine;

public static class HalfVector
{
    //Half down
    public static Vector3 HalfForward => Vector3.forward / 2; 
    public static Vector3 HalfBack => Vector3.back / 2; 
    public static Vector3 HalfUp => Vector3.up / 2; 
    public static Vector3 HalfDown => Vector3.down / 2; 
    public static Vector3 HalfLeft => Vector3.left / 2; 
    public static Vector3 HalfRight => Vector3.right / 2;

    //Half up
    public static Vector3 SesquiForward => Vector3.forward * 1.5f;
    public static Vector3 SesquiBack => Vector3.back * 1.5f;
    public static Vector3 SesquiUp => Vector3.up * 1.5f;
    public static Vector3 SesquiDown => Vector3.down * 1.5f;
    public static Vector3 SesquiLeft => Vector3.left * 1.5f;
    public static Vector3 SesquiRight => Vector3.right * 1.5f;

    /// <summary>
    /// (0.5f, 0.5f, 0.5f)
    /// </summary>
    public static Vector3 Half => Vector3.one / 2; 
}
