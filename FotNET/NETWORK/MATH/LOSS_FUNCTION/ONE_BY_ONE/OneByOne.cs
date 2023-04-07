namespace FotNET.NETWORK.MATH.LOSS_FUNCTION.ONE_BY_ONE;

public class OneByOne : LossFunction {
    protected override double Derivation(double prediction, double expected) => 
        prediction * (1 - prediction) * (expected - prediction);
}