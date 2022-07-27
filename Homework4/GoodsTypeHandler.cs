using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using Dapper;
using Homework4.Entities;

namespace Homework4
{
    public class GoodsTypeHandler : SqlMapper.TypeHandler<List<Good>>
    {
        public override void SetValue(IDbDataParameter parameter, List<Good> value)
        {
            parameter.Value = JsonSerializer.Serialize(value);
        }

        public override List<Good> Parse(object value)
        {
            return JsonSerializer.Deserialize<List<Good>>(value.ToString()!);
        }
    }
}