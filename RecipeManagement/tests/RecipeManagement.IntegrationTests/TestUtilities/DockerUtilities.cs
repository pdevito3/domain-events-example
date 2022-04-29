// based on https://blog.dangl.me/archive/running-sql-server-integration-tests-in-net-core-projects-via-docker/

namespace RecipeManagement.IntegrationTests.TestUtilities;

using Docker.DotNet;
using Docker.DotNet.Models;
using Npgsql;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

public static class DockerUtilities
{
    private static bool IsRunningOnWindows()
    {
        return Environment.OSVersion.Platform == PlatformID.Win32NT;
    }

    public static DockerClient GetDockerClient()
    {
        var dockerUri = IsRunningOnWindows()
            ? "npipe://./pipe/docker_engine"
            : "unix:///var/run/docker.sock";
        return new DockerClientConfiguration(new Uri(dockerUri))
            .CreateClient();
    }

    public static async Task CleanupRunningContainers( string containerName, int hoursTillExpiration = -24)
    {
        var dockerClient = GetDockerClient();

        var runningContainers = await dockerClient.Containers
            .ListContainersAsync(new ContainersListParameters());

        foreach (var runningContainer in runningContainers.Where(cont => cont.Names.Any(n => n.Contains(containerName))))
        {
            // Stopping all test containers that are older than 24 hours
            var expiration = hoursTillExpiration > 0
                ? hoursTillExpiration * -1
                : hoursTillExpiration;
            if (runningContainer.Created < DateTime.UtcNow.AddHours(expiration))
            {
                try
                {
                    await EnsureDockerContainersStoppedAndRemovedAsync(runningContainer.ID);
                }
                catch
                {
                    // Ignoring failures to stop running containers
                }
            }
        }
    }

    public static async Task CleanupRunningVolumes(string volumeName, int hoursTillExpiration = -24)
    {
        var dockerClient = GetDockerClient();

        var runningVolumes = await dockerClient.Volumes.ListAsync();

        foreach (var runningVolume in runningVolumes.Volumes.Where(v => v.Name == volumeName))
        {
            // Stopping all test volumes that are older than 24 hours
            var expiration = hoursTillExpiration > 0
                ? hoursTillExpiration * -1
                : hoursTillExpiration;
            if (DateTime.Parse(runningVolume.CreatedAt) < DateTime.UtcNow.AddHours(expiration))
            {
                try
                {
                    await EnsureDockerVolumesRemovedAsync(runningVolume.Name);
                }
                catch
                {
                    // Ignoring failures to stop running containers
                }
            }
        }
    }

    public static async Task EnsureDockerContainersStoppedAndRemovedAsync(string dockerContainerId)
    {
        var dockerClient = GetDockerClient();
        await dockerClient.Containers
            .StopContainerAsync(dockerContainerId, new ContainerStopParameters());
        await dockerClient.Containers
            .RemoveContainerAsync(dockerContainerId, new ContainerRemoveParameters());
    }

    public static async Task EnsureDockerVolumesRemovedAsync(string volumeName)
    {
        var dockerClient = GetDockerClient();
        await dockerClient.Volumes.RemoveAsync(volumeName);
    }

    public static async Task WaitUntilDatabaseAvailableAsync(string dbConnection)
    {
        var start = DateTime.UtcNow;
        const int maxWaitTimeSeconds = 60;
        var connectionEstablished = false;
        while (!connectionEstablished && start.AddSeconds(maxWaitTimeSeconds) > DateTime.UtcNow)
        {
            try
            {
                using var sqlConnection = new NpgsqlConnection(dbConnection);
                await sqlConnection.OpenAsync();
                connectionEstablished = true;
            }
            catch
            {
                // If opening the SQL connection fails, SQL Server is not ready yet
                await Task.Delay(500);
            }
        }

        if (!connectionEstablished)
        {
            throw new Exception($"Connection to the SQL docker database could not be established within {maxWaitTimeSeconds} seconds.");
        }

        return;
    }

    public static int GetFreePort()
    {
        // From https://stackoverflow.com/a/150974/4190785
        var tcpListener = new TcpListener(IPAddress.Loopback, 0);
        tcpListener.Start();
        var port = ((IPEndPoint)tcpListener.LocalEndpoint).Port;
        tcpListener.Stop();
        return port;
    }

    public static string GetSqlConnectionString(string port, string password, string user, string dbName)
    {
        return new NpgsqlConnectionStringBuilder()
        {
            Host = "localhost",
            Password = password,
            Username = user,
            Database = dbName,
            Port = int.Parse(port)
        }.ToString();
    }
}