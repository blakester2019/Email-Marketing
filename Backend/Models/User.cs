using System;

namespace Backend.Models;
public class User
{
    public int id { get; set; }
    public string first { get; set; }
    public string last { get; set; }
    public string company { get; set; }
    public string email { get; set; }
    public string password { get; set; }

    // Empty Constructor
    public User()
    {
        id = -1;
        first = "";
        last = "";
        company = "";
        email = "";
        password = "";
    }

    // Main Constructor
    public User(int id, string first, string last, string company, string email, string password)
    {
        this.id = id;
        this.first = first;
        this.last = last;
        this.company = company;
        this.email = email;
        this.password = password;
    }

    // Set id
    public void setId(int id)
    {
        this.id = id;
    }
}
