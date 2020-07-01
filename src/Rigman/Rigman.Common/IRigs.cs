namespace Rigman.Common
{
    public interface IRigs
    {
        string Test { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        int Execute();
    }
}