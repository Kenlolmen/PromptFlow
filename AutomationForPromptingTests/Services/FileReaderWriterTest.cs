using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomationForPromptingApi.Exceptions;
using AutomationForPromptingApi.Models;
using AutomationForPromptingApi.Service;
using Shouldly;
using ClosedXML.Excel;
using AutomationForPromptingTests.Services;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using EmptyFiles;
using Xunit.Sdk;


namespace AutomationForPromptingTests.Services
{
    public class FileReaderWriterTest
    {
        #region Assert

        private readonly string _emptyFilePath;
        private readonly string _nonEmptyFilePath;
        private readonly string _emptyFilePathToSave;
        private readonly FileReaderWriter _fileReaderWriter;
        private readonly List<Dictionary<string, string>> _emptyData;
        public FileReaderWriterTest()
        {
            _emptyFilePath =
               "C:\\Users\\Lenovo\\source\\repos\\AutomationForPromptingApi\\AutomationForPromptingTests\\Services\\TestFiles\\EmptyFile.xlsx";
            _nonEmptyFilePath =
                "C:\\Users\\Lenovo\\source\\repos\\AutomationForPromptingApi\\AutomationForPromptingTests\\Services\\TestFiles\\NonEmptyFile.xlsx";
            _emptyFilePathToSave =
                "C:\\Users\\Lenovo\\source\\repos\\AutomationForPromptingApi\\AutomationForPromptingTests\\Services\\TestFiles\\EmptyFileToSave.xlsx";

            _fileReaderWriter = new FileReaderWriter();

        }

        #endregion


        [Fact]
        public void ReadFile_EmptyFile_ShouldThrowEmptyFileException()
        {
            // Arrange
            var fileModel = new FileModel(_emptyFilePath);

            // Act & Assert
            Should.Throw<EmptyFileException>(() => _fileReaderWriter.ReadFile(fileModel));

        }
        [Fact]
        public void SaveFile_EmptyFile_ShouldThrowEmptyFileException()
        {
            // Arrange
            var fileModel = new FileModel(_emptyFilePath);
            // Act & Assert
            Should.Throw<EmptyFileException>(() => _fileReaderWriter.SaveFile(fileModel, _emptyData));
        }


        //EXCEL FILE:
        /*
         
        1   2   3   4 
        A   J
        B       A
        C           V
        D               
        
        */

        [Fact]
        public void ReadFile_ValidFile_ShouldReturnCollectData()
        {
            var fileModel = new FileModel(_nonEmptyFilePath);
            var expectedData = new List<Dictionary<string, string>>
                {
                    new() { { "1", "A" }, { "2", "J" }, { "3", "" }, { "4", "" } },
                    new() { { "1", "B" }, { "2", "" }, { "3", "A" }, { "4", "" } },
                    new() { { "1", "C" }, { "2", "" }, { "3", "" }, { "4", "V" } },
                    new() { { "1", "D" }, { "2", "" }, { "3", "" }, { "4", "" } }
                };


            // Act
            var result = _fileReaderWriter.ReadFile(fileModel);

            // Assert
            result.ShouldBe(expectedData);
        }


        [Fact]
        public void SaveFile_ValidFile_ShouldSaveData()
        {
            // Arrange
            var fileModel = new FileModel(_emptyFilePathToSave);
            var data = new List<Dictionary<string, string>>
            {
                new() { { "1", "A" }, { "2", "J" }, { "3", "" }, { "4", "" } },
                new() { { "1", "B" }, { "2", "" }, { "3", "A" }, { "4", "" } },
                new() { { "1", "C" }, { "2", "" }, { "3", "" }, { "4", "V" } },
                new() { { "1", "D" }, { "2", "" }, { "3", "" }, { "4", "" } }
            };
            // Act
            _fileReaderWriter.SaveFile(fileModel, data);
            // Assert
            var result = _fileReaderWriter.ReadFile(fileModel);
            result.ShouldBe(data);
        }

        [Fact]
        public void GetKeywords_ValidFile_ShouldReturnKeywords()
        {
            // Arrange
            var fileModel = new FileModel(_nonEmptyFilePath);
            var expectedKeywords = new List<string> { "A", "B", "C", "D" };
            // Act
            var result = _fileReaderWriter.GetKeywords(fileModel);
            // Assert
            result.ShouldBe(expectedKeywords);
        }

        [Fact]
        public void GetKeywords_EmptyFile_ShouldReturnEmptyFileException()
        {
            // Arrange
            var fileModel = new FileModel(_emptyFilePath);
            // Act & Assert
            Should.Throw<EmptyFileException>(() => _fileReaderWriter.GetKeywords(fileModel));
        }
    }

    
}
