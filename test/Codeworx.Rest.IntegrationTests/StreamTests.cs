﻿using System.IO;
using System.Threading.Tasks;
using Codeworx.Rest.Client;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Api.Contract.Model;
using Codeworx.Rest.UnitTests.Data;
using Xunit;

namespace Codeworx.Rest.UnitTests
{
    public class StreamTests : TestServerTestsBase
    {
        private readonly IFileStreamController _controller;
        private readonly string _testFileName;

        public StreamTests()
        {
            _controller = Client<IFileStreamController>(FormatterSelection.Json);
            _testFileName = ItemsGenerator.CreateTestFilename();
        }

        public override void Dispose()
        {
            base.Dispose();
            ItemsGenerator.DeleteFile(_testFileName);
        }

        [Fact()]
        public async Task TestGetStream()
        {
            using (var stream = await _controller.GetFileStream())
            using (var fileStream = File.OpenWrite(_testFileName))
            {
                await stream.CopyToAsync(fileStream);
            }

            var text = File.ReadAllText(_testFileName);
            ItemsGenerator.DeleteFile(_testFileName);
            Assert.Equal(ItemsGenerator.TestFileContent, text);
        }

        [Fact(Skip = "To be implemented")]
        public async Task TestGetStreamItem()
        {
            var streamItem = await _controller.GetStreamItem();
            using (var stream = streamItem.Stream)
            using (var fileStream = File.OpenWrite(_testFileName))
            {
                await stream.CopyToAsync(fileStream);
            }

            var text = File.ReadAllText(_testFileName);
            Assert.Equal(ItemsGenerator.TestFileContent, text);
            Assert.Equal(ItemsGenerator.TestGuid, streamItem.Id);
        }

        [Fact()]
        public async Task TestSwaggerFile()
        {
            var httpClient = ((RestClient<IFileStreamController>)_controller).Options.GetHttpClient();
            var response = await httpClient.GetAsync("swagger/v1/swagger.json");
            var swagger = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact()]
        public async Task TestUpdateFile()
        {
            await ItemsGenerator.CreateTestFile(_testFileName);
            using (var fileStream = File.OpenRead(_testFileName))
            {
                var fileContent = await _controller.UpdateFile(fileStream);
                Assert.Equal(ItemsGenerator.TestFileContent, fileContent);
            }
            ItemsGenerator.DeleteFile(_testFileName);
        }

        [Fact()]
        public async Task TestUpdateFileById()
        {
            await ItemsGenerator.CreateTestFile(_testFileName);
            using (var fileStream = File.OpenRead(_testFileName))
            {
                var fileContent = await _controller.UpdateFileById(fileStream, ItemsGenerator.TestGuid);
                Assert.Equal(ItemsGenerator.TestFileContent, fileContent);
            }
            ItemsGenerator.DeleteFile(_testFileName);
        }

        [Fact(Skip = "To be implemented")]
        public async Task TestUpdateStreamItem()
        {
            await ItemsGenerator.CreateTestFile(_testFileName);
            using (var fileStream = File.OpenRead(_testFileName))
            {
                var streamItem = new StreamItem
                {
                    Stream = fileStream,
                    Id = ItemsGenerator.TestGuid
                };

                var fileContent = await _controller.UpdateStreamItem(streamItem);
                Assert.Equal(ItemsGenerator.TestFileContent, fileContent);
            }
        }
    }
}