using System.Collections.Generic;
using System.Linq;
using System.Text;
using MelbergFramework.Common.Exceptions;

namespace MelbergFramework.Infrastructure.InfluxDB;

public class InfluxDBDataModel
{
    public IDictionary<string,string> Tags {get; set;}
    public IDictionary<string,object> Fields {get; set;}
    public string Measurement {get; set;}
    public long Timestamp {get; set;}

    public InfluxDBDataModel(string measurement)
    {
        Tags = new Dictionary<string,string>();
        Fields = new Dictionary<string,object>();
        Measurement = measurement;
    }
}