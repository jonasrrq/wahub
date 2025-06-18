using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaHub.Shared.Models;

public class Response<T> where T : class
{
    public T? Data { get; set; }
    public string? Message { get; set; }
    public bool Success { get; set; } = true;
    public int Code { get; set; } = 200;
}
