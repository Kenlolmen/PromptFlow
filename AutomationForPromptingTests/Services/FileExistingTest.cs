using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomationForPromptingApi.Exceptions;
using AutomationForPromptingApi.Models;
using AutomationForPromptingApi.Service;
using Shouldly;

namespace AutomationForPromptingTests.Services
{
    public class FileExistingTest
    {
        private readonly string _emptyFilePath;
        private readonly string _accuratePath;
        private readonly string _innacuratePath;

        public FileExistingTest()
        {
            _emptyFilePath = "C:\\Users\\Lenovo\\source\\repos\\AutomationForPromptingApi\\AutomationForPromptingTests\\Services\\TestFiles\\EmptyFile.xlsx";
            _accuratePath = "C:\\Users\\Lenovo\\source\\repos\\AutomationForPromptingApi\\AutomationForPromptingApi\\Input\\SourceFile.xlsx";
            _innacuratePath = "C:\\Users\\Lenovo\\source\\repos\\AutomationForPromptingApi\\AutomationForPromptingApi\\Input\\SourceFile1.xlsx";
        }


        [Fact]
        public void CheckFileExists_FileDoesNotExistsException()
        {
            // Arrange
            var fileModel = new FileModel(_innacuratePath);
            var fileReaderWriter = new FileReaderWriter();

            // Act & Assert
            var exception = Should.Throw<FileDoesNotExistsException>(() => fileReaderWriter.CheckFileExists(fileModel));

            exception.Message.ShouldBe("File does not exists!");
        }


        [Fact]
        public void CheckFileExists_FileShouldExists()
        {
            // Arrange
            var fileModel = new FileModel(_accuratePath);
            var fileReaderWriter = new FileReaderWriter();
            // Act
            var result = fileReaderWriter.CheckFileExists(fileModel);
            // Assert
            result.ShouldBeTrue();
        }


    }
}
