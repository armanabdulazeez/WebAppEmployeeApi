using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace WebAppEmployeeApi.Middlewares
{
    public class RequestLogging
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLogging> _logger;
        private readonly string _connectionString;

        public RequestLogging(RequestDelegate next, ILogger<RequestLogging> logger, IConfiguration config)
        {
            _next = next;
            _logger = logger;
            _connectionString = config.GetConnectionString("DefaultConnection");

            if (!Directory.Exists("Logs"))
                Directory.CreateDirectory("Logs");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var request = context.Request;

            string method = request.Method;
            string path = request.Path;
            string query = request.QueryString.HasValue ? request.QueryString.Value : string.Empty;
            string fullpath = path + query;
            string reqTime = DateTime.Now.ToString();

            await _next(context);

            stopwatch.Stop();

            int statusCode = context.Response.StatusCode;
            var logData = $"[{reqTime} Request: {method} {path}{query}\n[{DateTime.Now}] Response: {statusCode} (Time: {stopwatch.ElapsedMilliseconds} ms)";

            await File.AppendAllTextAsync("Logs/log.txt", logData + "\n\n");
            _logger.LogInformation("{ResponseInfo}\n", logData);

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    //string sql = $"INSERT INTO RequestLogs (Incoming, Method, Path,Outgoing, StatusCode, Duration) VALUES ('{reqTime}', '{method}', '{fullpath}', '{DateTime.Now}', {statusCode}, {stopwatch.ElapsedMilliseconds})";
                    string sql = @"INSERT INTO RequestLogs (Incoming, Method, Path,Outgoing, StatusCode, Duration) VALUES (@ReqTime, @Method, @FullPath, @ResTime, @StatusCode, @Duration)";

                    using var command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@ReqTime", reqTime);
                    command.Parameters.AddWithValue("@Method", method);
                    command.Parameters.AddWithValue("@FullPath", fullpath);
                    command.Parameters.AddWithValue("@ResTime", DateTime.Now);
                    command.Parameters.AddWithValue("@StatusCode", statusCode);
                    command.Parameters.AddWithValue("@Duration", stopwatch.ElapsedMilliseconds);
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to log to database.");
            }
        }
    }
}
