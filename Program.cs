using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        Console.Write("Enter the size of the file in MB: ");
        int fileSizeMB = int.Parse(Console.ReadLine());

        int chunkSizeMB = 100;

        string filePath = "large_file.txt";
        string outputFilePath = "sorted_large_file.txt";
        string tempDir = "temp";
        Directory.CreateDirectory(tempDir);

        GenerateRandomFile(filePath, fileSizeMB);

        var watch = System.Diagnostics.Stopwatch.StartNew();
        SortLargeFile(filePath, tempDir, chunkSizeMB, outputFilePath);
        watch.Stop();

        Console.WriteLine($"File sorted and saved to {outputFilePath}");
        Console.WriteLine($"Sorting time: {watch.Elapsed.TotalSeconds:F2} seconds");
    }

    static void GenerateRandomFile(string filePath, int sizeMB)
    {
        Random random = new Random();
        long totalLines = sizeMB * 1024L * 1024L / 8L;

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (long i = 0; i < totalLines; i++)
            {
                writer.WriteLine(random.Next(0, 1000000));

                if (i % 1000000 == 0)
                {
                    writer.Flush();
                }
            }
        }
    }


    static void MergeFilesWithHeap(List<string> inputFiles, string outputFile)
    {
        using (StreamWriter outFile = new StreamWriter(outputFile))
        {
            var minHeap = new SortedSet<(int value, int index)>();
            var filePointers = new List<StreamReader>();

            for (int i = 0; i < inputFiles.Count; i++)
            {
                StreamReader reader = new StreamReader(inputFiles[i]);
                filePointers.Add(reader);
                string line = reader.ReadLine();
                if (line != null)
                {
                    minHeap.Add((int.Parse(line), i));
                }
            }

            while (minHeap.Count > 0)
            {
                var (smallest, fileIndex) = minHeap.Min;
                minHeap.Remove(minHeap.Min);
                outFile.WriteLine(smallest);

                string nextLine = filePointers[fileIndex].ReadLine();
                if (nextLine != null)
                {
                    minHeap.Add((int.Parse(nextLine), fileIndex));
                }
            }

            foreach (var reader in filePointers)
            {
                reader.Close();
            }
        }
    }

    static void SortLargeFile(string filePath, string tempDir, int chunkSizeMB, string outputFilePath)
    {
        int chunkSize = chunkSizeMB * 1024 * 1024 / 8;
        List<string> tempFiles = new List<string>();

        using (StreamReader inputFile = new StreamReader(filePath))
        {
            List<Task> sortTasks = new List<Task>();

            while (true)
            {
                int[] numbers = new int[chunkSize];
                int count = 0;

                for (int i = 0; i < chunkSize; i++)
                {
                    string line = inputFile.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    numbers[count++] = int.Parse(line);
                }

                if (count == 0)
                {
                    break;
                }

                int[] sortedNumbers = new int[count];
                Array.Copy(numbers, sortedNumbers, count);
                string tempFile = Path.Combine(tempDir, Path.GetRandomFileName());
                tempFiles.Add(tempFile);

                sortTasks.Add(Task.Run(() =>
                {
                    Array.Sort(sortedNumbers);
                    using (StreamWriter writer = new StreamWriter(tempFile))
                    {
                        foreach (int number in sortedNumbers)
                        {
                            writer.WriteLine(number);
                        }
                    }
                }));
            }

            Task.WaitAll(sortTasks.ToArray());
        }

        MergeFilesWithHeap(tempFiles, outputFilePath);

        foreach (var tempFile in tempFiles)
        {
            File.Delete(tempFile);
        }
    }
}
