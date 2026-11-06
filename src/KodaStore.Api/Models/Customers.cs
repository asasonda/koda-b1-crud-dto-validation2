namespace KodaStore.Api.Models;

public sealed class Customers
{
    public string CustomerCode {get; set;}
    public string Name {get; set;}
    public string Email {get; set;}
    public string Phone {get; set;}
    public string Address {get; set;}

    public Customers(string customerCode, string name, string email, string phone, string address)
    {
        if (string.IsNullOrWhiteSpace(customerCode))
        {
            throw new ArgumentException("Customer is required", nameof(customerCode));
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("name is required", nameof(name));
        }
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("email is required", nameof(email));
        }
        if (string.IsNullOrWhiteSpace(phone))
        {
            throw new ArgumentException("phone is required", nameof(phone));
        }
        if (string.IsNullOrWhiteSpace(address))
        {
            throw new ArgumentException("address is required", nameof(address));
        }

        CustomerCode = customerCode;
        Name = name;
        Email = email;
        Phone = phone;
        Address = address;
    }
}