using FotNET.NETWORK.OBJECTS.DATA_OBJECTS;

namespace FotNET.NETWORK.FIT;

public static class Fit {
    public static Network FitModel(Network network, List<IData> dataSet, int epochs, double baseLearningRate) {
        for (var epoch = 0; epoch < epochs; epoch++)
            foreach (var datum in dataSet) {
                var predictedClass = network.ForwardFeed(datum.AsTensor());
                var predictedValue = network.GetLayers()[^1].GetValues().Flatten()[predictedClass];

                var expectedClass = datum.GetRight().GetMaxIndex();
                var expectedValue = datum.GetRight().Flatten()[datum.GetRight().GetMaxIndex()];

                if (predictedClass != expectedClass) {
                    network.BackPropagation(expectedClass, expectedValue,
                        baseLearningRate * Math.Pow(.1, epoch));
                    continue;
                }
                
                if (Math.Abs(predictedValue - expectedValue) > .1)
                    network.BackPropagation(expectedClass, expectedValue,
                        baseLearningRate * Math.Pow(.1, epoch));
            }

        return network;
    }

    public static double TestModel(Network network, List<IData> dataSet) =>
        dataSet.Count / (double)(from data in dataSet let prediction = 
            network.ForwardFeed(data.AsTensor()) where prediction == 
                                                       data.GetRight().GetMaxIndex() select data).Count();
    
}