using NUnit.Framework;
using Sana.Utils;
using UnityEngine;

public class MovingBoxTest
{
    [Test]
    public void TestConvertXZ()
    {
        var result = new Vector2(1f, 2f).ConvertXZ();
        var xOk = result.x == 1f;
        var yOk = result.y == 0f;
        var zOk = result.z == 2f;
        var functionOk = xOk && yOk && zOk;
        
        Assert.IsTrue(functionOk);
    }
}
