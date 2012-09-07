using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMCMLibrary;
using System.IO;
using MMCMLibrary.Modalities;
namespace Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            Network.init();
            Time.turboBoost();

            StreamWriter benchmarkLog;
            string filePath = "optimizationType.txt";
            if (!File.Exists(filePath))
            {
                benchmarkLog = new StreamWriter(filePath, false, UTF8Encoding.Default);
                benchmarkLog.WriteLine("mapW \t mapH \t mapL \t mod1Size \t mod2Size \t connections \t stepTime \t learning \t multiThreading \t inputOptimization");
                benchmarkLog.Close();
            }

            int w = 10;
            int h = 10;
            int l = 1;
            int testOnSteps = 1;
            int modalityLenght1 = 10;
            int modalityLenght2 = 10;
            CVZ_MMCM_GPU.OptimizationType optimizationType = CVZ_MMCM_GPU.OptimizationType.auto;

            int actualConnections = 0;

            for (int mapSize = 8; mapSize <= 16; mapSize *= 2)
            {
                for (int modalitySize = 16; modalitySize <= 1024; modalitySize *= 4)
                {
                    modalityLenght1 = modalitySize * modalitySize;
                    modalityLenght2 = modalitySize * modalitySize;

                    //Vary among layer number
                    for (l = 1; l <= 1; l*=2)
                    {
                        w = mapSize;
                        h = mapSize;
                        actualConnections = (modalityLenght1 + modalityLenght2) * w * h * l;

                        for (int multiThreading = 0; multiThreading <= 1; multiThreading++)
                        {
                            for (int learning = 0; learning <= 1; learning++)
                            {
                                for (int opt = 2; opt <= 2; opt++)
                                {
                                    if (multiThreading == 0)
                                        opt = 10;

                                    if (opt == 0)
                                        optimizationType = CVZ_MMCM_GPU.OptimizationType.map;
                                    if (opt == 1)
                                        optimizationType = CVZ_MMCM_GPU.OptimizationType.input;
                                    if (opt == 2)
                                        optimizationType = CVZ_MMCM_GPU.OptimizationType.auto;

                                    benchmarkLog = new StreamWriter(filePath, true, UTF8Encoding.Default);

                                    //Console.WriteLine("Creating map(" + w + "," + h + "," + l + ")");
                                    CVZ_MMCM map;
                                    if (multiThreading == 0)
                                        map = new CVZ_MMCM("testMap", w, h, l);
                                    else
                                        map = new CVZ_MMCM_GPU("testMap", w, h, l, optimizationType);

                                    //Console.WriteLine("Adding random modality( " + modalityLenght1 + " )");
                                    RandomModality modImage1 = new RandomModality("testMap", "rnd_1", modalityLenght1);
                                    map.AddModality(modImage1, 1.0f);


                                    //Console.WriteLine("Adding random modality( " + modalityLenght2 + " )");
                                    RandomModality modImage2 = new RandomModality("testMap", "rnd_2", modalityLenght2);
                                    map.AddModality(modImage2, 1.0f);

                                    Console.WriteLine("Map : \t" + l + " x (" + w + "x" + h + ")");
                                    Console.WriteLine("Input : \t 2x (" + modalitySize + ")");
                                    Console.WriteLine("Learning : \t" + (learning != 0).ToString());
                                    Console.WriteLine("Multithread : \t" + multiThreading);
                                    Console.WriteLine("Optimization : \t" + optimizationType.ToString());

                                    //Console.WriteLine("Starting benchmark...");
                                     map.LearningRate = learning;

                                    double meanTime = 0;
                                    for (int i = 0; i < testOnSteps; i++)
                                    {
                                        //double t0 = Time.now();
                                        DateTime t0 = DateTime.Now;
                                        map.Step(0.0f);
                                        TimeSpan stepTime = DateTime.Now - t0;
                                        meanTime += stepTime.TotalMilliseconds;
                                        string lineResult = w + "\t"
                                            + h + "\t"
                                            + l + "\t"
                                            + modalityLenght1 + "\t"
                                            + modalityLenght2 + "\t"
                                            + actualConnections + "\t"
                                            + stepTime.TotalMilliseconds + "\t"
                                            + "learning=" + (learning != 0).ToString() + "\t"
                                            + "multithread=" + multiThreading + "\t"
                                            + "optimization=" + optimizationType.ToString();

                                        benchmarkLog.WriteLine(lineResult);
                                        if (stepTime.TotalMilliseconds > 1000)
                                            break;
                                        //Console.WriteLine("T=" + stepTime);
                                    }
                                    meanTime /= testOnSteps;
                                    Console.WriteLine("T= " + meanTime);
                                    Console.WriteLine();
                                    benchmarkLog.Close();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

