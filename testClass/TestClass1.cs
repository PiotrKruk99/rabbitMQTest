using MessagePack;

namespace testClass;

[MessagePackObject(true)]
public class TestClass1
{
    public required string msg1 { get; set; }
    public required int number1 { get; set; }
}
