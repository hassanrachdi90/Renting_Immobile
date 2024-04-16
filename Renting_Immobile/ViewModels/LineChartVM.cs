namespace Renting_Immobile.ViewModels
{
    public class LineChartVM
    {
        public List<CharData> Series { get; set; }
        public string[] Categories { get; set; }
    }
    public class CharData
    {
          public string Name { get; set; }
          public int[] Data { get; set; }
    }
}
