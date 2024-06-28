using System;
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
        public static Task Client_Ready(DiscordClient sender, ReadyEventArgs e)
        {
            sender.UpdateStatusAsync(new DiscordActivity("雷電様の命令に", ActivityType.ListeningTo), UserStatus.DoNotDisturb);

            return Task.CompletedTask;
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
                default:
                    return 1255056967655751790;
            }
        }
    }
}
