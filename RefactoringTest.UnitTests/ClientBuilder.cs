using LegacyApp.Models;

namespace RefactoringTest.UnitTests;

public class ClientBuilder
{
    private Client _client = new Client();
    
    public ClientBuilder WithId(int id)
    {
        _client.Id = id;
        return this;
    }

    public ClientBuilder WithName(string name)
    {
        _client.Name = name;
        return this;
    }

    public ClientBuilder WithClientStatus(ClientStatus clientStatus)
    {
        _client.ClientStatus = clientStatus;
        return this;
    }

    public ClientBuilder WithTestValues()
    {
        _client = new Client()
        {
            Id = 1,
            Name = "Client Test"
        };
        return this;
    }

    public Client Build()
    {
        return _client;
    }
}