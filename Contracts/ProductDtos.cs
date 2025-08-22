namespace ProductApi.Contracts;
public record ProductCreateDto(string ProductCode, string ProductName, decimal Price);
public record ProductUpdateDto(string ProductCode, string ProductName, decimal Price);
