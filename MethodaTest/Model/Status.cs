namespace MethodaTest.Model;

public class Status
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsInitial { get; set; }
    public bool IsFinal { get; set; }
    public bool IsOrphan { get; set; }
}
