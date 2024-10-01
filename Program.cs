/*using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        Console.Write("Enter the size of the file in MB: ");
        int fileSizeMB = int.Parse(Console.ReadLine());

        Console.Write("Enter the memory limit in MB: ");
        int memoryLimitMB = int.Parse(Console.ReadLine());

        Console.Write("Enter the chunk size in MB: ");
        int chunkSizeMB = int.Parse(Console.ReadLine());

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
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int i = 0; i < sizeMB * 1024 * 1024 / 8; i++)
            {
                writer.WriteLine(random.Next(0, 1000000));
            }
        }
    }

    static void MergeFiles(List<string> inputFiles, string outputFile)
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

                Array.Sort(numbers, 0, count);

                string tempFile = Path.Combine(tempDir, Path.GetRandomFileName());
                tempFiles.Add(tempFile);
                using (StreamWriter writer = new StreamWriter(tempFile))
                {
                    for (int i = 0; i < count; i++)
                    {
                        writer.WriteLine(numbers[i]);
                    }
                }
            }
        }

        MergeFiles(tempFiles, outputFilePath);

        foreach (var tempFile in tempFiles)
        {
            File.Delete(tempFile);
        }
    }
}*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        Console.Write("Enter the size of the file in MB: ");
        int fileSizeMB = int.Parse(Console.ReadLine());

        Console.Write("Enter the memory limit in MB: ");
        int memoryLimitMB = int.Parse(Console.ReadLine());

        Console.Write("Enter the chunk size in MB: ");
        int chunkSizeMB = int.Parse(Console.ReadLine());

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
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int i = 0; i < sizeMB * 1024 * 1024 / 8; i++)
            {
                writer.WriteLine(random.Next(0, 1000000));
            }
        }
    }

    static void MergeFiles(List<string> inputFiles, string outputFile)
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
            while (true)
            {
                int[] numbers = new int[chunkSize];
                int count = 0;

                // Зчитуємо блок даних з файлу
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

                // Копіюємо фрагмент і сортуємо
                int[] sortedNumbers = new int[count];
                Array.Copy(numbers, sortedNumbers, count);
                Array.Sort(sortedNumbers);

                // Створюємо тимчасовий файл для збереження відсортованого фрагменту
                string tempFile = Path.Combine(tempDir, Path.GetRandomFileName());
                tempFiles.Add(tempFile);

                // Записуємо відсортовані числа в тимчасовий файл
                using (StreamWriter writer = new StreamWriter(tempFile))
                {
                    for (int i = 0; i < count; i++)
                    {
                        writer.WriteLine(sortedNumbers[i]);
                    }
                }
            }
        }

        // Після сортування і запису всіх блоків, зливаємо їх
        MergeFiles(tempFiles, outputFilePath);

        // Видаляємо тимчасові файли
        foreach (var tempFile in tempFiles)
        {
            File.Delete(tempFile);
        }
    }
}
