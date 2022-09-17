using System.Text;
using Newtonsoft.Json;

namespace Blog.Tests;

public static class Extensions
{
    public static StringContent ToStringContent(this object obj)
    {
        var serializedObject = JsonConvert.SerializeObject(obj);
        return new StringContent(serializedObject, Encoding.UTF8, "application/json");
    }
}
