using System.Data;
using KodaStore.Api.DTOs;
using KodaStore.Api.Models;
using Microsoft.Data.SqlClient;

namespace KodaStore.Api.Repositories;

public sealed class SqlProductRepository : IProductRepository
{
    private readonly string _connectionString;

    public SqlProductRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("eccomercedb") ?? throw new InvalidOperationException("Connection menghilang");
    }

    public async Task<bool> AddProductAsync(Products products, CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO dbo.Product(ProductCode, Name, Price, Stock, Category) VALUES (@productCode, @name, @price, @stock, @category)
        """;
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql,connection);
        AddProductParameter(command, products);
        try
        {
            var affectedRows = await command.ExecuteNonQueryAsync(cancellationToken);
            return affectedRows == 1;
        }
        catch (SqlException e) when (e.Number is 2601 or 2627)
        {
            return false;
        }
        catch (SqlException e)
        {
            Console.WriteLine($"SQL Error tidak terduga: {e.Number} - {e.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteProductAsync(string productCode, CancellationToken cancellationToken)
    {
        const string sql = """
            DELETE FROM dbo.Product WHERE
            ProductCode = @productCode;
        """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        AddProductCode(command, productCode);
        var affectedRows = await command.ExecuteNonQueryAsync(cancellationToken);
        return affectedRows == 1;
    }

    public async Task<IReadOnlyList<Products>> GetAllAsync(CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT ProductCode, Name, Price, Stock, Category from dbo.Product
        """;

        var product = new List<Products>();
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while(await reader.ReadAsync(cancellationToken))
        {
            product.Add(ReadProduct(reader));
        }
        return product;
    }

    public async Task<IReadOnlyList<ProductCustomerResponse>> GetAllWithCustomerAsync(
    CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                p.ProductCode,
                p.Name,
                p.Price,
                p.Stock,
                p.Category,
                c.Name,
                c.Email
            FROM dbo.Product p
            JOIN dbo.Customers c
                ON c.CustomerId = p.CustomerId
            """;

        var products = new List<ProductCustomerResponse>();
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            products.Add(new ProductCustomerResponse(
                reader.GetString(0),
                reader.GetString(1),
                reader.GetDecimal(2),
                reader.GetInt32(3),
                reader.GetString(4),
                reader.GetString(5),
                reader.GetString(6)
            ));
        }

        return products;
    }

    public async Task<Products?> GetByCodeAsync(string productCode, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT p.ProductCode, p.Name, p.Price, p.Stock, p.Category, c.Name, c.Email from dbo.Product p join Customers c on c.CustomerId = p.CustomerId WHERE ProductCode = @productCode
        """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);

        AddProductCode(command, productCode);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if(!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }
        return ReadProduct(reader);
    }

    public async Task<bool> UpdateProductAsync(Products products, CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE dbo.Product Set 
                    Name = @name,
                    Price = @price,
                    Stock = @stock,
                    Category = @category,
                WHERE ProductCode = @productCode
        """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        AddProductParameter(command, products);

        var affectedRows = await command.ExecuteNonQueryAsync(cancellationToken);
        return affectedRows == 1;
    }


    private static void AddProductParameter(SqlCommand command, Products products)
    {
        AddProductCode(command, products.ProductCode);
        command.Parameters.Add("@name", SqlDbType.NVarChar, 120).Value = products.Name;
        command.Parameters.Add("@stock", SqlDbType.Int).Value = products.Stock;
        var price = command.Parameters.Add("@price", SqlDbType.Decimal);
        command.Parameters.Add("@category", SqlDbType.NVarChar, 80).Value = products.Category;

        price.Precision = 18;
        price.Scale = 2;
        price.Value = products.Price;
    }

    private static void AddProductCode(SqlCommand command, string productCode)
    {
        command.Parameters.Add("@productCode", SqlDbType.NVarChar, 30).Value = productCode.Trim().ToUpperInvariant();
    }

    private static Products ReadProduct(SqlDataReader reader)
    {
        return new Products(
            reader.GetString(0),
            reader.GetString(1),
            reader.GetDecimal(2),
            reader.GetInt32(3),
            reader.GetString(4)
        );
    }
}