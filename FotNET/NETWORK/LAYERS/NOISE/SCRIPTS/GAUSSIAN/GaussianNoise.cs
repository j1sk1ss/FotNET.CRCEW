﻿using FotNET.NETWORK.MATH.OBJECTS;

namespace FotNET.NETWORK.LAYERS.NOISE.SCRIPTS.GAUSSIAN;

public class GaussianNoise : INoise {
    public GaussianNoise(double mean = 0, double stdDev = 1) {
        Mean   = mean;
        StdDev = stdDev;
    }
    
    private double Mean { get; }
    private double StdDev { get; }
    
    public Vector GenerateNoise(int size) {
        var noise = new double[size];

        for (var i = 0; i < size; i += 2) {
            var u1 = new Random().NextDouble();
            var u2 = new Random().NextDouble();

            var z1 = Math.Sqrt(-2 * Math.Log(u1)) * Math.Cos(2 * Math.PI * u2);
            var z2 = Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2);

            noise[i] = Mean + StdDev * z1;
                
            if (i + 1 < size) 
                noise[i + 1] = Mean + StdDev * z2;
        }

        return new Vector(noise);
    }
}