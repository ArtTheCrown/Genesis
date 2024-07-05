using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.CommandsNext.Attributes;
using Genesis.Genesis.Services;
using Newtonsoft.Json;
using System.Windows;
using System.Net.Http;

namespace Genesis.Genesis.Helpers
{
    /// <summary>
    /// Provides support functions for the DBEngine class, including serialization, deserialization,
    /// event handling for Discord messages and client readiness, and retrieving destination channels.
    /// </summary>
    public static class DBEngineSupport
    {
        /// <summary>
        /// Serializes a RequestObject into a JSON string asynchronously.
        /// </summary>
        /// <param name="requestObject">The object to serialize.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the JSON string.</returns>
        public static async Task<string> SerializeRequestObjectAsync(RequestObject requestObject)
        {
            return JsonConvert.SerializeObject(requestObject);
        }

        /// <summary>
        /// Deserializes a JSON string into a ResponseObject asynchronously.
        /// </summary>
        /// <param name="responseObject">The JSON string to deserialize.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized ResponseObject or null if the operation fails.</returns>
        public static async Task<ResponseObject?> DeserializeRequestObjectAsync(string responseObject)
        {
            return JsonConvert.DeserializeObject<ResponseObject>(responseObject);
        }

        /// <summary>
        /// Handles the event when a message is created in Discord. Ignores messages from bots.
        /// </summary>
        /// <param name="sender">The Discord client that sent the event.</param>
        /// <param name="e">The event arguments containing message details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static Task Message_Created(DiscordClient sender, MessageCreateEventArgs e)
        {
            if (!e.Author.IsBot)
            {
                // Handle non-bot messages here.
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Handles the event when the Discord client is ready. Updates the client's status and handles a debug request.
        /// </summary>
        /// <param name="sender">The Discord client that sent the event.</param>
        /// <param name="e">The event arguments containing readiness details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task Client_Ready(DiscordClient sender, ReadyEventArgs e)
        {
            await sender.UpdateStatusAsync(new DiscordActivity("雷電様の命令に", ActivityType.ListeningTo), UserStatus.DoNotDisturb);

            var channel = await sender.GetChannelAsync(1258765765008556092); // Replace with your channel ID

            // Path to the image file in the runtime directory
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "image.jpg");

            // Create the message builder and attach the image
            var message = new DiscordMessageBuilder();

            using (var fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                message.AddFile("image.jpg", fs);

                // Send the message
                var sentMessage = await message.SendAsync(channel);

                // Get the URL of the attached image
                var attachmentUrl = sentMessage.Attachments[0].Url;

                // Send the URL in the next message
                var urlMessage = new DiscordMessageBuilder()
                    .WithContent($"```\n{attachmentUrl}\n```");

                //await urlMessage.SendAsync(channel);
            }
        }

        /// <summary>
        /// Retrieves the destination Discord channel based on the request type asynchronously.
        /// </summary>
        /// <param name="requestType">The type of the request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the destination Discord channel.</returns>
        public static async Task<DiscordChannel> GetDestinationAsync(RequestType requestType)
        {
            ulong Address = await GetRequestChannelAsync(requestType);

            var Destination = await DBEngine.Client.GetChannelAsync(Address);

            return Destination;
        }

        /// <summary>
        /// Retrieves the channel ID for a specific request type asynchronously.
        /// </summary>
        /// <param name="requestType">The type of the request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the channel ID.</returns>
        private static async Task<ulong> GetRequestChannelAsync(RequestType requestType)
        {
            switch (requestType)
            {
                case RequestType.System:
                    return 1255786057832599583;
                case RequestType.Request:
                    return 1255779753294561281;
                case RequestType.Authentication:
                    return 1255779696432644116;
                case RequestType.Registration:
                    return 1255779910027444224;
                case RequestType.Communication:
                    return 1255786428412068002;
                case RequestType.Kaizen:
                    return 1255785863699501097;
                case RequestType.Report:
                    return 1255813320976498730;
                case RequestType.Debug:
                    return 1255056967655751790;
                case RequestType.Files:
                    return 1258765765008556092;
                default:
                    return 1255056967655751790;
            }
        }

        public static async Task<string> DownloadAttachmentAsync(string attachmentUrl)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(attachmentUrl);
            var imageBytes = await response.Content.ReadAsByteArrayAsync();

            var fileName = Path.GetFileName(attachmentUrl);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"Data\Assets\downloaded_.png");
            await File.WriteAllBytesAsync(filePath, imageBytes);

            return filePath;
        }

        public static async Task<bool> DownloadFileAsync(string fileUrl, string fileName)
        {
            string tempDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp");
            Directory.CreateDirectory(tempDirectory); // Ensure the directory exists
            string destinationPath = Path.Combine(tempDirectory, fileName);

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Add headers to mimic a browser request
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
                    client.DefaultRequestHeaders.Referrer = new Uri("https://discord.com"); // Set the referer to a valid Discord URL

                    // Send request and get response
                    Console.WriteLine($"Attempting to download file from URL: {fileUrl}");
                    HttpResponseMessage response = await client.GetAsync(fileUrl);
                    response.EnsureSuccessStatusCode(); // Throw on error code.

                    // Read content as byte array and save to file
                    byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
                    File.WriteAllBytes(destinationPath, fileBytes);

                    Console.WriteLine($"File downloaded successfully to: {destinationPath}");
                    return true;
                }
                catch (HttpRequestException httpEx)
                {
                    Console.WriteLine($"HttpRequestException: {httpEx.Message}");
                    if (httpEx.InnerException != null)
                    {
                        Console.WriteLine($"InnerException: {httpEx.InnerException.Message}");
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error downloading file: {ex.Message}");
                    return false;
                }
            }
        }
    }
}
