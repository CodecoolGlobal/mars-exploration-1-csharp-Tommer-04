using System.Text;

namespace Codecool.MarsExploration.MapElements.Model;

public record Map(string?[,] Representation, bool SuccessfullyGenerated = false)
{
    protected static string CreateStringRepresentation(string?[,] arr)
    {
        if (arr == null)
        {
            return "Invalid Map";
        } 
        return string.Join("", arr.Cast<string>());
    }

    public override string ToString()
    {
        return CreateStringRepresentation(Representation);
    }
}
