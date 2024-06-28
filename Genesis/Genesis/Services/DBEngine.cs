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
using Genesis.Genesis.Helpers;
using System.Diagnostics;
using System.Windows;

namespace Genesis.Genesis.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class DBEngine
    {
        public static DiscordClient? Client { get; set; }
        public static CommandsNextExtension? Commands { get; set; }
        
        /// <summary>
        /// Initializes the Discord client with configuration settings, event handlers, and command settings.
        /// Connects the client to Discord and keeps it running indefinitely.
        /// </summary>
        public static async Task Initialize() 
        {
            var jsonReader = new JSONReader();
            await jsonReader.ReadJSON();

            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = jsonReader.token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
            };

            Client = new DiscordClient(discordConfig);


            Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(2)
            });


            Client.Ready += DBEngineSupport.Client_Ready;
            Client.MessageCreated += DBEngineSupport.Message_Created;

            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { jsonReader.prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = true,
                DmHelp = false,
                IgnoreExtraArguments = true,
            };

            Commands = Client.UseCommandsNext(commandsConfig);


            await Client.ConnectAsync();

            await Task.Delay(-1);
        }

        /// <summary>
        /// Handles a request asynchronously by serializing the request object, posting it to a destination channel,
        /// and awaiting a response.
        /// </summary>
        /// <param name="request">The request object to handle.</param>
        /// <param name="requestType">The type of the request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a tuple indicating success and the response object.</returns>
        public static async Task<(bool, ResponseObject?)> HandleRequestAsync(RequestObject request, RequestType requestType)
        {
            var Destination = await DBEngineSupport.GetDestinationAsync(requestType);

            var serializedRequest = await DBEngineSupport.SerializeRequestObjectAsync(request);

            var Response = await PostRequestAsync(serializedRequest, Destination);

            return Response == null ? (false, null) : (true, Response);
        }

        /// <summary>
        /// Posts a serialized request to a specified Discord channel asynchronously and waits for a response.
        /// </summary>
        /// <param name="request">The serialized request string.</param>
        /// <param name="discordChannel">The Discord channel to post the request to.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response object or null if the operation fails.</returns>
        private static async Task<ResponseObject?> PostRequestAsync(string request, DiscordChannel discordChannel)
        {
            await discordChannel.SendMessageAsync($"{request}");

            return await WaitForResponse(discordChannel);
        }

        /// <summary>
        /// Waits for a response message in a specified Discord channel asynchronously.
        /// </summary>
        /// <param name="channel">The Discord channel to wait for a response in.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response object or null if the operation fails or times out.</returns>
        private static async Task<ResponseObject?> WaitForResponse(DiscordChannel channel)
        {
            var interactivity = Client.GetInteractivity();

            var waitTime = TimeSpan.FromSeconds(5);

            var messageResult = await interactivity.WaitForMessageAsync(
                       x => x.Channel.Id == channel.Id,
                       waitTime
                   );

            if (!messageResult.TimedOut)
            {
                Debug.WriteLine(messageResult.Result.Content);

                try
                {
                    var responseObject = await DBEngineSupport.DeserializeRequestObjectAsync(messageResult.Result.Content);

                    await messageResult.Result.CreateReactionAsync(DiscordEmoji.FromName(Client, ":white_check_mark:"));

                    return responseObject;
                }catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
