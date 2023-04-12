﻿using FotNET.NETWORK.OBJECTS.MATH_OBJECTS;

namespace FotNET.NETWORK.LAYERS.CONVOLUTION.SCRIPTS {
    public static class Padding {
        private static Matrix GetPadding(Matrix matrix, int paddingSize) {
            var newMatrix = new Matrix(matrix.Rows + paddingSize * 2, matrix.Columns + paddingSize * 2);

            for (var i = paddingSize; i < newMatrix.Rows - paddingSize; i++)
                for (var j = paddingSize; j < newMatrix.Columns - paddingSize; j++)
                    newMatrix.Body[i, j] = matrix.Body[i - paddingSize, j - paddingSize];

            return newMatrix;
        }

        public static Tensor GetPadding(Tensor tensor, int paddingSize) {
            var newTensor = new Tensor(new List<Matrix>());
            for (var i = 0; i < tensor.Channels.Count; i++)
                newTensor.Channels.Add(new Matrix(0, 0));
            
            Parallel.For(0, tensor.Channels.Count, i => {
                newTensor.Channels[i] = GetPadding(tensor.Channels[i],  paddingSize);
            });

            return newTensor;
        }
    }
}
