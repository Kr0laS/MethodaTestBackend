namespace MethodaTest.Dto;

public class CreateTransitionDto
{
    public string Name { get; set; }
    public int FromStatusId { get; set; }
    public int ToStatusId { get; set; }
}
