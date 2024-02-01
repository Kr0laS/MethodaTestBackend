namespace MethodaTest.Model;

public class Transition
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int FromStatusId { get; set; }
    public int ToStatusId { get; set; }
}
