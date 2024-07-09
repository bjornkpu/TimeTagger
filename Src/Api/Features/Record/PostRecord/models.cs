namespace Api.Features.Record.PostRecord;

public class Request
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
}

public class Response
{
    public string FullName { get; set; }
    public bool IsOver18 { get; set; }
}
