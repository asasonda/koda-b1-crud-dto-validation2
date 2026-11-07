using System.Data;
using KodaStore.Api.DTOs;
using KodaStore.Api.Models;
using Microsoft.Data.SqlClient;

namespace KodaStore.Api.Repositories;

public sealed class SqlCustomerRepository : ICustomerRepository
{
    private readonly string _connectionString;
    public SqlCustomerRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("eccomercedb") ?? throw new InvalidOperationException("Gagal connection");
    }
    public async Task<bool> AddCustomersAsync(Customers customers, CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO dbo.Customers(CustomerCode, Name, Email, Phone, Address) VALUES (@customerCode, @name, @email, @phone, @address)
        """;
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql,connection);
        AddCustomerParameter(command, customers);
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

    public async Task<bool> DeleteCustomerAsync(string customerCode, CancellationToken cancellationToken)
    {
        const string sql = """
            DELETE FROM dbo.Customers WHERE
            CustomerCode = @customerCode;
        """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        AddCustomerCode(command, customerCode);
        var affectedRows = await command.ExecuteNonQueryAsync(cancellationToken);
        return affectedRows == 1;
    }

    public async Task<IReadOnlyList<Customers>> GetAllAsync(CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT CustomerCode, Name, Email, Phone, Address 
            FROM dbo.Customers
        """;

        var customers = new List<Customers>();
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while(await reader.ReadAsync(cancellationToken))
        {
            customers.Add(ReadCustomers(reader));
        }
        return customers;
    }

    public async Task<Customers?> GetByCodeAsync(string customerCode, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT CustomerCode, Name, Email, Phone, Address from dbo.Customers WHERE CustomerCode = @customerCode
        """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);

        AddCustomerCode(command, customerCode);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if(!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }
        return ReadCustomers(reader);
    }

    public async Task<bool> UpdateCustomersAsync(Customers customers, CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE dbo.Customers Set 
                    Name = @name,
                    Email = @email,
                    Phone = @phone,
                    Address = @address
                WHERE CustomerCode = @customerCode
        """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        AddCustomerParameter(command, customers);
        
        var affectedRows = await command.ExecuteNonQueryAsync(cancellationToken);
        return affectedRows == 1;
    }


    private static void AddCustomerParameter(SqlCommand command, Customers customers)
    {
        AddCustomerCode(command, customers.CustomerCode);
        command.Parameters.Add("@name", SqlDbType.NVarChar, 100).Value = customers.Name;
        command.Parameters.Add("@email", SqlDbType.NVarChar, 100).Value = customers.Email;
        command.Parameters.Add("@phone", SqlDbType.NVarChar, 30).Value = customers.Phone;
        command.Parameters.Add("@address", SqlDbType.NVarChar, 255).Value = customers.Address;
    }

    private static void AddCustomerCode(SqlCommand command, string customerCode)
    {
        command.Parameters.Add("@customerCode", SqlDbType.NVarChar, 20).Value = customerCode.Trim().ToUpperInvariant();
    }

    private static Customers ReadCustomers(SqlDataReader reader)
    {
        return new Customers(
            reader.GetString(0),
            reader.GetString(1),
            reader.GetString(2),
            reader.GetString(3),
            reader.GetString(4)
        );
    }
}