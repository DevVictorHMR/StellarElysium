namespace StellarElysium.Domain.Utils;

public class PaginadoResultado<T>
{
    public int Total { get; set; }
    public List<T> Itens { get; set; } = new();
}
