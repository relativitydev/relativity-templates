using System.Collections.Generic;
using System.Linq;
using kCura.Relativity.Client.DTOs;
using NUnit.Framework;
using Relativity_Extension.Helpers.Rsapi;

namespace Relativity_Extension.Helpers.NUnit
{
    [TestFixture]
    public class ResponseTests
    {
        [Test]
        public void CompileWriteResult_ReceivesFailedSet_ReturnsFormattedMessageWithResults()
        {
            // Arrange
            WriteResultSet<RDO> results = new WriteResultSet<RDO>
            {
                Message = "This is my global message",
                Success = false,
                Results = new List<Result<RDO>>
                {
                    new Result<RDO>
                    {
                        Artifact = new RDO(),
                        Message = "This is a test message",
                        Success = true
                    },
                    new Result<RDO>
                    {
                        Artifact = new RDO(),
                        Message = "This is another test message",
                        Success = false
                    }
                }
            };

            // Act
            Response<IEnumerable<RDO>> actual = Response<int>.CompileWriteResults(results);

            // Assert
            Assert.IsFalse(actual.Success);
            Assert.AreEqual("This is my global messageThis is a test messageThis is another test message", actual.Message);
            Assert.Greater(actual.Results.Count(), 0);
        }

        [Test]
        public void CompileWriteResult_ReceivesSuccessfulSet_ReturnsParsedResults()
        {
            // Arrange
            WriteResultSet<RDO> results = new WriteResultSet<RDO>
            {
                Message = "This is my global message",
                Success = true,
                Results = new List<Result<RDO>>
                {
                    new Result<RDO>
                    {
                        Artifact = new RDO(),
                        Message = "This is a test message",
                        Success = true
                    }
                }
            };

            // Act
            Response<IEnumerable<RDO>> actual = Response<int>.CompileWriteResults(results);

            // Assert
            Assert.IsTrue(actual.Success);
            Assert.AreEqual(string.Empty, actual.Message);
            Assert.Greater(actual.Results.Count(), 0);
        }
    }
}