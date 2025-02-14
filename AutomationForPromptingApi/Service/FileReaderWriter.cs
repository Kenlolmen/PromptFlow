using AutomationForPromptingApi.Models;
using OfficeOpenXml;
using System;
using AutomationForPromptingApi.Exceptions;
using System.Collections.Generic;

namespace AutomationForPromptingApi.Service
{
    public class FileReaderWriter : IFileReaderWriter, IFileService
    {

        public bool CheckFileExists(FileModel fileModel)
        {
            if (!File.Exists(fileModel.Path))
            {
                throw new FileDoesNotExistsException();
            }

            return true;
        }

        public List<string> LoadKeywords(FileModel fileModel)
        {
            CheckFileExists(fileModel);

            var keywords = new List<string>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {
                using (var package = new ExcelPackage(new FileInfo(fileModel.Path)))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    if (worksheet.Dimension == null)
                    {
                        throw new EmptyFileException();
                    }

                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)  
                    {
                        var keyword = worksheet.Cells[row, 1].Text.Trim();  
                        if (!string.IsNullOrWhiteSpace(keyword))
                        {
                            keywords.Add(keyword);  
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new EmptyFileException();  
            }

            return keywords;
        }

        public List<Dictionary<string, string>> ReadFile(FileModel fileModel)
        {
            CheckFileExists(fileModel);

            var data = new List<Dictionary<string, string>>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {
                using (var package = new ExcelPackage(new FileInfo(fileModel.Path)))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    if (worksheet.Dimension == null)
                    {
                        throw new EmptyFileException();
                    }

                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    var headers = new List<string>();
                    for (int col = 1; col <= colCount; col++)
                    {
                        headers.Add(worksheet.Cells[1, col].Text.Trim());
                    }
                    if (headers.TrueForAll(string.IsNullOrWhiteSpace))
                    {
                        throw new EmptyFileException();
                    }

                    for (int row = 2; row <= rowCount; row++) 
                    {
                        var rowData = new Dictionary<string, string>();
                        for (int col = 1; col <= colCount; col++)
                        {
                            var header = headers[col - 1];
                            if (!string.IsNullOrWhiteSpace(header)) 
                            {
                                rowData[header] = worksheet.Cells[row, col].Text.Trim();
                            }
                        }
                        data.Add(rowData);
                    }
                }
            }
            catch 
            {
                throw new EmptyFileException();
            }

            return data;
        }

        public void SaveFile(FileModel fileModel, List<Dictionary<string, string>> data)
        {


            if (data == null || data.Count == 0)
                throw new EmptyFileException();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                var headers = data.First().Keys.ToList();

                for (int col = 0; col < headers.Count; col++)
                {
                    worksheet.Cells[1, col + 1].Value = headers[col];
                }

                for (int row = 0; row < data.Count; row++)
                {
                    var rowData = data[row];
                    for (int col = 0; col < headers.Count; col++)
                    {
                        worksheet.Cells[row + 2, col + 1].Value = rowData[headers[col]];
                    }
                }

                
                var fileInfo = new FileInfo(fileModel.Path);
                package.SaveAs(fileInfo);
            }
        }
    }
}
