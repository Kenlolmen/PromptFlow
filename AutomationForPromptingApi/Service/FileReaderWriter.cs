using AutomationForPromptingApi.Models;
using ClosedXML.Excel;
using System;
using AutomationForPromptingApi.Exceptions;
using System.Collections.Generic;

namespace AutomationForPromptingApi.Service
{
    public class FileReaderWriter : IFileReaderWriter
    {

        public bool CheckFileExists(FileModel fileModel)
        {
            if (!File.Exists(fileModel.Path))
            {
                throw new FileDoesNotExistsException();
            }

            return true;
        }

        public List<Dictionary<string, string>> ReadFile(FileModel fileModel)
        {

            CheckFileExists(fileModel);

            var data = new List<Dictionary<string, string>>();

            using (var workbook = new XLWorkbook(fileModel.Path))
            {
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RowsUsed();
                var headerRow = rows.First(); 
                var headers = headerRow.Cells().Select(c => c.Value.ToString()).ToList(); 

                foreach (var row in rows.Skip(1)) 
                {
                    var rowData = new Dictionary<string, string>();

                    for (int i = 0; i < headers.Count; i++)
                    {
                        string header = headers[i];
                        string cellValue = row.Cell(i + 1).Value.ToString();
                        rowData[header] = cellValue;
                    }

                    data.Add(rowData);
                }
            }

            return data;
        }

        public void SaveFile(FileModel fileModel, List<Dictionary<string, string>> data)
        {


            using (var workbook = new XLWorkbook())
            {
                if (data.Count == 0)
                    throw new EmptyFileException();

                var worksheet = workbook.Worksheets.Add("Sheet1");
                var headers = data.First().Keys.ToList();

                for (int i = 0; i < headers.Count; i++)
                {
                    worksheet.Cell(1, i + 1).Value = headers[i];
                }

                for (int rowIdx = 0; rowIdx < data.Count; rowIdx++)
                {
                    var rowData = data[rowIdx];
                    for (int colIdx = 0; colIdx < headers.Count; colIdx++)
                    {
                        worksheet.Cell(rowIdx + 2, colIdx + 1).Value = rowData[headers[colIdx]];
                    }
                }

                workbook.SaveAs(fileModel.Path);
            }
        }
    }
}
