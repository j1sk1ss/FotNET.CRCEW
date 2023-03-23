using FotNET.NETWORK.OBJECTS.MATH_OBJECTS;

namespace FotNET.NETWORK.LAYERS.RECURRENT.RECURRENCY_TYPE.MANY_TO_MANY;

public class ManyToMany : IRecurrent {
    
    public Tensor GetNextLayer(RecurrentLayer layer, Tensor tensor) {
        var sequence = tensor.Flatten();
        for (var step = 0; step < sequence.Count; step++) {
            var currentElement = sequence[step];
            var inputNeurons = (layer.InputWeights * currentElement).GetAsList().ToArray();
            
            if (step > 0)
                layer.HiddenNeurons.Add(new Vector(new Vector(inputNeurons) 
                                                  + new Vector(layer.HiddenNeurons[step - 1] 
                                                               * layer.HiddenWeights)) + new Vector(layer.HiddenBias));
            else
                layer.HiddenNeurons.Add(inputNeurons);

            layer.HiddenNeurons[^1] = layer.Function.Activate(layer.HiddenNeurons[^1]);
            layer.OutputNeurons.Add((new Vector(layer.HiddenNeurons[^1] * layer.OutputWeights) + layer.OutputBias).Body[0]);
        }
        
        return new Vector(layer.OutputNeurons.ToArray()).AsTensor(1, layer.OutputNeurons.Count, 1);
    }
    
    public Tensor BackPropagate(RecurrentLayer layer, Tensor error, double learningRate) {
        var sequence = error.Flatten();
        var nextHidden = (layer.OutputWeights.Transpose() * sequence[^1]).GetAsList().ToArray();
        
        for (var step = sequence.Count - 1; step >= 0; step--) {
            var currentError = sequence[step];
            
            var inputGradient = (new Vector(nextHidden) * currentError).Body;
            layer.InputWeights  -= new Matrix(inputGradient).Transpose() * learningRate;
            
            if (nextHidden.Length == 0)
                nextHidden = (layer.OutputWeights.Transpose() * currentError).GetAsList().ToArray();
            else {
                nextHidden = (layer.OutputWeights.Transpose() * currentError 
                              + new Vector(nextHidden * layer.HiddenWeights.Transpose())
                                  .AsMatrix(1, layer.OutputWeights.Rows, 0)).GetAsList().ToArray(); 
            }

            nextHidden = layer.Function.Derivation(nextHidden);

            if (step > 0) {
                var hiddenWeightGradient = Matrix.Multiply(new Matrix(layer.HiddenNeurons[step - 1]), new Matrix(nextHidden).Transpose());
                layer.HiddenWeights -= hiddenWeightGradient * learningRate;
                for (var bias = 0; bias < layer.HiddenBias.Length; bias++)
                    layer.HiddenBias[bias] -= hiddenWeightGradient.GetAsList().Average() * learningRate;                
            }
            
            var outputWeightsGradient = (new Vector(layer.HiddenNeurons[step]) * currentError).Body;
            layer.OutputWeights -= new Matrix(outputWeightsGradient) * learningRate;
            layer.OutputBias -= currentError * learningRate;
        }

        return error;
    }
}