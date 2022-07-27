using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using Homework4.Contracts;
using Homework4.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;

namespace Homework4.Services
{
    public class OrderService : IOrderService
    {
        private readonly string _connectionString;

        public OrderService(IConfiguration configuration)
        {
            _connectionString = configuration.GetSection("ConnectionStrings:OrdersDatabase").Value;
        }

        public async Task Save(Order[] orders)
        {
            const string query =
                "INSERT INTO \"order\"" +
                "SELECT * FROM unnest(@idData, @clientData, @createdData, @deliveredData, @statusData, @storehouseData, @goodsData)";
            
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var cmd = new NpgsqlCommand(query, connection);

            cmd.Parameters.Add(new NpgsqlParameter("idData", orders.Select(x => x.Id).ToArray()));
            cmd.Parameters.Add(new NpgsqlParameter("clientData", orders.Select(x => x.ClientId).ToArray()));
            cmd.Parameters.Add(new NpgsqlParameter("createdData", orders.Select(x => x.CreatedAt).ToArray()));
            cmd.Parameters.Add(new NpgsqlParameter("deliveredData", orders.Select(x => x.DeliveredAt).ToArray()));
            cmd.Parameters.Add(new NpgsqlParameter("statusData", orders.Select(x => (int)x.Status).ToArray()));
            cmd.Parameters.Add(new NpgsqlParameter("storehouseData", orders.Select(x => x.StorehouseId).ToArray()));
            cmd.Parameters.Add(new NpgsqlParameter("goodsData",
                orders.Select(x => JsonSerializer.Serialize(x.Goods)).ToArray())
            {
                // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
                NpgsqlDbType = NpgsqlDbType.Array | NpgsqlDbType.Json
            });

            await cmd.ExecuteNonQueryAsync();
        }

        public async IAsyncEnumerable<Order> Find(
            long storehouseId,
            OrderStatus status,
            DateTime createdFrom,
            DateTime createdTo)
        {
            const string query =
                "SELECT * FROM \"order\" " +
                "WHERE storehouse_id = @storehouseId AND " +
                "status = @status AND " +
                "created_at >= @createdFrom AND " +
                "created_at <= @createdTo";

            var parameters = new DynamicParameters();
            parameters.Add("storehouseId", storehouseId);
            parameters.Add("status", status);
            parameters.Add("createdFrom", createdFrom);
            parameters.Add("createdTo", createdTo);

            await using var connection = new NpgsqlConnection(_connectionString);
            await using var reader = await connection.ExecuteReaderAsync(query, parameters);
            var rowParser = reader.GetRowParser<Order>();

            while (await reader.ReadAsync())
            {
                yield return rowParser(reader);
            }
        }
    }
}