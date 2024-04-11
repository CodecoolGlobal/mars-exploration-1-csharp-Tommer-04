using System.Text;

namespace Codecool.MarsExploration.MapElements.Model;

public record Map(string?[,] Representation, bool SuccessfullyGenerated = false)
{
    protected static string CreateStringRepresentation(string?[,] arr)
    {
        if (arr == null)
        {
            return "InvalidMap";
        } else
        {
            return string.Join("", arr.Cast<string>()) ?? "Invalid map";
        }
    }

    public override string ToString()
    {
        return CreateStringRepresentation(Representation);
    }
}
