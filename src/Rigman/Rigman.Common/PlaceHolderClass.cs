namespace Rigman.Common
{
    public class PlaceHolderClass : IPlaceHolderClass
    {
        public string Nothing { get; set; }
    }

    public interface IPlaceHolderClass
    {
        string Nothing { get; set; }
    }
}