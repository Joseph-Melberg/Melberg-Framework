using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Melberg.Common.Exceptions;
using Melberg.Core.InfluxDB;

namespace Melberg.Infrastructure.InfluxDB;
public class DefaultContext
{
    private readonly WriteApiAsync WriteApi;
    private readonly IInfluxDBConfigurationProvider _configurationProvider;
    public DefaultContext(IInfluxDBConfigurationProvider configurationProvider)
    {
        _configurationProvider = configurationProvider;

        var connectionString = configurationProvider.GetConnectionString(this.GetType().Name)
        ?? throw new MissingConnectionStringException($"Unable to find connection string: {this.GetType().Name}");

        WriteApi = InfluxDBClientFactory.Create(connectionString).GetWriteApiAsync();
    }

    public async Task WritePointAsync(InfluxDBDataModel model, string bucket, string org_id)
    {
        model.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var point = ToPointModel(model);
        await WriteApi.WritePointAsync( bucket, org_id, point);
    }

    public PointData ToPointModel(InfluxDBDataModel model)
    {
        if(!model.Tags.Any())
        {
            throw new InvalidInfluxDBDataModelException("Tags were not given.");
        }
        if(!model.Fields.Any())
        {
            throw new InvalidInfluxDBDataModelException("Fields were not given");
        }
        if(string.IsNullOrEmpty(model.Measurement))
        {
            throw new InvalidInfluxDBDataModelException("Measurement not given");
        }


        var point = PointData.Measurement(model.Measurement);
        foreach(var item in model.Tags)
        {
            point = point.Tag(item.Key,item.Value);
        }
        foreach(var item in model.Fields)
        {
            point = FieldCleaner(point, item.Key,item.Value);
        }
        
        if(model.Timestamp != 0)
        {
            point = point.Timestamp(model.Timestamp,WritePrecision.S);
        }
        return point;
    }

    private PointData FieldCleaner(PointData point,string key, object obj)
    {
        if(obj is int) { return point.Field(key,(int)obj); }
        if(obj is uint) { return point.Field(key,(uint)obj); }
        if(obj is double) { return point.Field(key,(double)obj); }
        if(obj is long) { return point.Field(key,(long)obj); }
        if(obj is ulong) { return point.Field(key,(ulong)obj); }
        //if(obj is string) { return point.Field(key,"\"" + (string)obj+ "\""); }
        if(obj is string) throw new InvalidInfluxDBFieldValueException("string is not a valid type yet");
        if(obj is float) { return point.Field(key,(float)obj); }
        if(obj is decimal) { return point.Field(key,(decimal)obj);}
        if(obj is byte) { return point.Field(key, (byte)obj);}
        throw new InvalidInfluxDBFieldValueException($"Field {key} had an invalid type of {obj.GetType().Name}");
    }
    private string ListStringBuilder(IEnumerable<(string,object)> valuePairs, bool isTag) => string.Join(",",valuePairs.Select(_ => KeyValueToString(_.Item1, _.Item2, isTag)));

    private string KeyValueToString(string key, object value, bool isTag) => $"{key}={StringEscaper(value, isTag)}"; 

    private string StringEscaper(object value, bool isTag)
    {
        if(value is string)
        {
            return "{value}".Replace("{value}",((!isTag) ? "\"" : "") + value.ToString() + ((!isTag) ? "\"" : ""));
        }
        return value.ToString();
    }
}