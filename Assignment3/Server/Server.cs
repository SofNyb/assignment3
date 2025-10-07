using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Assignment3.Models;
using Assignment3.Services;
using Assignment3.Utils;

namespace Assignment3.Server
{
    public class ServerMain
    {
        static void Main()
        {
            var server = new EchoServer(5000);
            server.Run();
        }
    }

    public class EchoServer
    {
        private TcpListener _server;
        private CategoryService _service = new CategoryService();
        private RequestValidator _validator = new RequestValidator();
        
        public int Port { get; set; }

        public EchoServer(int port)
        {
            Port = port;

        }

        public void Run()
        {
            _server = new TcpListener(IPAddress.Loopback, Port);
            _server.Start();
            Console.WriteLine($"Server started on port {Port}");

            while (true)
            {
                TcpClient client = _server.AcceptTcpClient();
                Console.WriteLine("Client connected");
                Task.Run(() => HandleClient(client));
            }
        }

        private void HandleClient(TcpClient client)
        {
            var stream = client.GetStream();
            Response resp = new Response { Status = "", Body = null }; // Always initialize
            try
            {
                // Læs fra klienten
                byte[] buffer = new byte[4096];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) return;

                string requestStr = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                // Parse JSON
                Request req;
                try
                {
                    var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                    req = JsonSerializer.Deserialize<Request>(requestStr, options);
                    if (req == null)
                    {
                        resp.Status = "4 illegal body";
                        SendResponse(stream, resp);
                        return;
                    }
                }
                catch
                {
                    resp.Status = "4 illegal body";
                    SendResponse(stream, resp);
                    return;
                }

                // Validate request
                resp = _validator.ValidateRequest(req);
                if (resp.Status != "1 Ok")
                {
                    SendResponse(stream, resp);
                    return;
                }

                // Handle echo method separately (doesn't need path validation)
                if (req.Method.ToLower() == "echo")
                {
                    resp.Status = "1 Ok";
                    resp.Body = req.Body;
                    SendResponse(stream, resp);
                    return;
                }

                var parser = new UrlParser();
                parser.ParseUrl(req.Path);

                // Check if path starts with /api/categories
                if (!parser.Path.StartsWith("/api/categories"))
                {
                    resp.Status = "5 Not Found";
                    SendResponse(stream, resp);
                    return;
                }

                // If path has extra segments that aren't a valid ID, return bad request
                if (parser.Path != "/api/categories" && !parser.HasId)
                {
                    resp.Status = "4 Bad Request";
                    SendResponse(stream, resp);
                    return;
                }

                switch (req.Method.ToLower())
                {
                    case "read":
                        if (parser.HasId)
                        {
                            if (int.TryParse(parser.Id, out int id))
                            {
                                var cat = _service.GetCategory(id);
                                if (cat == null)
                                    resp.Status = "5 Not Found";
                                else
                                {
                                    resp.Status = "1 Ok";
                                    var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                                    resp.Body = JsonSerializer.Serialize(cat, options);
                                }
                            }
                            else
                            {
                                resp.Status = "4 Bad Request";
                            }
                        }
                        else
                        {
                            resp.Status = "1 Ok";
                            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                            resp.Body = JsonSerializer.Serialize(_service.GetCategories(), options);
                        }
                        break;

                    case "create":
                        if (parser.HasId)
                        {
                            resp.Status = "4 Bad Request";
                            resp.Body = null;
                        }
                        else if (string.IsNullOrWhiteSpace(req.Body))
                        {
                            resp.Status = "4 illegal body";
                            resp.Body = null;
                        }
                        else
                        {
                            try
                            {
                                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                                // Parse the request body to get the name
                                var requestData = JsonSerializer.Deserialize<dynamic>(req.Body, options);
                                var nameProperty = ((JsonElement)requestData).GetProperty("name");
                                string name = nameProperty.GetString();
                                
                                // Create category with auto-generated ID
                                var createdCategory = _service.CreateCategory(name);
                                resp.Status = "2 Created";
                                resp.Body = JsonSerializer.Serialize(createdCategory, options);
                            }
                            catch
                            {
                                resp.Status = "4 illegal body";
                                resp.Body = null;
                            }
                        }
                        break;

                    case "update":
                        if (!parser.HasId)
                        {
                            resp.Status = "4 Bad Request";
                            resp.Body = null;
                        }
                        else if (!int.TryParse(parser.Id, out int updateId))
                        {
                            resp.Status = "4 Bad Request";
                            resp.Body = null;
                        }
                        else if (string.IsNullOrWhiteSpace(req.Body))
                        {
                            resp.Status = "4 illegal body";
                            resp.Body = null;
                        }
                        else
                        {
                            try
                            {
                                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                                var cat = JsonSerializer.Deserialize<Category>(req.Body, options);
                                bool ok = _service.UpdateCategory(updateId, cat.Name);
                                if (ok)
                                {
                                    // Return the updated category
                                    var updatedCategory = _service.GetCategory(updateId);
                                    resp.Status = "3 Updated";
                                    resp.Body = JsonSerializer.Serialize(updatedCategory, options);
                                }
                                else
                                {
                                    resp.Status = "5 Not Found";
                                    resp.Body = null;
                                }
                            }
                            catch
                            {
                                resp.Status = "4 illegal body";
                                resp.Body = null;
                            }
                        }
                        break;

                    case "delete":
                        if (!parser.HasId)
                        {
                            resp.Status = "4 Bad Request";
                            resp.Body = null;
                        }
                        else if (!int.TryParse(parser.Id, out int deleteId))
                        {
                            resp.Status = "4 Bad Request";
                            resp.Body = null;
                        }
                        else
                        {
                            bool ok = _service.DeleteCategory(deleteId);
                            resp.Status = ok ? "1 Ok" : "5 Not Found";
                            resp.Body = null;
                        }
                        break;

                    default:
                        resp.Status = "4 Illegal Method";
                        resp.Body = null;
                        break;
                }

                SendResponse(stream, resp);
            }
            finally
            {
                stream.Close();
                client.Close();
            }
        }


        private void SendResponse(NetworkStream stream, Response resp)
        {
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            string respStr = JsonSerializer.Serialize(resp, options);
            byte[] data = Encoding.UTF8.GetBytes(respStr);
            stream.Write(data, 0, data.Length);
        }


    }
}
