﻿using OpenAI_API;
using OpenAI_API.Models;
using static OpenAI_API.Chat.ChatMessage;

namespace CC_Backend.Services
{
    public class OpenAIService : IOpenAIService
    {
        private string _apiKey;

        public OpenAIService(string apiKey)
        {
            _apiKey = apiKey;
        }

        // Read an image with a prompt and get a response from GPT4-Vision
        public async Task<string> ReadImage(string prompt)
        {
            var api = new OpenAI_API.OpenAIAPI(_apiKey);
            var chat = api.Chat.CreateConversation();
            chat.Model = Model.GPT4_Vision;
            chat.AppendSystemMessage("Your job is to answer whether or not an object appears in the picture or not. Your answer should only be a numbers between 1-100, where 1 means the object is not in the picture and 100 means the object is definitely in the picture.");


            // Testbart genom att bara byta ut filepathen till annan bild.
            chat.AppendUserInput($"Does {prompt} appear in the image?", ImageInput.FromFile("C:\\Users\\Hjalm\\Downloads\\oak.jpg"));
            var response = await chat.GetResponseFromChatbotAsync();

            // !! Tar bort sålange för vi fattar inte hur man lagger in en byte array. !!
            // --> chat.AppendUserInput($"Does {prompt} appear in the image?", ImageInput.FromImageBytes(bytes)); <--

            return response;
        }
    }
}
