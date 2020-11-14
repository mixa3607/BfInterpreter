using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace Bf
{
    public class ImagesAsBfMemConsoleRenderer
    {
        public ImagesAsBfMemConsoleRenderer(byte bitsInWord, uint wordsInRow, uint rows)
        {
            BitsInWord = bitsInWord;
            WordsInRow = wordsInRow;
            Rows = rows;
        }

        public string ImagesDir { get; set; }
        public int FramesPerSec { get; set; }
        public int ImagesLoadParallelism { get; set; } = 4;

        /// <summary>
        /// Bits count for store value. Max 64.
        /// </summary>
        public byte BitsInWord { get; }

        /// <summary>
        /// Count of words in one line
        /// </summary>
        public uint WordsInRow { get; }

        /// <summary>
        /// Rows
        /// </summary>
        public uint Rows { get; }

        public void Run()
        {
            var renderer = new Render.BfMemoryConsoleRenderer(BitsInWord, WordsInRow, Rows)
            {
                ReverseByteOrder = false,
                AddSeparator = false,
                AddFrameTime = true
            };

            Console.WriteLine("Read and resizing images... ");
            var globalSw = new Stopwatch();
            globalSw.Start();
            var frames = LoadImages(ImagesDir, ImagesLoadParallelism, BitsInWord, (int) WordsInRow, (int) Rows);
            globalSw.Stop();
            Console.WriteLine($"Total {frames.Length} images. Elapsed seconds {globalSw.Elapsed.Seconds}");

            var oneFrameTime = 1000 / FramesPerSec;
            Console.WriteLine($"{oneFrameTime} ms per frame ({FramesPerSec} fps)");

            Delay();

            globalSw.Reset();
            globalSw.Start();

            var overtimeFrames = Render(frames, oneFrameTime, renderer);

            globalSw.Stop();
            Console.WriteLine($"Total render time: {globalSw.Elapsed}. {oneFrameTime}+ frames: {overtimeFrames}");
        }

        public static int Render(ulong[][] frames, int oneFrameTime, Render.BfMemoryConsoleRenderer renderer)
        {
            var overtimeFrames = 0;
            var frameExpectedMs = oneFrameTime;

            var renderSw = new Stopwatch();
            renderSw.Start();
            foreach (var frame in frames)
            {
                renderer.DrawFrame(frame, true);
                var delay = frameExpectedMs - renderSw.ElapsedMilliseconds;

                if (delay > 0)
                    Task.Delay((int) delay).Wait();
                else
                    overtimeFrames++;

                frameExpectedMs += oneFrameTime;
            }

            return overtimeFrames;
        }

        public static void Delay(int delaySecs = 5)
        {
            for (int i = 0; i < delaySecs; i++)
            {
                Console.WriteLine($"Play for {delaySecs - i}s");
                Task.Delay(1000).Wait();
            }
        }

        public static ulong[][] LoadImages(string imagesDir, int imagesLoadParallelism, byte bitsPerWord,
            int wordsInRow, int rows)
        {
            var imagesPaths = Directory.GetFiles(imagesDir);
            var frames = new ulong[imagesPaths.Length][];
            Parallel.For(0, frames.Length, new ParallelOptions()
            {
                MaxDegreeOfParallelism = imagesLoadParallelism
            }, i =>
            {
                var img = ImageHelper.ResizeImage(Image.FromFile(imagesPaths[i]), bitsPerWord * wordsInRow, rows);
                var words = ImageHelper.BitmapToBfMemFrame(img, bitsPerWord);
                frames[i] = words;
            });
            return frames;
        }
    }
}