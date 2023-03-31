using FotNET.NETWORK.OBJECTS.MATH_OBJECTS;

namespace FotNET.NETWORK.MATH.Initialization.HE;

public class HeInitialization : IWeightsInitialization {
    public Matrix Initialize(Matrix matrix) {
        var scale = Math.Sqrt(2.0 / matrix.Columns);
        
        for (var i = 0; i < matrix.Rows; i++)
            for (var j = 0; j < matrix.Columns; j++)
                matrix.Body[i, j] = new Random().NextDouble() * scale * 2 - scale;

        return matrix;
    }
}